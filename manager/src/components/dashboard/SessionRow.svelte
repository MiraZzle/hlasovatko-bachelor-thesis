<script lang="ts">
	import { goto } from '$app/navigation';
	import type { Session } from '$lib/sessions/types';
	import { tick } from 'svelte';
	import { onDestroy } from 'svelte';
	import { formatDate } from '$lib/functions/utils';

	let {
		session,
		onDelete = (id: string) => {
			console.warn('onDelete handler not provided for session', id);
		}
	}: {
		session: Session;
		onDelete?: (id: string) => void;
	} = $props();

	let isMenuOpen = $state(false);
	let formattedDate = $derived(formatDate(session.created));

	// Navigate to the session details page
	function navigateToDetails(): void {
		goto(`/overview/sessions/${session.id}/overview`);
	}

	// Handler for see action menu
	function handleSeeDetails(event: MouseEvent): void {
		event.stopPropagation();
		navigateToDetails();
		isMenuOpen = false;
	}

	// Toggle the actions menu
	async function toggleMenu(event: MouseEvent): Promise<void> {
		event.stopPropagation();
		isMenuOpen = !isMenuOpen;

		if (isMenuOpen) {
			await tick();
			window.addEventListener('click', closeMenuOnClickOutside, { once: true });
		}
	}

	function closeMenuOnClickOutside(): void {
		isMenuOpen = false;
	}

	// Handler for delete action in the actions menu
	function handleDelete(event: MouseEvent): void {
		event.stopPropagation();
		onDelete(session.id);
		isMenuOpen = false;
	}

	const statusModifier = $derived(() => session.status.toLowerCase());

	onDestroy(() => {
		window.removeEventListener('click', closeMenuOnClickOutside);
	});
</script>

<tr class="session-row">
	<td class="session-row__cell session-row__cell--title-code">
		<span class="session-row__title">{session.title}</span>
		<span class="session-row__code">({session.templateID})</span>
	</td>

	<td class="session-row__cell session-row__cell--date">{formattedDate}</td>

	<td class="session-row__cell session-row__cell--status">
		<span class="session-row__status session-row__status--{statusModifier}">
			{session.status}
		</span>
	</td>

	<td class="session-row__cell session-row__cell--participants">{session.participants}</td>

	<td class="session-row__cell session-row__cell--actions">
		<div class="actions-container">
			<button
				class="session-row__action-button"
				aria-label={`Actions for session ${session.title}`}
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
	.session-row {
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

			&--date {
				white-space: nowrap;
			}

			&--participants {
				text-align: left;
				padding-right: $spacing-xl;
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

		&__code {
			display: block;
			font-size: $font-size-xs;
			color: $color-text-secondary;
			margin-top: $spacing-xs * 0.5;
		}

		&__status {
			display: inline-block;
			border-radius: $border-radius-pill;
			font-size: $font-size-xs;
			font-weight: $font-weight-medium;
			text-transform: uppercase;
			white-space: nowrap;

			&--inactive {
				background-color: $color-surface-alt;
				color: $color-text-secondary;
			}
			&--active {
				background-color: rgba($color-success, 0.15);
				color: darken($color-success, 10%);
			}
			&--finished {
				color: $color-text-disabled;
				background-color: $color-surface-alt;
			}
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
