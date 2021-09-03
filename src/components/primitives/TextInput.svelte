<script>
export let value = '';
</script>

<input id={$$props.id} class="text-input" type="text" placeholder="some text" bind:value/>

<style lang="scss">
:global {
  @import 'primitives';
}
</style>