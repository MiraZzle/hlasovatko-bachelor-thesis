<script lang="ts">
	/**
	 * @file Reusable component for displaying a template row in the dashboard.
	 * Similar to SessionRow, but for templates.
	 */
	import { goto } from '$app/navigation';
	import { tick } from 'svelte';
	import type { Template } from '$lib/templates/types';
	import { formatDate } from '$lib/functions/utils';
	import { toast } from '$lib/stores/toast_store';
	import { get } from 'svelte/store';
	import { base } from '$app/paths';

	let {
		template,
		onDelete = (id: string) => {
			console.warn('onDelete not provided for template', id);
		}
	}: {
		template: Template;
		onDelete?: (id: string) => void;
	} = $props();

	// State for the pop-up menu
	let isMenuOpen = $state(false);
	let formattedDate = $derived(formatDate(template.dateCreated!));

	function getTemplateDetailsUrl(): string {
		return `${base}/overview/templates/${template.id}/overview`.replace(/\/{2,}/g, '/');
	}

	/**
	 * Navigates to the template details page.
	 */
	function navigateToDetails(): void {
		goto(getTemplateDetailsUrl());
	}

	/**
	 * Handler for the "See Details" action in the menu.
	 */
	function handleSeeDetails(event: MouseEvent): void {
		event.stopPropagation();
		navigateToDetails();
		isMenuOpen = false;
	}

	/**
	 * Handler for the delete action in the menu.
	 */
	function handleDelete(event: MouseEvent): void {
		event.stopPropagation();
		onDelete(template.id);
		toast.show(`Template "${template.settings!.title}" deleted.`, 'info');
		isMenuOpen = false;
	}

	/**
	 * Toggles the visibility of the actions menu.
	 */
	async function toggleMenu(event: MouseEvent): Promise<void> {
		event.stopPropagation();
		isMenuOpen = !isMenuOpen;

		if (isMenuOpen) {
			await tick();
			window.addEventListener('click', closeMenuOnClickOutside, { once: true });
		}
	}

	// Closes the menu when a click occurs outside of it
	function closeMenuOnClickOutside(): void {
		isMenuOpen = false;
	}
</script>

<tr class="template-row">
	<td class="template-row__cell template-row__cell--title-code">
		<span class="template-row__title">
			<a href={getTemplateDetailsUrl()} class="template-row__link">{template.settings!.title}</a
			></span
		>
		<span class="template-row__code">(#{template.id})</span>
	</td>

	<td class="template-row__cell template-row__cell--date">{formattedDate}</td>

	<td class="template-row__cell template-row__cell--tags">
		<div class="template-row__tags">
			{#each template.settings!.tags as tag (tag)}
				<span class="template-row__tag">{tag}</span>
			{/each}
		</div>
	</td>

	<td class="template-row__cell template-row__cell--actions">
		<div class="actions-container">
			<button
				class="template-row__action-button"
				aria-label={`Actions for ${template.settings!.title}`}
				onclick={toggleMenu}
				type="button"
			>
				...
			</button>

			{#if isMenuOpen}
				<div class="actions-menu">
					<button class="actions-menu__item" onclick={handleSeeDetails}> See Details </button>
					<button class="actions-menu__item actions-menu__item--delete" onclick={handleDelete}>
						Delete
					</button>
				</div>
			{/if}
		</div>
	</td>
</tr>

<style lang="scss">
	.template-row {
		transition: background-color $transition-duration-fast;

		&:hover {
			background-color: $color-surface-alt;
		}

		&__cell {
			padding: $spacing-sm $spacing-md;
			text-align: left;
			border-bottom: $border-width-thin solid $color-border-light;
			white-space: nowrap;
			color: $color-text-primary;
			vertical-align: middle;

			&--tags {
				white-space: normal;
			}
			&--actions {
				width: 1%;
			}
		}

		&__title {
			display: block;
			font-weight: $font-weight-medium;
			color: $color-text-primary;
		}

		&__link {
			color: $color-text-primary;
			text-decoration: none;
			cursor: pointer;

			&:hover {
				text-decoration: underline;
			}
		}
		&__code {
			display: block;
			font-size: $font-size-xs;
			color: $color-text-secondary;
			margin-top: $spacing-xs * 0.5;
		}

		&__tags {
			display: flex;
			flex-wrap: wrap;
			gap: $spacing-xs;
			max-width: 250px;
		}
		&__tag {
			background-color: $color-surface-alt;
			color: $color-text-secondary;
			padding: $spacing-xs $spacing-sm;
			border-radius: $border-radius-pill;
			font-size: $font-size-xs;
			font-weight: $font-weight-medium;
			white-space: nowrap;
		}
		&__action-button {
			background: none;
			border: none;
			padding: $spacing-xs;
			cursor: pointer;
			color: $color-text-secondary;
			border-radius: $border-radius-circle;
			display: inline-flex;
			align-items: center;
			justify-content: center;
			line-height: 0;

			&:hover {
				background-color: $color-surface-alt;
				color: $color-text-primary;
			}
			&:focus-visible {
				outline: 2px solid $color-primary-light;
				outline-offset: 1px;
			}
		}
	}

	.actions-container {
		position: relative;
		display: flex;
		justify-content: center;
	}

	.actions-menu {
		position: absolute;
		top: 100%;
		right: 0;
		z-index: 10;
		background-color: $color-surface;
		border: 1px solid $color-border-light;
		border-radius: $border-radius-md;
		box-shadow: $box-shadow-lg;
		margin-top: $spacing-xs;
		width: 120px;
		overflow: hidden;

		&__item {
			display: block;
			width: 100%;
			text-align: left;
			padding: $spacing-sm $spacing-md;
			background: none;
			border: none;
			cursor: pointer;
			font-size: $font-size-sm;
			color: $color-text-primary;

			&:hover {
				background-color: $color-surface-alt;
			}
			&--delete {
				color: $color-danger;
				&:hover {
					background-color: rgba($color-danger, 0.1);
				}
			}
		}
	}
</style>
