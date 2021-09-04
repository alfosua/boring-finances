<script lang="ts">
import bfapi from "@/services/bfapi";

import { onMount } from "svelte";
import { writable } from "svelte/store";

type OperationEntryTypeJson = {
  id: string,
  code: string,
};

export let value = '';
export let required = false;
export let operationEntryTypes = writable<OperationEntryTypeJson[]>([]);

let operationEntryTypePromise: Promise<OperationEntryTypeJson[]>;

onMount(() => {
  operationEntryTypePromise = getOperationEntryTypes();
});

async function getOperationEntryTypes() {
  if (!$operationEntryTypes || !$operationEntryTypes.length) {
    $operationEntryTypes = await bfapi.operations().entries().types();
  }
  return $operationEntryTypes;
}
</script>

<select
  id={$$props.id}
  bind:value
  {required}
  >
  {#if operationEntryTypePromise}
    {#await operationEntryTypePromise}
      <option value="" disabled selected>Loading options..</option>
    {:then operationEntryTypes}
      <option value="" disabled selected>Select an option..</option>
      {#each operationEntryTypes as operationEntryType (operationEntryType.id)}
        <option value="{operationEntryType.id}">
          {operationEntryType.code}
        </option>
      {/each}
    {/await}
  {:else}
    <slot name="error">Any operation entry type couldn't be loaded.</slot>
  {/if}
</select>