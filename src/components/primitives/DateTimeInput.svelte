<script>
export let value = '';
</script>

<input id={$$props.id} class="datetime-input" type="datetime-local" bind:value/>

<style lang="scss">
:global {
  @import 'primitives';
}
</style>