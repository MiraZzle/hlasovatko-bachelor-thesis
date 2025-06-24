<script lang="ts">
	/**
	 * @file AddFromBankModal component for selecting and adding activities from a predefined bank.
	 */
	import type { Snippet } from 'svelte';
	import type { Activity } from '$lib/activities/types';
	import Button from '$components/elements/typography/Button.svelte';
	import ModalDialog from '$components/elements/modals/ModalDialog.svelte';

	let {
		open = $bindable(false),
		activities = [],
		onAdd
	}: {
		open?: boolean;
		activities?: Activity[];
		onAdd: (activities: Activity[]) => void;
	} = $props();

	let selectedActivityIds: string[] = $state([]);

	function handleAdd() {
		const activitiesToAdd = activities.filter((activity) =>
			selectedActivityIds.includes(activity.id)
		);

		onAdd(activitiesToAdd);
		handleClose();
	}

	function handleClose() {
		selectedActivityIds = [];
		open = false;
	}
</script>

<ModalDialog bind:open onclose={handleClose} width="md">
	<div class="add-from-bank">
		<header class="add-from-bank__header">
			<h2 class="add-from-bank__title">Add from Activity Bank</h2>
		</header>
		<div class="add-from-bank__body">
			<div class="add-from-bank__list">
				{#each activities as activity}
					<label for={activity.id} class="add-from-bank__item">
						<input
							type="checkbox"
							id={activity.id}
							value={activity.id}
							bind:group={selectedActivityIds}
							class="add-from-bank__item-checkbox"
						/>
						<div class="add-from-bank__item-details">
							<span class="add-from-bank__item-title">{activity.title}</span>
							<span class="add-from-bank__item-type">{activity.type.replace('_', ' ')}</span>
						</div>
					</label>
				{/each}
			</div>
		</div>
		<footer class="add-from-bank__footer">
			<Button onclick={handleClose} variant="outline">Cancel</Button>
			<Button onclick={handleAdd} disabled={selectedActivityIds.length === 0}>Add Selected</Button>
		</footer>
	</div>
</ModalDialog>

<style lang="scss">
	.add-from-bank {
		display: flex;
		flex-direction: column;
		height: 65vh;

		&__header {
			flex-shrink: 0;
		}

		&__title {
			margin: 0;
			font-size: $font-size-2xl;
		}

		&__body {
			flex-grow: 1;
			overflow-y: auto;
			padding: $spacing-md 0;
		}

		&__list {
			display: flex;
			flex-direction: column;
			gap: $spacing-sm;
		}

		&__item {
			display: flex;
			align-items: center;
			gap: $spacing-md;
			padding: $spacing-sm $spacing-md;
			border-radius: $border-radius-md;
			transition: background-color $transition-duration-fast;
			cursor: pointer;

			&:hover {
				background-color: $color-surface-alt;
			}
		}

		&__item-checkbox {
			width: 20px;
			height: 20px;
			accent-color: $color-primary;
			flex-shrink: 0;
		}

		&__item-details {
			display: flex;
			flex-direction: column;
		}

		&__item-title {
			font-weight: $font-weight-medium;
		}

		&__item-type {
			font-size: $font-size-sm;
			color: $color-text-secondary;
			text-transform: capitalize;
		}

		&__footer {
			flex-shrink: 0;
			margin-top: auto;
			padding-top: $spacing-lg;
			border-top: $border-width-thin solid $color-border-light;
			display: flex;
			justify-content: flex-end;
			gap: $spacing-md;
		}
	}
</style>
