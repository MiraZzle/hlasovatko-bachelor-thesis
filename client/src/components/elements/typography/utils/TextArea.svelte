<script lang="ts">
	/**
	 * @file A reusable TextArea component for user input.
	 * It supports binding, placeholder text, and various attributes.
	 */
	let {
		name = '',
		id = '',
		placeholder = '',
		value = $bindable(''),
		required = false,
		disabled = false,
		rows = 4,
		label = null as string | null,
		ariaLabel = null as string | null,
		oninput = null as ((event: Event & { currentTarget: HTMLTextAreaElement }) => void) | null
	} = $props<{
		name?: string;
		id?: string;
		placeholder?: string;
		value?: string;
		required?: boolean;
		disabled?: boolean;
		rows?: number;
		label?: string | null;
		ariaLabel?: string | null;
		oninput?: (event: Event & { currentTarget: HTMLTextAreaElement }) => void;
	}>();

	const defaultId = `textarea-${Math.random().toString(36).substring(2, 9)}`;
	let currentId = id || defaultId;
</script>

<div class="textarea-wrapper">
	{#if label}
		<label for={currentId} class="textarea-wrapper__label">
			{label}
		</label>
	{/if}
	<textarea
		class="textarea-wrapper__textarea"
		bind:value
		{name}
		id={currentId}
		{placeholder}
		{required}
		{disabled}
		{rows}
		aria-label={ariaLabel || label}
		{oninput}
	></textarea>
</div>

<style lang="scss">
	.textarea-wrapper {
		width: 100%;

		&__label {
			display: block;
			margin-bottom: $spacing-xs;
			font-weight: $font-weight-medium;
			font-size: $font-size-sm;
			color: $color-text-secondary;
		}

		&__textarea {
			width: 100%;
			padding: $spacing-sm $spacing-md;
			border: $border-width-thin solid $color-input-border;
			border-radius: $border-radius-md;
			background-color: $color-surface;
			font-size: $font-size-md;
			color: $color-text-primary;
			font-family: inherit;
			line-height: $line-height-base;
			resize: vertical;
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
