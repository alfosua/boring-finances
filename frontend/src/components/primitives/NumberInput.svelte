<script>
export let value = '';
</script>

<input id={$$props.id} class="number-input" type="number" bind:value/>

<style lang="scss">
:global {
  @import 'primitives';
}
</style>