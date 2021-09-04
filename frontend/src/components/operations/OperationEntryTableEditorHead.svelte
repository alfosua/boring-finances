<script lang="ts">
import { fade, fly } from 'svelte/transition';

import ColumnValueFiller from '@/components/commons/ColumnValueFiller.svelte';
import DateTimeInput from '@/components/primitives/DateTimeInput.svelte';
import AccountSelect from '@/components/operations/AccountSelect.svelte';
import FinancialUnitSelect from '@/components/operations/FinancialUnitSelect.svelte';
import NumberInput from '@/components/primitives/NumberInput.svelte';
import OperationEntryTypeSelect from '@/components/operations/OperationEntryTypeSelect.svelte';

import type { Writable } from 'svelte/store';

export let formState: Writable<any>;

let headers = [
  {
    key: "dateTime",
    title: "Date And Time",
    Component: DateTimeInput,
    selectedValuesProvider: createSelectedValuesProvider(
      x => x.value.dateTime
    ),
    valueFillingHandler: createValueFillingHandler(
      (valueToFillOver) => ({ dateTime: valueToFillOver })
    ),
  },
  {
    key: "account",
    title: "Account",
    Component: AccountSelect,
    selectedValuesProvider: createSelectedValuesProvider(
      x => x.value.account
    ),
    valueFillingHandler: createValueFillingHandler(
      (valueToFillOver) => ({ account: valueToFillOver })
    ),
  },
  {
    key: "financialUnit",
    title: "Financial Unit",
    Component: FinancialUnitSelect,
    selectedValuesProvider: createSelectedValuesProvider(
      x => x.value.account
    ),
    valueFillingHandler: createValueFillingHandler(
      (valueToFillOver) => ({ financialUnit: valueToFillOver }),
    ),
  },
  {
    key: "amount",
    title: "Amount",
    Component: NumberInput,
    selectedValuesProvider: createSelectedValuesProvider(
      x => x.value.amount
    ),
    valueFillingHandler: createValueFillingHandler(
      (valueToFillOver) => ({ amount: valueToFillOver }),
    ),
  },
  {
    key: "operationEntryType",
    title: "Entry Type",
    Component: OperationEntryTypeSelect,
    selectedValuesProvider: createSelectedValuesProvider(
      x => x.value.amount
    ),
    valueFillingHandler: createValueFillingHandler(
      (valueToFillOver) => ({ operationEntryType: valueToFillOver }),
    ),
  },
];

function createSelectedValuesProvider(projection) {
  const result = () => $formState.entries.filter(x => x.selected).map(projection);

  return result;
}

function createValueFillingHandler(decorator) {
  const result = (event) => {
    $formState.selectedAll = false;
    $formState.isSelectionModeActive = false;
    $formState.entries = $formState.entries.map(x => ({
      ...x,
      selected: false,
      value: x.selected
        ? { ...x.value, ...decorator(event.detail.valueToFillOver) }
        : x.value,
    }));

    return result;
  };

  return result;
}

function toogleAllOperationEntriesSelection(checked: boolean) {
  $formState.selectedAll = checked;
  $formState.isSelectionModeActive = checked;
  $formState.entries = $formState.entries.map(x => ({
    ...x,
    selected: checked,
  }));
}

function deleteOperationEntriesSelection() {
  $formState.selectedAll = false;
  $formState.isSelectionModeActive = false;
  $formState.entries = $formState.entries.filter(x => !x.selected);
}
</script>

{#key null}
  <tr class="operation-entry-header" transition:fly>
    <th class="bulk-edition-toolkit-column" rowspan="{Math.max(($formState.entries.length || 0), 5) + 2}">
      {#if $formState.isSelectionModeActive}
        <div transition:fade class="bulk-edition-toolkit-item-group">
          <button
            class="delete-button bulk-edition-toolkit-item"
            type="button"
            on:click="{deleteOperationEntriesSelection}"
          />
        </div>
      {/if}
    </th>
    <td>
      <input
        type="checkbox"
        bind:checked="{$formState.selectedAll}"
        on:change="{e => toogleAllOperationEntriesSelection(e.currentTarget.checked)}"
      />
    </td>
    {#each headers as header (header.key)}
      <th class="operation-entry-table-header-column">
        <span class="operation-entry-table-header-column-content">
          <span class="operation-entry-table-header-column-title">{header.title}</span>
          <ColumnValueFiller
            class="operation-entry-table-header-column-filler"
            selectedValuesProvider={header.selectedValuesProvider}
            on:valuefilling={header.valueFillingHandler}
            bind:valueToFillOver={$formState.valuesToFillOver[header.key]}
            bind:canFill={$formState.isSelectionModeActive}
          >
            <svelte:component this={header.Component} bind:value={$formState.valuesToFillOver[header.key]} />
          </ColumnValueFiller>
        </span>
      </th>
    {/each}
  </tr>
{/key}

<style lang="scss">
:global {
  @import 'operations';
}

.delete-button {
  display: inline-block;
  vertical-align: baseline;
  position: relative;
  width: 1.5em;
  height: 1.5em;
  border: none;
  bottom: 0;
  padding: 0;
  margin: 0;
  text-decoration: none;
  background: transparent;
  color: transparent;
  font-family: sans-serif;
  font-size: 1rem;
  line-height: 1;
  cursor: pointer;
  text-align: right;
  transition: background 250ms ease-in-out, transform 150ms ease;
  -webkit-appearance: none;
  -moz-appearance: none;
  background-image: url(/assets/icons/trash.svg);
  background-size: 1.5em;
}

.delete-button:hover,
.delete-button:focus {
  border: 0 0 0 transparent;
}

.delete-button:focus {
  border: 0 0 0 transparent;
}

.delete-button:active {
  border: 0 0 0 transparent;
}

.bulk-edition-toolkit-column {
  height: 100%;
  vertical-align: top;
}

.bulk-edition-toolkit-item-group {
  position: relative;
  display: inline-block;
  height: 100%;
  margin: 1em 0.5em 0 0;
  flex-direction: column;
  align-items: flex-start;
}

.bulk-edition-toolkit-item {
  margin-bottom: 1em;
}
</style>