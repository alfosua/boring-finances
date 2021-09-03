<script lang="ts">
export let reorderRate = 1;

import { v4 as uuid4 } from 'uuid';
import bfapi from '@/services/bfapi';
import inputTracker from '@/actions/inputTracker';

import { fly } from 'svelte/transition';
import { flip } from 'svelte/animate';
import { writable } from 'svelte/store';

import AccountSelect from '@/components/operations/AccountSelect.svelte';
import FinancialUnitSelect from '@/components/operations/FinancialUnitSelect.svelte';
import OperationEntryTableEditorHead from '@/components/operations/OperationEntryTableEditorHead.svelte';
import OperationEntryTypeSelect from '@/components/operations/OperationEntryTypeSelect.svelte';

let operation = writable({
  entries: [],
  notes: [],
  isSelectionModeActive: false,
  valuesToFillOver: {
    dateTime: null,
    account: null,
    financialAccount: null,
    amount: 0.00,
    operationEntryType: null,
  }
});

let actionLogs = [];

let submitPromise;

let operationEntriesReoderTimeout;

async function handleEntryInputFieldFocus(event) {
  const { itemKey } = event.detail;
  const lastChangeOrReorderActionLog = actionLogs.filter(x => x.type === 'change' || x.type === 'reorder').pop();

  const isThereSomeChange = (lastChangeOrReorderActionLog || false)
    && lastChangeOrReorderActionLog.type === 'change';

  if (isThereSomeChange) {
    const operationEntryCompare = await createOperationEntriesComparisonFunction();
    const currentEntryFocused = $operation.entries.find(x => x.itemKey == itemKey);
    const lastEntryChanged = $operation.entries.find(x => x.itemKey == lastChangeOrReorderActionLog.itemKey);
    const fromCurrentSideComparison = operationEntryCompare(currentEntryFocused.value, lastEntryChanged.value);

    const maySomeReorderBeRequestedBecauseOfCurrentItem = isThereSomeChange
      && lastChangeOrReorderActionLog.itemKey === itemKey;

    const maySomeReorderBecauseOfAnotherItemMoveTheCurrentOne = isThereSomeChange
      && lastChangeOrReorderActionLog.itemKey != itemKey
      && lastEntryChanged.position > currentEntryFocused.position
      ? fromCurrentSideComparison > 0
      : fromCurrentSideComparison < 0;

    const shouldAvoidReorder = maySomeReorderBeRequestedBecauseOfCurrentItem
      || maySomeReorderBecauseOfAnotherItemMoveTheCurrentOne;
          
    if (shouldAvoidReorder) {
      tryCancelOperationEntriesReorderIfAny();
    }
  }
}

function handleEntryInputFieldBlur() {
  const lastChangeOrReorderActionLog = actionLogs.filter(x => x.type === 'change' || x.type === 'reorder').pop();
  const reorderIsNeeded =  (lastChangeOrReorderActionLog || false)
    && lastChangeOrReorderActionLog.type === 'change';
        
  if (reorderIsNeeded) {
    tryRequestOperationEntriesReorder();
  }
}

function handleEntryInputFieldChange(event) {
  const { itemKey, fieldKey, value } = event.detail;
  actionLogs = [...actionLogs, { type: "change", itemKey, fieldKey, value }];
}

function tryRequestOperationEntriesReorder() {
  if (!operationEntriesReoderTimeout) {
    operationEntriesReoderTimeout = setTimeout(
      reorderEntriesToOperation,
      reorderRate * 1000,
    );
  }
}

function tryCancelOperationEntriesReorderIfAny() {
  if (operationEntriesReoderTimeout) {
    clearTimeout(operationEntriesReoderTimeout);
    operationEntriesReoderTimeout = null;
    console.log('operation entries reorder timeout was cleared');
  }
}

async function reorderEntriesToOperation() {
  const operationEntriesCompare = await createOperationEntriesComparisonFunction();

  $operation.entries = $operation.entries.sort((left, right) => {
    return operationEntriesCompare(left.value, right.value)
  }).map((operationEntry, i) => ({...operationEntry, position: i}));

  actionLogs = [...actionLogs, { type: "reorder" }];

  if (operationEntriesReoderTimeout) {
    operationEntriesReoderTimeout = null;
  }
}

async function createOperationEntriesComparisonFunction() {
  const accounts = await bfapi.accounts().all();
  const financialUnits = await bfapi.financialUnits().all();
  const now = new Date();
  const stringFallback = '\uffff';
  const dateTimeFallback = new Date(
    now.getFullYear() + 1000,
    now.getMonth(),
    now.getDate(),
  );
  const accountTitlesById = accounts.reduce(
    (a, x) => ({...a, [x.id]: x.title ? x.title.toLowerCase() : undefined}), {}
  );
  const financialUnitKebabsById = financialUnits.reduce(
    (a, x) => ({...a, [x.id]: x.kebab ? x.kebab.toLowerCase() : undefined}), {}
  );

  const resolveOperationEntryType = o => o.operationEntryType || Number.MAX_SAFE_INTEGER;
  const resolveDateTime = o => o.dateTime ? new Date(o.dateTime) : dateTimeFallback;
  const resolveFinancialUnit = o => o.financialUnit ? financialUnitKebabsById[o.financialUnit] : stringFallback;
  const resolveAmount = o => o.amount || Number.MAX_SAFE_INTEGER;
  const resolveAccount = o => o.account ? accountTitlesById[o.account] : stringFallback;

  const result = (left, right) => {
    const operationEntryTypeOrder = resolveOperationEntryType(left) - resolveOperationEntryType(right);
    const dateTimeOrder = +(resolveDateTime(left)) - +(resolveDateTime(right));
    const financialUnitOrder = resolveFinancialUnit(left).localeCompare(resolveFinancialUnit(right));
    const amountOrder = resolveAmount(left) - resolveAmount(right);
    const accountOrder = resolveAccount(left).localeCompare(resolveAccount(right));

    const order = operationEntryTypeOrder
      || dateTimeOrder
      || financialUnitOrder
      || amountOrder
      || accountOrder;

    return order;
  }
  
  return result;
}

function addNoteToOperation() {
  $operation.notes = [...$operation.notes, ""];
}

function deleteNoteToOperation(index) {
  $operation.notes = $operation.notes.filter((_, i) => i != index);
}

function addEntryToOperation() {
  $operation.entries = [...$operation.entries, {
    itemKey: uuid4(),
    position: $operation.entries.length,
    selected: false,
    value: {
      dateTime: '',
      account: '',
      financialUnit: '',
      amount: 0.00,
      operationEntryType: '',
    },
  }];
}

function deleteEntryToOperation(index) {
  $operation.entries = $operation.entries.filter((_, i) => i != index);
}

async function createOperation(operation) {
  const result = await bfapi.operations().createOne(operation);
  return result;
}

function handleSubmit() {
  submitPromise = createOperation($operation);
}

function reorderingDetector(node) {
  node.addEventListener('inputblur', handleEntryInputFieldBlur);
  node.addEventListener('inputchange', handleEntryInputFieldChange);
  node.addEventListener('inputfocus', handleEntryInputFieldFocus);

  return {
    destroy() {
      node.removeEventListener('inputblur', handleEntryInputFieldBlur);
      node.removeEventListener('inputchange', handleEntryInputFieldChange);
      node.removeEventListener('inputfocus', handleEntryInputFieldFocus);
    }
  };
}
</script>
  
<form on:submit|preventDefault={handleSubmit} class="account-form" method="POST">
  <button type="button" on:click={tryRequestOperationEntriesReorder}>Reorder</button>

  {#if submitPromise}
    {#await submitPromise}
      <p>Creating account...</p>
    {:catch error}
    <div class="error-container">
      <button
        on:click={() => submitPromise = undefined}
        class='error-container-toogle'
        >X</button>
      <p><strong>Something went wrong:</strong></p>
      <pre class="error-message-container">{error.message}</pre>
    </div>
    {/await}
  {/if}

  <fieldset>
    <p>
      <legend><h2>Entries</h2></legend>
    </p>
    <table class="operation-entry-table">
      {#if $operation.entries && $operation.entries.length}
        <colgroup>
          <col class="operation-entry-column-toolkits" />
          <col class="operation-entry-column-selector" />
          <col class="operation-entry-column-datetime" />
          <col class="operation-entry-column-account" />
          <col class="operation-entry-column-financial-unit" />
          <col class="operation-entry-column-amount" />
          <col class="operation-entry-column-entry-type" />
        </colgroup>
        <OperationEntryTableEditorHead formState={operation} /> 
          {#each $operation.entries as operationEntry, i (operationEntry.itemKey)}
            <tr
              class="operation-entry-row"
              data-itemKey={operationEntry.itemKey}
              use:inputTracker
              use:reorderingDetector
              animate:flip="{{duration: d => 45 * Math.sqrt(d)}}"
              in:fly={{ y: i > 0 ? -20 : 0 }}
            >
              <td class="operation-entry-row-column-toggle">
                <input
                  id="operation-entry-selector-{operationEntry.itemKey}"
                  type="checkbox"
                  bind:checked="{operationEntry.selected}"
                  on:change="{e => {
                    const isThereSomeEntrySelected = $operation.entries.some(x => x.selected);
                    $operation.isSelectionModeActive = isThereSomeEntrySelected;
                  }}"
                />
              </td>
              <td>
                <input
                  id="operation-entry-datetime-{operationEntry.itemKey}"
                  type="datetime-local"
                  bind:value={operationEntry.value.dateTime}
                  required
                  />
              </td>
              <td class="operation-entry-row-column-account-{operationEntry.itemKey}">
                <AccountSelect
                  id="operation-entry-account-{operationEntry.itemKey}"
                  bind:value={operationEntry.value.account}
                  required
                />
              </td>
              <td>
                <FinancialUnitSelect
                  id="operation-entry-financial-unit-{operationEntry.itemKey}"
                  bind:value={operationEntry.value.financialUnit}
                  required
                />
              </td>
              <td>
                <input
                  id="operation-entry-amount-{operationEntry.itemKey}"
                  type="number"
                  bind:value={operationEntry.value.amount}
                  min="0.00"
                  step="any"
                  required
                  />
              </td>
              <td>
                <OperationEntryTypeSelect
                  id="operation-entry-type-{operationEntry.itemKey}"
                  bind:value={operationEntry.value.operationEntryType}
                  required
                />
              </td>
              <!--<button type="button" on:click={() => deleteEntryToOperation(i)}>X</button>-->
            </tr>
          {/each}
      {:else}
        <tr>No entries here.</tr>
      {/if}
      <tfoot>
        <button type="button" on:click={addEntryToOperation}>+</button>
      </tfoot>
    </table>
  </fieldset>

  <fieldset>
    <p>
      <legend>Notes</legend>
      <button type="button" on:click={addNoteToOperation}>+</button>
    </p>
    {#if $operation.notes && $operation.notes.length}
      {#each $operation.notes as _, i (i)}
        <p>
          <input bind:value={$operation.notes[i]} />
          <button type="button" on:click={() => deleteNoteToOperation(i)}>X</button>
        </p>
      {/each}
    {:else}
        <p>No notes here.</p>
    {/if}
  </fieldset>
  
  <button type="submit">Create</button>

  <code style="white-space: pre; text-align: left">
    {JSON.stringify({ operation: $operation, actionLogs: actionLogs }, null, 4)}
  </code>
</form>

<style>
.account-form {
  display: flex;
  flex-direction: column;
}
.error-container {
  display: flex;
  padding: 2em;
  background-color: crimson;
  color: white;
  border-radius: 1em;
  flex-direction: column;
  align-items: center;
  contain: content;
}
.error-message-container {
  height: 15em;
  max-width: 100%;
  overflow: scroll;
  text-align: left;
  resize: vertical;
}

.error-container-toogle {
  position: fixed;
  top: 1em;
  right: 1em;
}

.operation-entry-column-toolkits {
  width: 2.5em;
}
</style>
