<script lang="ts">
	/**
	 * @file A reusable Input component for user input.
	 * It supports various input types, binding,
	 * placeholder text, and additional attributes.
	 */
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
		onkeydown,
		class: customClass = ''
	}: {
		type?: 'text' | 'email' | 'password' | 'number';
		name?: string;
		id?: string;
		placeholder?: string;
		value?: string;
		required?: boolean;
		disabled?: boolean;
		label?: string | null;
		ariaLabel?: string | null;
		oninput?: (event: Event & { currentTarget: HTMLInputElement }) => void;
		onkeydown?: (event: KeyboardEvent & { currentTarget: HTMLInputElement }) => void;
		class?: string;
	} = $props();

	const defaultId = `input-${Math.random().toString(36).substring(2, 9)}`;
	let currentId = id || defaultId;

	// dynamic class based on props
	let inputClasses = `input-wrapper__input ${customClass}`.trim();
</script>

<div class="input-wrapper">
	{#if label}
		<label for={currentId || undefined} class="input-wrapper__label">
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
		aria-invalid={required && value === ''}
	/>
</div>

<style lang="scss">
	.input-wrapper {
		width: 100%;

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
				opacity: 1;
			}

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
	}
</style>
