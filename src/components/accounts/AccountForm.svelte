<script>
export let ref;

import { BORING_FINANCES_WEBAPI_URL } from '../../../utils/constants';

import { writable } from 'svelte/store'

let account = writable({
  title: '',
  kebab: '',
  accountType: 'Good',
});

let submitPromise;
let accountTypesPromise = getAccountTypes();

async function getAccountTypes() {
  const response = await fetch(`${BORING_FINANCES_WEBAPI_URL}/accounts/types`);
  const accountTypes = await response.json()

  return accountTypes;
}

async function createAccount(account) {
  const response = await fetch(`${BORING_FINANCES_WEBAPI_URL}/accounts`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(account),
  });
  
  if (response.ok) {
    const result = await response.json();
    return result;
  } else {
    const message = await response.text();
    throw new Error(message);
  }
};

function handleSubmit()
{
  submitPromise = createAccount($account);
}
</script>

<div {ref} class="account-form-container">

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

<form on:submit|preventDefault={handleSubmit} class="account-form" method="POST">
  <span class="account-form-field">
    <label for="title">Title</label>
    <input type="text" id='title' bind:value={$account.title} required/>
  </span>
  <span class="account-form-field">
    <label for='kebab'>Kebab</label>
    <input type="text" id='kebab' bind:value={$account.kebab} required/>
  </span>
  <span class="account-form-field">
    <label for="account-type">Type</label>
    <select id='account-type' bind:value={$account.accountType} required>
      {#await accountTypesPromise}
        <option value="" disabled>Loading account types...</option>
      {:then accountTypes}
        <option value="">Select an option...</option>
        {#each accountTypes as accountType (accountType.id)}
        <option value="{accountType.id.toString()}">{accountType.code}</option>
        {/each}
      {/await}
    </select>
  </span>
  <button type="submit">Create</button>
</form>

</div>

<style>
.account-form-container {
  padding: 2em;
}
.account-form {
  display: flex;
  flex-direction: column;
}
.account-form-field {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  padding-left: 10%;
  padding-right: 10%;
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
</style>
