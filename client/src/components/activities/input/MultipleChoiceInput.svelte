<script lang="ts">
	/**
	 * @file A reusable component for selecting one or more options from a list.
	 */
	import type { ActivityOption } from '$lib/shared_types';

	let {
		options,
		allowMultiple = false,
		selected = $bindable(allowMultiple ? [] : null),
		disabled = false
	}: {
		options: ActivityOption[];
		allowMultiple?: boolean;
		selected?: string | string[] | null;
		disabled?: boolean;
	} = $props();

	/**
	 * Handles the selection of an option.
	 * If `allowMultiple` is true, it toggles the selection state of the option
	 */
	function handleSelection(optionId: string): void {
		if (disabled) return;

		if (allowMultiple) {
			const currentSelection = Array.isArray(selected) ? [...selected] : [];
			const index = currentSelection.indexOf(optionId);
			if (index > -1) {
				currentSelection.splice(index, 1); // Deselect
			} else {
				currentSelection.push(optionId); // Select
			}
			selected = currentSelection;
		} else {
			selected = optionId;
		}
	}

	/**
	 * Determines if a given option is currently selected
	 */
	function isSelected(optionId: string): boolean {
		if (allowMultiple) {
			return Array.isArray(selected) && selected.includes(optionId);
		}
		return selected === optionId;
	}
</script>

<div class="choice-input">
	<ul class="choice-input__list">
		{#each options as option (option.id)}
			<li class="choice-input__item">
				<button
					type="button"
					class="choice-input__button"
					class:choice-input__button--selected={isSelected(option.id)}
					onclick={() => handleSelection(option.id)}
					{disabled}
					aria-pressed={isSelected(option.id)}
				>
					<span class="choice-input__text">{option.text}</span>
					{#if isSelected(option.id)}
						<span class="choice-input__check-icon">✓</span>
					{/if}
				</button>
			</li>
		{/each}
	</ul>
</div>

<style lang="scss">
	.choice-input {
		width: 100%;

		&__list {
			list-style: none;
			padding: 0;
			margin: 0;
			display: grid;
			gap: $spacing-md;
			grid-template-columns: 1fr;

			@media (min-width: $breakpoint-sm) {
				grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
			}
		}

		&__button {
			display: flex;
			align-items: center;
			justify-content: center;
			width: 100%;
			padding: $spacing-md $spacing-lg;
			font-size: $font-size-md;
			font-weight: $font-weight-medium;
			text-align: center;
			border: 2px solid $color-border-light;
			border-radius: $border-radius-md;
			background-color: $color-surface;
			color: $color-text-primary;
			cursor: pointer;
			transition: all $transition-duration-fast ease-in-out;
			position: relative;

			&:hover:not(:disabled) {
				border-color: $color-primary-light;
				transform: translateY(-2px);
				box-shadow: $box-shadow-sm;
			}

			&:active:not(:disabled) {
				transform: translateY(-1px);
			}

			&--selected {
				border-color: $color-primary;
				background-color: $color-primary;
				color: $color-text-on-primary;
				font-weight: $font-weight-semibold;
				box-shadow: $box-shadow-sm;

				&:hover:not(:disabled) {
					background-color: lighten($color-primary, 5%);
				}
			}

			&:disabled {
				background-color: $color-surface-alt;
				border-color: $color-border-light;
				color: $color-text-disabled;
				cursor: not-allowed;
			}
		}

		&__text {
			flex-grow: 1;
		}

		&__check-icon {
			font-weight: $font-weight-bold;
			margin-left: $spacing-sm;
			font-size: 1.2em;
			line-height: 1;
		}
	}
</style>
