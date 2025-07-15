<script lang="ts">
	/**
	 * @file Modal dialog component for adding a new activity to the bank.
	 */
	import ModalDialog from '$components/elements/modals/ModalDialog.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import Select from '$components/elements/typography/Select.svelte';
	import Input from '$components/elements/typography/Input.svelte';
	import TextArea from '$components/elements/typography/utils/TextArea.svelte';
	import type { SelectOption } from '$lib/shared_types';
	import type { NewActivityData, StaticActivityType } from '$lib/activities/types';
	import { toast } from '$lib/stores/toast_store';
	import { writable } from 'svelte/store';

	let {
		open = $bindable(false),
		activityTypes = [],
		onclose = () => {
			open = false;
		},
		onAdd = async (data: NewActivityData) => {
			console.warn('onAdd handler not provided', data);
		}
	}: {
		open?: boolean;
		activityTypes?: SelectOption[];
		onclose?: () => void;
		onAdd?: (data: NewActivityData) => void | Promise<void>;
	} = $props();

	// Store for boilerplate activity definitions
	const activityBoilerplates = writable<Record<StaticActivityType, object>>({
		multiple_choice: {
			options: [{ id: '1', text: 'Option 1' }],
			allowMultipleAnswers: false,
			correctOptionId: '1'
		},
		poll: { options: [{ id: '1', text: 'Option A' }] },
		scale_rating: { min: 1, max: 5, minLabel: 'Min', maxLabel: 'Max' },
		open_ended: { placeholder: 'Enter your answer...' },
		custom_activity: {}
	});

	let title = $state('');
	let selectedActivityType = $state<StaticActivityType>('multiple_choice');
	let activityDefinition = $state('');
	let categoriesString = $state('');
	let isSubmitting = $state(false);

	function getActivityTypeOptions(): SelectOption[] {
		return [{ value: '', label: 'Select activity type...' }, ...activityTypes];
	}

	const activityTypeOptions = $derived(getActivityTypeOptions());

	// Reset modal state when opened
	$effect(() => {
		if (open) {
			title = '';
			selectedActivityType = 'multiple_choice';
			categoriesString = '';
			isSubmitting = false;
		}
	});

	// Update the definition when activity type changes
	$effect(() => {
		if (selectedActivityType && open) {
			const boilerplate = $activityBoilerplates[selectedActivityType];
			activityDefinition = JSON.stringify(boilerplate, null, 2);
		}
	});

	async function handleSubmit(): Promise<void> {
		if (!title.trim() || !selectedActivityType) {
			toast.show('Please provide a title and select an activity type.', 'error');
			return;
		}
		try {
			JSON.parse(activityDefinition);
		} catch (e) {
			toast.show('Activity definition does not seem to be valid JSON.', 'error');
			return;
		}

		if (isSubmitting) return;
		isSubmitting = true;
		try {
			const categories = categoriesString
				.split(',')
				.map((cat) => cat.trim())
				.filter((cat) => cat.length > 0);

			await onAdd({
				title: title.trim(),
				type: selectedActivityType,
				definition: JSON.parse(activityDefinition),
				tags: categories
			});
			toast.show(`Activity ${title} added successfully!`, 'success');
			requestClose();
		} catch (err) {
			console.error('Error adding activity:', err);
			toast.show(
				`Failed to add activity: ${err instanceof Error ? err.message : 'Unknown error'}`,
				'error'
			);
		} finally {
			isSubmitting = false;
		}
	}

	function requestClose(): void {
		if (onclose) {
			onclose();
		}
	}

	const titleId = 'add-activity-title';
	const descriptionId = 'add-activity-desc';
</script>

<ModalDialog bind:open {onclose} width="md" {titleId} {descriptionId}>
	<h2 id={titleId} class="add-activity-modal__title">Add activity to bank</h2>
	<p id={descriptionId} class="add-activity-modal__description">
		Create a new activity to reuse later.
	</p>

	<form onsubmit={handleSubmit} class="add-activity-modal__form">
		<Input
			label="Title (Question)"
			id="activity-title-modal"
			bind:value={title}
			required
			placeholder="e.g., Lecture 5 Quick Check"
			disabled={isSubmitting}
		/>

		<Select
			label="Activity type"
			id="activity-type-modal"
			options={activityTypeOptions}
			bind:value={selectedActivityType}
			required
			disabled={isSubmitting}
			width="full"
		/>

		<div>
			<TextArea
				label="Activity Definition (JSON)"
				id="activity-definition-modal"
				bind:value={activityDefinition}
				placeholder="Predefined activity JSON..."
				rows={8}
				disabled={isSubmitting}
			/>
			<p class="add-activity-modal__definition-guide">
				For more information on the expected JSON structure, please see the <a
					href="/guide"
					target="_blank"
					rel="noopener noreferrer">Guide</a
				>.
			</p>
		</div>

		<Input
			label="Categories"
			id="activity-categories-modal"
			bind:value={categoriesString}
			placeholder="Tags separated by commas"
			disabled={isSubmitting}
		/>

		<div class="add-activity-modal__actions">
			<Button type="button" variant="outline" onclick={requestClose} disabled={isSubmitting}
				>Cancel</Button
			>
			<Button
				type="submit"
				variant="primary"
				disabled={isSubmitting || !title || !selectedActivityType}
			>
				{#if isSubmitting}Adding...{:else}Add activity{/if}
			</Button>
		</div>
	</form>
</ModalDialog>

<style lang="scss">
	.add-activity-modal {
		&__title {
			font-size: $font-size-xl;
			font-weight: $font-weight-semibold;
			margin-bottom: $spacing-xs;
			padding-top: $spacing-sm;
		}

		&__description {
			font-size: $font-size-md;
			color: $color-text-secondary;
			margin-bottom: $spacing-lg;
		}

		&__form {
			display: flex;
			flex-direction: column;
			gap: $spacing-lg;
		}

		&__definition-guide {
			font-size: $font-size-sm;
			color: $color-text-secondary;
			margin-top: $spacing-sm;
			text-align: left;

			a {
				color: $color-primary;
				text-decoration: none;
				&:hover {
					text-decoration: underline;
				}
			}
		}

		&__actions {
			display: flex;
			justify-content: flex-end;
			gap: $spacing-sm;
			margin-top: $spacing-md;
			border-top: 1px solid $color-border-light;
			padding-top: $spacing-lg;
		}
	}
</style>
