<script lang="ts">
	// Define the props the component accepts
	type Props = {
		type?: 'text' | 'email' | 'password' | 'number';
		name?: string;
		id?: string;
		placeholder?: string;
		value?: string; // Bindable
		required?: boolean;
		disabled?: boolean;
		label?: string | null;
		ariaLabel?: string | null;
		oninput?: (event: Event & { currentTarget: HTMLInputElement }) => void; // Corrected event type
		onkeydown?: (event: KeyboardEvent & { currentTarget: HTMLInputElement }) => void; // <<< ADDED: onkeydown prop
		class?: string; // Allow passing additional classes
	};

	let {
		type = 'text',
		name = '',
		id = '',
		placeholder = '',
		value = $bindable(''),
		required = false,
		disabled = false,
		label = null,
		ariaLabel = null,
		oninput,
		onkeydown, // <<< ADDED: Destructure onkeydown
		class: customClass = ''
	}: Props = $props();

	const defaultId = `input-${Math.random().toString(36).substring(2, 9)}`;
	let currentId = id || defaultId;

	// Combine internal and passed classes for the input element itself
	let inputClasses = `input-wrapper__input ${customClass}`.trim();
</script>

<div class="input-wrapper">
	{#if label}
		<label for={currentId} class="input-wrapper__label">
			{label}
		</label>
	{/if}
	<input
		class={inputClasses}
		bind:value
		{type}
		{name}
		id={currentId}
		{placeholder}
		{required}
		{disabled}
		aria-label={ariaLabel || label}
		{oninput}
		{onkeydown}
	/>
</div>

<style lang="scss">
	@import '../../../styles/variables.scss'; // Adjust path if needed

	// Block: input-wrapper
	.input-wrapper {
		width: 100%; // Make wrapper take full width by default

		&__label {
			display: block;
			margin-bottom: $spacing-xs;
			font-weight: $font-weight-medium;
			font-size: $font-size-sm;
			color: $color-text-secondary;
		}

		// Styles for the native input element
		&__input {
			// This is the base class applied by the component
			width: 100%;
			padding: $spacing-sm $spacing-md;
			border: $border-width-thin solid $color-input-border;
			border-radius: $border-radius-md;
			background-color: $color-surface;
			font-size: $font-size-md;
			color: $color-text-primary;
			transition: border-color $transition-duration-fast $transition-timing-function;

			&::placeholder {
				color: $color-text-disabled;
				opacity: 1; // Reset opacity for browsers like Firefox
			}

			&:focus {
				outline: none;
				border-color: $color-input-focus-border;
				box-shadow: 0 0 0 2px rgba($color-primary, 0.2); // Optional focus ring shadow
			}

			&:disabled {
				background-color: $color-surface-alt;
				cursor: not-allowed;
				opacity: 0.7;
			}
		}
	}
</style>
