<script lang="ts">
	/**
	 * @file A multi-select dropdown component with checkboxes.
	 */
	import type { SelectOption } from '$lib/shared_types';
	import { onMount } from 'svelte';

	let {
		options,
		selectedValues = $bindable(),
		label = null,
		placeholder = 'Select options'
	}: {
		options: SelectOption[];
		selectedValues?: string[];
		label?: string | null;
		placeholder?: string;
	} = $props();

	let isOpen = $state(false);
	let multiselectElement: HTMLElement | undefined;
	const defaultId = `input-${Math.random().toString(36).substring(2, 9)}`;

	onMount(() => {
		function handleClickOutside(event: MouseEvent) {
			if (isOpen && multiselectElement && !multiselectElement.contains(event.target as Node)) {
				isOpen = false;
			}
		}
		// Handle outside clicks to close the dropdown
		document.addEventListener('click', handleClickOutside);
		return () => {
			document.removeEventListener('click', handleClickOutside);
		};
	});

	function toggleDropdown() {
		isOpen = !isOpen;
	}

	/**
	 * Handles changes to the checkbox selections.
	 */
	function handleCheckboxChange(event: Event & { currentTarget: HTMLInputElement }) {
		const { value, checked } = event.currentTarget;
		const currentValues = selectedValues ?? [];
		if (checked) {
			selectedValues = [...currentValues, value];
		} else {
			selectedValues = currentValues.filter((v) => v !== value);
		}
	}
</script>

<div class="multiselect" bind:this={multiselectElement}>
	{#if label}
		<label class="multiselect__label" for={defaultId}>{label}</label>
	{/if}
	<button
		class="multiselect__trigger"
		onclick={toggleDropdown}
		aria-haspopup="listbox"
		aria-expanded={isOpen}
	>
		<span>{placeholder}</span>
		<div class="multiselect__icon" class:rotated={isOpen}>
			<svg width="16" height="16" viewBox="0 0 16 16" fill="none" stroke="currentColor">
				<path d="M4 6 L8 10 L12 6" stroke-width="1.5" />
			</svg>
		</div>
	</button>

	{#if isOpen}
		<div class="multiselect__panel" role="listbox" id={defaultId}>
			{#each options as option (option.value)}
				<div class="multiselect__option">
					<input
						type="checkbox"
						id={`ms-checkbox-${String(option.value)}`}
						value={String(option.value)}
						checked={(selectedValues ?? []).includes(String(option.value))}
						onchange={handleCheckboxChange}
						class="multiselect__checkbox"
					/>
					<label for={`ms-checkbox-${String(option.value)}`} class="multiselect__option-label"
						>{option.label}</label
					>
				</div>
			{/each}
		</div>
	{/if}
</div>

<style lang="scss">
	.multiselect {
		position: relative;
		min-width: 200px;

		&__label {
			display: block;
			margin-bottom: $spacing-xs;
			font-weight: $font-weight-medium;
			font-size: $font-size-sm;
			color: $color-text-secondary;
		}

		&__trigger {
			display: flex;
			justify-content: space-between;
			align-items: center;
			width: 100%;
			padding: $spacing-sm $spacing-md;
			border: $border-width-thin solid $color-input-border;
			border-radius: $border-radius-md;
			background-color: $color-surface;
			font-size: $font-size-md;
			color: $color-text-primary;
			cursor: pointer;
			text-align: left;

			&:focus {
				outline: none;
				border-color: $color-input-focus-border;
				box-shadow: 0 0 0 2px rgba($color-primary, 0.2);
			}
		}

		&__panel {
			position: absolute;
			top: 100%;
			left: 0;
			right: 0;
			z-index: 10;
			margin-top: $spacing-xs;
			border: $border-width-thin solid $color-input-border;
			border-radius: $border-radius-md;
			background-color: $color-surface;
			box-shadow: $box-shadow-md;
			max-height: 200px;
			overflow-y: auto;
		}

		&__option {
			display: flex;
			align-items: center;
			padding: $spacing-sm $spacing-md;
			cursor: pointer;

			&:hover {
				background-color: $color-surface-alt;
			}
		}

		&__checkbox {
			margin-right: $spacing-sm;
			cursor: pointer;
			width: 1rem;
			height: 1rem;
			accent-color: $color-primary;
		}

		&__option-label {
			flex-grow: 1;
			cursor: pointer;
			user-select: none;
		}
	}
</style>
