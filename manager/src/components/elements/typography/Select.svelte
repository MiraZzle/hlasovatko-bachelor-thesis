<script lang="ts">
	/**
	 * @file Select component for creating a styled dropdown select input.
	 */
	import type { SelectOption } from '$lib/shared_types';

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
			console.warn('onchange not provided', event.currentTarget.value);
		},
		width = 'auto'
	}: {
		options: SelectOption[];
		value?: string;
		label?: string | null;
		id?: string;
		name?: string;
		required?: boolean;
		disabled?: boolean;
		ariaLabel?: string | null;
		onchange?: (event: Event & { currentTarget: HTMLSelectElement }) => void;
		width?: 'auto' | 'full';
	} = $props();

	const defaultId = `select-${Math.random().toString(36).substring(2, 9)}`;
	let currentId = id || defaultId;

	let displayValue: string = $derived(String(value));

	// custom handler for onchange event
	function handleChange(event: Event & { currentTarget: HTMLSelectElement }): void {
		const selectedStringValue = event.currentTarget.value;
		const selectedOption = options.find((opt) => String(opt.value) === selectedStringValue);

		if (selectedOption) {
			value = selectedOption.value.toString();
		}

		if (onchange) {
			onchange(event);
		}
	}

	// dynamic class based on props
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
			onchange={handleChange}
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
	.select-wrapper {
		&__label {
			display: block;
			margin-bottom: $spacing-xs;
			font-weight: $font-weight-medium;
			font-size: $font-size-sm;
			color: $color-text-secondary;
		}

		&__container {
			position: relative;
			display: inline-block;
			min-width: 150px;

			&--full {
				display: block;
				width: 100%;
				min-width: unset;
			}
		}

		&__select {
			appearance: none;
			-webkit-appearance: none;
			-moz-appearance: none;
			width: 100%;
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
