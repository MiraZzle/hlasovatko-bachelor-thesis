<script lang="ts" generics="T extends string | number">
	type Option = { value: T; label: string };

	// Define props for the component
	type Props = {
		options: Option[];
		value?: T; // Bindable
		label?: string | null;
		id?: string;
		name?: string;
		required?: boolean;
		disabled?: boolean;
		ariaLabel?: string | null;
		onchange?: (event: Event & { currentTarget: HTMLSelectElement }) => void;
		width?: 'auto' | 'full'; // <<< ADDED: width prop with 'auto' or 'full'
	};

	let {
		options,
		value = $bindable(),
		label = null,
		id = '',
		name = '',
		required = false,
		disabled = false,
		ariaLabel = null,
		onchange = (event) => {
			console.warn('onchange not provided for Select', event.currentTarget.value);
		},
		width = 'auto' // Default width to 'auto'
	}: Props = $props();

	const defaultId = `select-${Math.random().toString(36).substring(2, 9)}`;
	let currentId = id || defaultId;

	// Derived state for the select's displayed value (always a string for native select)
	let displayValue: string = $derived(String(value));

	// Handler for when the native select's value changes
	function handleChange(event: Event & { currentTarget: HTMLSelectElement }) {
		const selectedStringValue = event.currentTarget.value;
		// Find the original option to get the correct type (T) for the bound value
		const selectedOption = options.find((opt) => String(opt.value) === selectedStringValue);

		if (selectedOption) {
			// Update the bindable 'value' prop with the original type
			value = selectedOption.value;
		}

		// Call the optional onchange callback prop if provided
		if (onchange) {
			onchange(event);
		}
	}

	// --- NEW: Derived class for the container based on width prop ---
	let containerClasses = $derived(
		`select-wrapper__container ${width === 'full' ? 'select-wrapper__container--full' : ''}`
	);
</script>

<div class="select-wrapper">
	{#if label}
		<label for={currentId} class="select-wrapper__label">{label}</label>
	{/if}
	<div class={containerClasses}>
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
	@import '../../../styles/variables.scss'; // Adjust path if needed

	// Block: select-wrapper
	.select-wrapper {
		// If width is 'full', the wrapper itself should also be full width
		// This is handled by its parent or by adding a modifier here if needed.
		// For now, assuming parent controls the overall block width.
		// If select-wrapper needs to be 100% when width='full', add:
		// &.select-wrapper--full { width: 100%; }

		// Element: Label
		&__label {
			display: block;
			margin-bottom: $spacing-xs;
			font-weight: $font-weight-medium;
			font-size: $font-size-sm;
			color: $color-text-secondary;
		}

		// Element: Container (for select and icon)
		&__container {
			position: relative;
			display: inline-block; // Default: wraps content width
			min-width: 150px; // Default minimum width

			// Modifier: Full width
			&--full {
				display: block; // Change display to block for full width
				width: 100%; // Take full width of its parent
				min-width: unset; // Remove min-width when full
			}
		}

		// Element: Native select
		&__select {
			appearance: none;
			-webkit-appearance: none;
			-moz-appearance: none;
			width: 100%; // Select always takes full width of its container
			padding: $spacing-sm $spacing-xl $spacing-sm $spacing-md;
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

		// Element: Dropdown Icon
		&__icon {
			position: absolute;
			right: $spacing-md;
			top: 50%;
			transform: translateY(-50%);
			pointer-events: none;
			color: $color-text-secondary;
		}
	}
</style>
