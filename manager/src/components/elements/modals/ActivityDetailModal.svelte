<script lang="ts">
	/**
	 * @file Modal dialog component for displaying activity details.
	 * Provides a structured view of activity information including type, categories, and definition.
	 */
	import ModalDialog from '$components/elements/modals/ModalDialog.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import type { Activity } from '$lib/activities/types';

	let {
		open = $bindable(false),
		activity,
		onclose = () => {
			open = false;
		}
	}: {
		open?: boolean;
		activity: Activity | null;
		onclose?: () => void;
	} = $props();

	// Format the activity definition as JSON or return a string representation
	function getFormattedDefinition(): object | string {
		if (!activity?.definition) return '{}';
		try {
			const jsonObj = activity.definition;
			return JSON.stringify(jsonObj, null, 2);
		} catch (e) {
			return activity.definition;
		}
	}

	// try to parse the activity definition as JSON and format it
	const formattedDefinition = $derived(getFormattedDefinition());

	const titleId = 'activity-detail-title';
</script>

<ModalDialog bind:open {onclose} width="md" {titleId}>
	{#if activity}
		<h2 id={titleId} class="detail-modal__title">{activity.title}</h2>

		<div class="detail-modal__content">
			<div class="detail-modal__item">
				<span class="detail-modal__label">Type</span>
				<span class="detail-modal__value">{activity.type}</span>
			</div>

			<div class="detail-modal__item">
				<span class="detail-modal__label">Categories</span>
				<div class="detail-modal__tags">
					{#if activity.tags!.length}
						{#each activity.tags! as category}
							<span class="detail-modal__tag">{category}</span>
						{/each}
					{:else}
						<span>No categories assigned.</span>
					{/if}
				</div>
			</div>

			<div class="detail-modal__item">
				<span class="detail-modal__label">Definition</span>
				<pre class="detail-modal__definition"><code>{formattedDefinition}</code></pre>
			</div>
		</div>

		<div class="detail-modal__actions">
			<Button type="button" variant="primary" onclick={onclose}>Close</Button>
		</div>
	{:else}
		<h2 id={titleId} class="detail-modal__title">No Activity Selected</h2>
		<p>Please close this modal and select an activity to view its details.</p>
		<div class="detail-modal__actions">
			<Button type="button" variant="primary" onclick={onclose}>Close</Button>
		</div>
	{/if}
</ModalDialog>

<style lang="scss">
	.detail-modal {
		&__title {
			font-size: 1.25rem;
			font-weight: 600;
			margin-bottom: 0.25rem;
		}

		&__content {
			display: flex;
			flex-direction: column;
			gap: 1rem;
			margin-top: 1.5rem;
			font-size: 0.875rem;
		}

		&__item {
			display: flex;
			flex-direction: column;
		}

		&__label {
			font-weight: 500;
			color: #6b7280;
			margin-bottom: 0.25rem;
		}

		&__value {
			color: #111827;
		}

		&__tags {
			display: flex;
			flex-wrap: wrap;
			gap: 0.5rem;
		}

		&__tag {
			background-color: #e5e7eb;
			color: #374151;
			padding: 0.25rem 0.75rem;
			border-radius: 9999px;
			font-size: 0.75rem;
		}

		&__definition {
			background-color: #f3f4f6;
			border: 1px solid #e5e7eb;
			border-radius: 0.375rem;
			padding: 0.75rem;
			white-space: pre-wrap;
			word-break: break-all;
			max-height: 200px;
			overflow-y: auto;
		}

		&__actions {
			display: flex;
			justify-content: flex-end;
			margin-top: 1.5rem;
			border-top: 1px solid #e5e7eb;
			padding-top: 1rem;
		}
	}
</style>
