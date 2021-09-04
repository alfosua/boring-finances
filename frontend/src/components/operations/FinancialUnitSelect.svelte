<script lang="ts">
import bfapi from "@/services/bfapi";

import { onMount } from "svelte";
import { writable } from "svelte/store";

type FinancialUnitJson = {
  id: string,
  kebab: string,
  financialUnitType: {id: string, code: string, href: string},
};

export let value = '';
export let required = false;
export let financialUnits = writable<FinancialUnitJson[]>([]);

let financialUnitPromise: Promise<FinancialUnitJson[]>;

onMount(() => {
  financialUnitPromise = getFinancialUnits();
});

async function getFinancialUnits() {
  if (!$financialUnits || !$financialUnits.length) {
    $financialUnits = await bfapi.financialUnits().all();
  }
  return $financialUnits;
}
</script>

<select
  id={$$props.id}
  bind:value
  {required}
  >
  {#if financialUnitPromise}
    {#await financialUnitPromise}
      <option value="" disabled selected>Loading options..</option>
    {:then financialUnits}
      <option value="" disabled selected>Select an option..</option>
      {#each financialUnits as financialUnit (financialUnit.id)}
        <option value="{financialUnit.id}">
          {financialUnit.kebab}
        </option>
      {/each}
    {/await}
  {:else}
    <slot name="error">Any financial unit couldn't be loaded.</slot>
  {/if}
</select>