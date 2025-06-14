<script lang="ts">
	import ModalDialog from '$components/elements/modals/ModalDialog.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import Select from '$components/elements/typography/Select.svelte';
	import Input from '$components/elements/typography/Input.svelte';
	import TextArea from '$components/elements/typography/utils/TextArea.svelte';
	import type { NewActivity, KnownActivityDefinition } from '$lib/activity_types';

	interface ActivityTypeOption {
		value: string;
		label: string;
	}

	interface NewActivityData {
		title: string;
		type: string;
		definition: string;
		categories: string[];
	}

	let {
		open = $bindable(false),
		activityTypes = [],
		onclose = () => {
			open = false;
		},
		onAdd = async (data) => {
			console.warn('onAdd handler not provided', data);
		}
	}: {
		open?: boolean;
		activityTypes?: ActivityTypeOption[];
		onclose?: () => void;
		onAdd?: (data: NewActivityData) => void | Promise<void>;
	} = $props();

	let title = $state('');
	let selectedActivityType = $state<string>('');
	let activityDefinition = $state('');
	let categoriesString = $state('');
	let isSubmitting = $state(false);

	function getActivityTypeOptions() {
		return [{ value: '', label: 'Select activity type...' }, ...activityTypes];
	}

	const activityTypeOptions = $derived(getActivityTypeOptions());

	$effect(() => {
		if (open) {
			title = '';
			selectedActivityType = '';
			activityDefinition = '';
			categoriesString = '';
			isSubmitting = false;
		}
	});

	async function handleSubmit(): Promise<void> {
		if (!title.trim() || !selectedActivityType) {
			alert('Please provide a title and select an activity type.');
			return;
		}
		if (activityDefinition.trim()) {
			try {
				JSON.parse(activityDefinition);
			} catch (e) {
				alert('Activity definition does not seem to be valid JSON.');
				return;
			}
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
				definition: activityDefinition.trim(),
				categories: categories
			});
			requestClose();
		} catch (err) {
			console.error('Error adding activity:', err);
			alert(`Failed to add activity: ${err instanceof Error ? err.message : 'Unknown error'}`);
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
		Create new activity to reuse later.
	</p>

	<form onsubmit={handleSubmit} class="add-activity-modal__form">
		<Input
			label="Title"
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

		<TextArea
			label="Activity Definition (JSON)"
			id="activity-definition-modal"
			bind:value={activityDefinition}
			placeholder="Predefined activity JSON..."
			rows={6}
			disabled={isSubmitting}
		/>

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
