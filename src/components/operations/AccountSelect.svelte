<script lang="ts">
import bfapi from "@/services/bfapi";

import { onMount } from "svelte";
import { writable } from "svelte/store";

type AccountJson = {
  id: string,
  title: string,
  kebab: string,
  accountType: {id: string, code: string, href: string},
};

type AccountTypeJson = {
  id: string,
  code: string,
};

export let value = '';
export let required = false;
export let accounts = writable<AccountJson[]>([]);
export let accountTypes = writable<AccountTypeJson[]>([]);

let accountInfoPromise: Promise<{ accounts: AccountJson[], accountTypes: AccountTypeJson[] }>;

onMount(() => {
  accountInfoPromise = getAccountInfo();
});

async function getAccountInfo() {
  const [accounts, accountTypes] = await Promise.all([getAccounts(), getAccountTypes()]);
  const result = { accounts, accountTypes };
  return result;
}

async function getAccounts() {
  if (!$accounts || !$accounts.length) {
    $accounts = await bfapi.accounts().all();
  }
  return $accounts;
}

async function getAccountTypes() {
  if (!$accountTypes || !$accountTypes.length) {
    $accountTypes = await bfapi.accounts().types();
  }
  return $accountTypes;
}

function createAccountAbstract(account: AccountJson, accountTypes: AccountTypeJson[]) {
  const targetAccountType = accountTypes.find(type => type.id == account.accountType.id);
  const abstract = `${ targetAccountType.code } :: ${account.title}`
  return abstract;
}
</script>

<select
  id={$$props.id}
  bind:value
  {required}
  >
  {#if accountInfoPromise}
    {#await accountInfoPromise}
      <option value="" disabled selected>Loading options..</option>
    {:then accountInfo}
      <option value="" disabled selected>Select an option..</option>
      {#each accountInfo.accounts as account (account.id)}
        <option value="{account.id}">
          { createAccountAbstract(account, accountInfo.accountTypes) }
        </option>
      {/each}
    {/await}
  {:else}
    <slot name="error">Any account couldn't be loaded.</slot>
  {/if}
</select>