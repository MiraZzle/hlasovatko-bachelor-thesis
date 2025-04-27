<script lang="ts">
	// Define the props the component accepts
	// Note: 'value' is now just a regular prop with a default.
	// Svelte handles the binding when the parent uses bind:value={$someState}
	let {
		type = 'text' as 'text' | 'email' | 'password' | 'number',
		name = '',
		id = '',
		placeholder = '',
		value = $bindable(''),
		required = false,
		disabled = false,
		label = null as string | null,
		ariaLabel = null as string | null // For accessibility when label is not visible
	} = $props<{
		type?: 'text' | 'email' | 'password' | 'number';
		name?: string;
		id?: string;
		placeholder?: string;
		value?: string; // This prop can be bound by the parent
		required?: boolean;
		disabled?: boolean;
		label?: string | null;
		ariaLabel?: string | null;
	}>();

	// Generate default id if not provided
	// Use $derived if id generation depends on reactive props, but here it's simple initialization logic.
	const defaultId = `input-${Math.random().toString(36).substring(2, 9)}`;
	let currentId = id || defaultId; // Use a let if id prop could change reactively, otherwise const is fine.
</script>

<div class="input-wrapper">
	{#if label}
		<label for={id} class="input-wrapper__label">
			{label}
		</label>
	{/if}
	<input
		class="input-wrapper__input"
		bind:value
		{type}
		{name}
		{id}
		{placeholder}
		{required}
		{disabled}
		aria-label={ariaLabel || label}
	/>
</div>

<style lang="scss">
	@import '../../../styles/variables.scss';

	.input-wrapper {
		width: 100%; // Make wrapper take full width by default

		&__label {
			display: block;
			margin-bottom: $spacing-xs;
			font-weight: $font-weight-medium;
			font-size: $font-size-sm;
			color: $color-text-secondary;
		}

		&__input {
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

		// Potential style for error state
		// &.input-wrapper--error {
		//     .input-wrapper__input {
		//         border-color: $color-error;
		//         &:focus {
		//             box-shadow: 0 0 0 2px rgba($color-error, 0.2);
		//         }
		//     }
		//     .input-wrapper__error {
		//         display: block; // Show error message
		//         color: $color-error;
		//         font-size: $font-size-xs;
		//         margin-top: $spacing-xs;
		//     }
		// }
	}
</style>
