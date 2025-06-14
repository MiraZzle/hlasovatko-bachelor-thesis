<script lang="ts">
	import { slide } from 'svelte/transition';
	import ChevronIcon from '$components/icons/ChevronIcon.svelte';

	export let isOpen = false;

	function toggleOpen(): void {
		isOpen = !isOpen;
	}

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
			<ChevronIcon />
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
	.accordion-item {
		width: 100%;
		border-bottom: $border-width-thin solid $color-border-light;
		cursor: pointer;
		padding: $spacing-md 0;

		&:focus-visible {
			outline: 2px solid $color-primary-light;
			outline-offset: 2px;
			border-radius: $border-radius-sm;
		}

		&__header {
			display: flex;
			justify-content: space-between;
			align-items: center;
			width: 100%;
			background: none;
			border: none;
			padding: 0;
			text-align: left;
		}

		&__question {
			font-size: $font-size-lg;
			font-weight: $font-weight-medium;
			color: $color-text-primary;
			margin: 0;
			flex-grow: 1;
			padding-right: $spacing-md;
		}

		&__icon-wrapper {
			display: flex;
			align-items: center;
			flex-shrink: 0;
			transition: transform $transition-duration-base $transition-timing-function;

			&--open {
				transform: rotate(180deg);
			}
		}

		&__content {
			overflow: hidden;
		}

		&__content-inner {
			padding: $spacing-sm 0 $spacing-lg;
			font-size: $font-size-md;
			color: $color-text-secondary;
			line-height: $line-height-base;
			:global(p) {
				margin-bottom: $spacing-sm;
				&:last-child {
					margin-bottom: 0;
				}
			}
		}
	}
</style>
