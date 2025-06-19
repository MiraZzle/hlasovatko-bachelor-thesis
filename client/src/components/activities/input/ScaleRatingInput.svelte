<script lang="ts">
	/**
	 * @file A reusable component for selecting a single value from a numerical scale.
	 */

	let {
		min,
		max,
		minLabel = String(min),
		maxLabel = String(max),
		value = $bindable(null),
		disabled = false
	}: {
		min: number;
		max: number;
		minLabel?: string;
		maxLabel?: string;
		value?: number | null;
		disabled?: boolean;
	} = $props();

	// Generate an array of options based on the min and max values
	let options = $derived(Array.from({ length: max - min + 1 }, (_, i) => min + i));

	/**
	 * Handles the selection of a rating value.
	 */
	function handleSelect(optionValue: number): void {
		if (disabled) return;
		value = optionValue;
	}
</script>

<div class="scale-rating-input">
	<div class="scale-rating-input__options">
		{#each options as option (option)}
			<button
				type="button"
				class="scale-rating-input__option-button"
				class:scale-rating-input__option-button--selected={value === option}
				onclick={() => handleSelect(option)}
				{disabled}
				aria-pressed={value === option}
			>
				{option}
			</button>
		{/each}
	</div>
	<div class="scale-rating-input__labels">
		<span class="scale-rating-input__label scale-rating-input__label--min">{minLabel}</span>
		<span class="scale-rating-input__label scale-rating-input__label--max">{maxLabel}</span>
	</div>
</div>

<style lang="scss">
	.scale-rating-input {
		width: 100%;

		&__options {
			display: flex;
			justify-content: space-between;
			gap: $spacing-sm;
			margin-bottom: $spacing-sm;
		}

		&__labels {
			display: flex;
			justify-content: space-between;
			font-size: $font-size-sm;
			color: $color-text-secondary;
			padding: 0 $spacing-xs;
		}

		&__option-button {
			flex: 1;
			padding: $spacing-sm;
			font-size: $font-size-md;
			font-weight: $font-weight-medium;
			border: 2px solid $color-border-light;
			border-radius: $border-radius-md;
			background-color: $color-surface;
			color: $color-text-primary;
			cursor: pointer;
			transition: all $transition-duration-fast ease-in-out;
			aspect-ratio: 1 / 1;
			display: flex;
			align-items: center;
			justify-content: center;

			&:hover:not(:disabled) {
				border-color: $color-primary-light;
				background-color: rgba($color-primary, 0.05);
				transform: translateY(-2px);
			}

			&--selected {
				border-color: $color-primary;
				background-color: $color-primary;
				color: $color-text-on-primary;
				font-weight: $font-weight-semibold;
				transform: translateY(-2px) scale(1.05);
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
