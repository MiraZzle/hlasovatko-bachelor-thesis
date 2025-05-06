<script lang="ts">
	import type { ActivityOption } from '$lib/types'; // Assuming types are in $lib

	type Props = {
		options: ActivityOption[];
		allowMultiple?: boolean;
		// Bindable prop for selected option(s)
		selected?: string | string[] | null; // string for single, string[] for multiple
		disabled?: boolean;
	};

	let {
		options,
		allowMultiple = false,
		selected = $bindable(allowMultiple ? [] : null),
		disabled = false
	}: Props = $props();

	function handleSelection(optionId: string) {
		if (disabled) return;

		if (allowMultiple) {
			let currentSelection = Array.isArray(selected) ? [...selected] : [];
			const index = currentSelection.indexOf(optionId);
			if (index > -1) {
				currentSelection.splice(index, 1); // Deselect
			} else {
				currentSelection.push(optionId); // Select
			}
			selected = currentSelection;
		} else {
			selected = optionId; // Single selection
		}
	}

	function isSelected(optionId: string): boolean {
		if (allowMultiple) {
			return Array.isArray(selected) && selected.includes(optionId);
		}
		return selected === optionId;
	}
</script>

<div class="mc-input">
	<ul class="mc-input__options-list">
		{#each options as option (option.id)}
			<li class="mc-input__option-item">
				<button
					type="button"
					class="mc-input__option-button"
					class:mc-input__option-button--selected={isSelected(option.id)}
					onclick={() => handleSelection(option.id)}
					{disabled}
					aria-pressed={isSelected(option.id)}
				>
					{option.text}
				</button>
			</li>
		{/each}
	</ul>
</div>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

	.mc-input {
		width: 100%;

		&__options-list {
			list-style: none;
			padding: 0;
			margin: 0;
			display: grid;
			gap: $spacing-sm;
			// Responsive columns for options
			grid-template-columns: 1fr; // Single column by default
			@media (min-width: $breakpoint-xs) {
				// Adjust breakpoint as needed
				grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
			}
		}

		&__option-button {
			display: block;
			width: 100%;
			padding: $spacing-md $spacing-lg;
			font-size: $font-size-md;
			font-weight: $font-weight-medium;
			text-align: center;
			border: 2px solid $color-border;
			border-radius: $border-radius-md;
			background-color: $color-surface;
			color: $color-text-primary;
			cursor: pointer;
			transition: all $transition-duration-fast;

			&:hover:not(:disabled) {
				border-color: $color-primary-light;
				background-color: rgba($color-primary, 0.05);
			}

			&--selected {
				border-color: $color-primary;
				background-color: $color-primary;
				color: $color-text-on-primary;
				font-weight: $font-weight-semibold;
				box-shadow: $box-shadow-sm;
			}

			&:disabled {
				background-color: $color-surface-alt;
				border-color: $color-border-light;
				color: $color-text-disabled;
				cursor: not-allowed;
			}
		}
	}
</style>
