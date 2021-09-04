<script lang="ts">
import AccountSelect from "@/components/operations/AccountSelect.svelte";
import FinancialUnitSelect from "@/components/operations/FinancialUnitSelect.svelte";
import OperationEntryTypeSelect from "@/components/operations/OperationEntryTypeSelect.svelte";
import OperationForm from '@/components/operations/OperationForm.svelte';

</script>

<main>
	<h1>Boring Finances</h1>
	<AccountSelect />
	<FinancialUnitSelect />
	<OperationEntryTypeSelect />
	<OperationForm />
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

	@media (min-width: 640px) {
		main {
			max-width: none;
		}
	}
</style>