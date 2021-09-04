<script>
import { createEventDispatcher } from "svelte";
import { fade, slide } from "svelte/transition";
import clickout from "../../actions/clickout";

const dispatch = createEventDispatcher();

export let valueToFillOver = null;
export let canFill = true;
export let clearingValue = null;
export let selectedValuesProvider = () => [];

let isFillingValues = false;
let isUsingCommonValue = false;

let toogle;

function handleToogleClick() {
  isFillingValues = !isFillingValues;

  if (isFillingValues && selectedValuesProvider && !valueToFillOver) {
    const selectedValues = selectedValuesProvider();
    const allAreEqual = selectedValues != null && selectedValues.length > 0
      && selectedValues.every(x => x == selectedValues[0]);

    if (allAreEqual) {
      valueToFillOver = selectedValues[0];
      isUsingCommonValue = true;
    }
  }
}

function handleClickout() {
  isFillingValues = false;

  if (valueToFillOver && isUsingCommonValue) {
    clearValueToFillOver();
    isUsingCommonValue = false;
  }
}

function handleFillClick() {
  dispatchValueFillingEvent(valueToFillOver);

  clearValueToFillOver();
  isFillingValues = false;
  canFill = false;
}

function clearValueToFillOver() {
  valueToFillOver = clearingValue;
}

function dispatchValueFillingEvent() {
  dispatch('valuefilling', {
    valueToFillOver,
  });
}

</script>

<span
  class="{`${$$props.class} fill-button-container-root`}"
  use:clickout
  on:clickout="{handleClickout}"
  >
  {#if canFill}
    <span class="fill-button-container" transition:fade={{duration: 150}}>
      <button type="button" class="fill-button" bind:this="{toogle}" on:click={handleToogleClick}/>
      {#if isFillingValues}
        <div class="filler-overlay" in:slide out:slide>
          
          <slot />

          <button type="button"
            on:click="{handleFillClick}"
            disabled="{!valueToFillOver}"
            >Fill</button>
        </div>
      {/if}
    </span>
  {/if}
</span>

<style>
.fill-button-container-root {
  display: inline-block;
  width: 2em;
  padding: 0;
}

.fill-button-container {
  display: inline-block;
  position: relative;
  vertical-align: baseline;
  width: 100%;
  height: 100%;
  margin: 0;
  padding: 0;
}

.fill-button {
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
  text-align: center;
  transition: background 250ms ease-in-out, transform 150ms ease;
  -webkit-appearance: none;
  -moz-appearance: none;
  background-image: url(/assets/icons/fill.svg);
  background-size: 1.5em;
}

.fill-button:hover,
.fill-button:focus {
  border: 0 0 0 transparent;
}

.fill-button:focus {
  border: 0 0 0 transparent;
}

.fill-button:active {
  border: 0 0 0 transparent;
}

.filler-overlay {
  position: absolute;
  transform: translate(-50%, 0);
  padding: 1em;
  top: 2em;
  left: 50%;
  background-color: white;
  border-radius: 1em;
  box-shadow: 0.3em 0.3em 0.9em rgba(0, 0, 0, 0.25);
}
</style>