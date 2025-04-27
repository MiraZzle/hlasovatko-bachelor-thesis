<script lang="ts">
	// Using Svelte 4 style export let for state management
	// Parent can optionally pass `isOpen` to control it, otherwise default is false.
	export let isOpen = false;

	// Import transition if needed
	import { slide } from 'svelte/transition';
	// Assuming ChevronIcon exists at this path based on new structure
	import ChevronIcon from '$components/icons/ChevronIcon.svelte';

	function toggleOpen(): void {
		isOpen = !isOpen;
	}

	// Generate a unique ID for accessibility (aria-controls)
	const uniqueId = `accordion-content-${Math.random().toString(36).substring(2, 9)}`;
</script>

<div
	class="accordion-item"
	role="button"
	tabindex="0"
	aria-expanded={isOpen}
	aria-controls={uniqueId}
	on:click={toggleOpen}
	on:keydown={(event) => event.key === 'Enter' && toggleOpen()}
>
	<div class="accordion-item__header">
		<h4 class="accordion-item__question">
			<slot name="question"></slot>
		</h4>
		<div class="accordion-item__icon-wrapper" class:accordion-item__icon-wrapper--open={isOpen}>
			<ChevronIcon class="accordion-item__icon" />
		</div>
	</div>

	{#if isOpen}
		<div id={uniqueId} class="accordion-item__content" transition:slide={{ duration: 300 }}>
			<div class="accordion-item__content-inner">
				<slot name="answer"></slot>
			</div>
		</div>
	{/if}
</div>

<style lang="scss">
	@import '../../../../styles/variables.scss'; // Import variables

	.accordion-item {
		width: 100%;
		border-bottom: $border-width-thin solid $color-border-light; // Use variable
		cursor: pointer;
		padding: $spacing-md 0; // Use variable for vertical padding, remove horizontal

		&:focus-visible {
			// Enhanced focus style
			outline: 2px solid $color-primary-light;
			outline-offset: 2px;
			border-radius: $border-radius-sm; // Optional: visual cue on focus
		}

		&__header {
			display: flex;
			justify-content: space-between;
			align-items: center;
			width: 100%;
			background: none; // Keep background transparent
			border: none; // No border on header itself
			padding: 0; // No padding on header itself
			text-align: left;
			// cursor: pointer; // Cursor is already on the main div
		}

		&__question {
			font-size: $font-size-lg; // Use variable
			font-weight: $font-weight-medium; // Use variable
			color: $color-text-primary; // Use variable
			margin: 0;
			flex-grow: 1;
			padding-right: $spacing-md; // Space between text and icon
		}

		&__icon-wrapper {
			display: flex;
			align-items: center;
			flex-shrink: 0;
			transition: transform $transition-duration-base $transition-timing-function; // Use variable

			// Class applied when isOpen is true
			&--open {
				transform: rotate(180deg);
			}
		}

		&__icon {
			// Style the icon if needed (size, color) - assuming ChevronIcon takes class
			width: 20px;
			height: 20px;
			stroke: $color-text-secondary; // Use variable
			color: $color-text-secondary; // Use variable (for fill potentially)
		}

		&__content {
			// margin-top: $spacing-sm; // Use variable, adjusted from 12px
			overflow: hidden; // Keep for slide transition
		}

		&__content-inner {
			padding: $spacing-sm 0 $spacing-lg; // Add padding top/bottom inside content
			font-size: $font-size-md; // Use variable
			color: $color-text-secondary; // Use variable
			line-height: $line-height-base; // Use variable

			// Style direct children (like <p>) within the answer slot
			:global(p) {
				margin-bottom: $spacing-sm;
				&:last-child {
					margin-bottom: 0;
				}
			}
		}
	}
</style>
