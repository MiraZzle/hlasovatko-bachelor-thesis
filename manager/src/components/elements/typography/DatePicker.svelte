<script lang="ts">
	/**
	 * @file A simple date picker component
	 */

	let {
		value = $bindable(''),
		label = '',
		id = '',
		disabled = false,
		required = false,
		min = new Date()
	}: {
		value?: string;
		label?: string;
		id?: string;
		disabled?: boolean;
		required?: boolean;
		min?: Date;
	} = $props();

	// target for label
	const defaultId = `input-${Math.random().toString(36).substring(2, 9)}`;
	let currentId = id || defaultId;
	const today = new Date();

	/**
	 * Formats a Date object into a string to use in the date input.
	 * @param {Date} date The date to format.
	 * @returns {string} The formatted date string (YYYY-MM-DD).
	 */
	function getLocalDateString(date: Date): string {
		const localDate = new Date(date);
		localDate.setMinutes(localDate.getMinutes() - localDate.getTimezoneOffset());
		return localDate.toISOString().slice(0, 10);
	}

	const minDateString = $derived(min ? getLocalDateString(min) : '');
</script>

<div class="input-wrapper">
	{#if label}
		<label for={currentId || undefined} class="input-wrapper__label">
			{label}
		</label>
	{/if}
	<input
		type="date"
		class="input-wrapper__input"
		id={currentId}
		bind:value
		{disabled}
		{required}
		min={minDateString}
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
			box-sizing: border-box;

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

			&::-webkit-calendar-picker-indicator {
				cursor: pointer;
				filter: invert(0.5);
			}
		}
	}
</style>
