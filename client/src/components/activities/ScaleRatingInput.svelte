<script lang="ts">
	type Props = {
		min: number;
		max: number;
		minLabel?: string;
		maxLabel?: string;
		value?: number | null;
		disabled?: boolean;
	};

	let {
		min,
		max,
		minLabel = String(min),
		maxLabel = String(max),
		value = $bindable(null),
		disabled = false
	}: Props = $props();

	let options = $derived(Array.from({ length: max - min + 1 }, (_, i) => min + i));

	function handleSelect(optionValue: number) {
		if (disabled) return;
		value = optionValue;
	}
</script>

<div class="scale-rating-input">
	<div class="scale-rating-input__labels">
		<span class="scale-rating-input__label scale-rating-input__label--min">{minLabel}</span>
		<span class="scale-rating-input__label scale-rating-input__label--max">{maxLabel}</span>
	</div>
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
</div>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

	.scale-rating-input {
		width: 100%;

		&__labels {
			display: flex;
			justify-content: space-between;
			font-size: $font-size-sm;
			color: $color-text-secondary;
			margin-bottom: $spacing-xs;
			padding: 0 $spacing-xs; // Align with buttons
		}

		&__options {
			display: flex;
			justify-content: space-between;
			gap: $spacing-xs; // Small gap between buttons
		}

		&__option-button {
			flex: 1; // Distribute space equally
			padding: $spacing-sm;
			font-size: $font-size-md;
			font-weight: $font-weight-medium;
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
