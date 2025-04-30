<script lang="ts">
	import ModalDialog from '$components/elements/ModalDialog.svelte';
	import Button from '$components/elements/typography/Button.svelte'; // Verify path
	import Select from '$components/elements/typography/Select.svelte'; // Verify path
	import Input from '$components/elements/typography/Input.svelte'; // Verify path
	import TextArea from '$components/elements/typography/utils/TextArea.svelte'; // Verify path

	// --- Component Props ---
	// Type for activity type options
	interface ActivityTypeOption {
		value: string; // e.g., 'quiz', 'poll'
		label: string; // e.g., 'Quiz', 'Poll'
	}
	// Type for data emitted on creation
	interface NewActivityData {
		title: string;
		type: string;
		definition: string; // Assuming JSON string for now
		categories: string[]; // Parsed array
	}

	type Props = {
		open?: boolean;
		activityTypes?: ActivityTypeOption[]; // Options for the dropdown
		onclose?: () => void;
		onAdd?: (data: NewActivityData) => void | Promise<void>;
	};

	let {
		open = $bindable(false),
		activityTypes = [],
		onclose = () => {
			open = false;
		},
		onAdd = async (data) => {
			console.warn('onAdd handler not provided', data);
		}
	}: Props = $props();

	// --- Internal State ---
	let title = $state('');
	let selectedActivityType = $state<string>(''); // Store the value (e.g., 'quiz')
	let activityDefinition = $state(''); // JSON content as string
	let categoriesString = $state(''); // Comma-separated string
	let isSubmitting = $state(false);

	function getActivityTypeOptions() {
		return [{ value: '', label: 'Select activity type...' }, ...activityTypes];
	}

	// --- Select Options ---
	const activityTypeOptions = $derived(getActivityTypeOptions());

	// --- Reset form when modal opens ---
	$effect(() => {
		if (open) {
			title = '';
			selectedActivityType = '';
			activityDefinition = '';
			categoriesString = '';
			isSubmitting = false;
		}
	});

	// --- Form Submit Handler ---
	async function handleSubmit() {
		if (!title.trim() || !selectedActivityType) {
			alert('Please provide a title and select an activity type.');
			return;
		}
		// Basic JSON validation example (optional)
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
			// Parse categories string into array, trimming whitespace
			const categories = categoriesString
				.split(',')
				.map((cat) => cat.trim())
				.filter((cat) => cat.length > 0); // Remove empty entries

			await onAdd({
				title: title.trim(),
				type: selectedActivityType,
				definition: activityDefinition.trim(),
				categories: categories
			});
			requestClose(); // Close on success
		} catch (err) {
			console.error('Error adding activity:', err);
			alert(`Failed to add activity: ${err instanceof Error ? err.message : 'Unknown error'}`);
		} finally {
			isSubmitting = false;
		}
	}

	// --- Request Close Handler ---
	function requestClose() {
		if (onclose) {
			onclose();
		}
	}

	// Define unique IDs for accessibility
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
	@import '../../styles/variables.scss'; // Adjust path if needed

	// BEM block for this modal's content
	.add-activity-modal {
		&__title {
			font-size: $font-size-xl;
			font-weight: $font-weight-semibold;
			// text-align: center; // Align left looks better here
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
			gap: $spacing-lg; // Consistent gap
		}

		&__actions {
			display: flex;
			justify-content: flex-end;
			gap: $spacing-sm;
			margin-top: $spacing-md; // Slightly less space above actions
			border-top: 1px solid $color-border-light;
			padding-top: $spacing-lg;
		}
	}
</style>
