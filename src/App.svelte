<script>
const BORING_FINANCES_WEBAPI_URL = 'https://localhost:44364/api/v0';
const ACCOUNT_ICON_IMAGE_URL = "https://image.flaticon.com/icons/png/512/2942/2942269.png";

let accountPromise = getAccounts();

async function getAccounts() {
	const response = await fetch(`${BORING_FINANCES_WEBAPI_URL}/accounts`);
	const accounts = response.json();

	return accounts;
}

async function getOperations() {
	const response = await fetch(`${BORING_FINANCES_WEBAPI_URL}/operations`);
	const operations = response.json();

	return operations
}
</script>

<main>
	<h1>Boring Finances</h1>
	<div>
	<div>
		{#await accountPromise}
		<p>Loading accounts...</p>
		{:then accounts}
		<ul class="account-boxes-container">
			{#each accounts as account}
			<li class="account-box"><a href="/account/{account.id}">
				<img alt="account icon" src="{ACCOUNT_ICON_IMAGE_URL}" />
				{account.title}
			</a></li>
			{/each}
		</ul>
		{/await}
	</div>
	</div>
</main>

<style>
	main {
		text-align: center;
		padding: 1em;
		max-width: 240px;
		margin: 0 auto;
	}

	h1 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 4em;
		font-weight: 100;
	}

	.account-boxes-container {
		display: flex;
		list-style-type: none;
		flex-direction: column;
	}
	
	.account-box {
		display: flex;
		flex-direction: row;
		align-items: center;
	}

	.account-box img {
		width: 3em;
	}

	@media (min-width: 640px) {
		main {
			max-width: none;
		}
	}
</style>