<script lang="ts" generics="T extends string | number">
	type Option = { value: T; label: string };

	let {
		options,
		value = $bindable(), // The bindable prop from the parent (Type T)
		label = null as string | null,
		id = '',
		name = '',
		required = false,
		disabled = false,
		ariaLabel = null as string | null,
		onchange = (event: Event & { currentTarget: HTMLSelectElement }) => {
			console.warn('onchange not provided', event.currentTarget.value);
		}
	}: {
		options: Option[];
		value?: T;
		label?: string | null;
		id?: string;
		name?: string;
		required?: boolean;
		disabled?: boolean;
		ariaLabel?: string | null;
		onchange?: (event: Event & { currentTarget: HTMLSelectElement }) => void;
	} = $props();

	const defaultId = `select-${Math.random().toString(36).substring(2, 9)}`;
	let currentId = id || defaultId;

	// Derived state ONLY for setting the select's displayed value attribute (string)
	let displayValue: string = $derived(String(value));

	// Renamed handler for clarity
	function handleChange(event: Event & { currentTarget: HTMLSelectElement }) {
		const selectedStringValue = event.currentTarget.value;
		const selectedOption = options.find((opt) => String(opt.value) === selectedStringValue);

		if (selectedOption) {
			// *** IMPORTANT: Update the original bindable 'value' prop (Type T) ***
			value = selectedOption.value;
		}

		// Call the optional onchange callback prop if provided
		if (onchange) {
			onchange(event);
		}
	}
</script>

<div class="select-wrapper">
	{#if label}
		<label for={currentId} class="select-wrapper__label">{label}</label>
	{/if}
	<div class="select-wrapper__container">
		<select
			class="select-wrapper__select"
			{name}
			id={currentId}
			{required}
			{disabled}
			aria-label={ariaLabel || label}
			value={displayValue}
			on:change={handleChange}
		>
			{#each options as option (option.value)}
				<option value={String(option.value)}>{option.label}</option>
			{/each}
		</select>
		<div class="select-wrapper__icon" aria-hidden="true">
			<svg width="16" height="16" viewBox="0 0 16 16" fill="none" stroke="currentColor">
				<path d="M4 6 L8 10 L12 6" stroke-width="1.5" />
			</svg>
		</div>
	</div>
</div>

<style lang="scss">
	@import '../../../styles/variables.scss';

	.select-wrapper {
		width: 100%; // Or inline-block depending on usage context

		&__label {
			display: block;
			margin-bottom: $spacing-xs;
			font-weight: $font-weight-medium;
			font-size: $font-size-sm;
			color: $color-text-secondary;
		}

		&__container {
			position: relative; // For positioning the icon
			display: inline-block; // Make container wrap the select width
			min-width: 150px; // Example min-width
		}

		&__select {
			appearance: none; // Remove default browser appearance
			-webkit-appearance: none;
			-moz-appearance: none;
			width: 100%;
			padding: $spacing-sm $spacing-xl $spacing-sm $spacing-md; // Space for icon on right
			border: $border-width-thin solid $color-input-border;
			border-radius: $border-radius-md;
			background-color: $color-surface;
			font-size: $font-size-md;
			color: $color-text-primary;
			cursor: pointer;
			transition: border-color $transition-duration-fast $transition-timing-function;

			&:focus {
				outline: none;
				border-color: $color-input-focus-border;
				box-shadow: 0 0 0 2px rgba($color-primary, 0.2);
			}

			&:disabled {
				background-color: $color-surface-alt;
				cursor: not-allowed;
				opacity: 0.7;
			}
		}

		&__icon {
			position: absolute;
			right: $spacing-md;
			top: 50%;
			transform: translateY(-50%);
			pointer-events: none; // Icon shouldn't be interactive
			color: $color-text-secondary;
		}
	}
</style>
