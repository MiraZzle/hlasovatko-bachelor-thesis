<script lang="ts">
	/**
	 * @file Modal dialog component for creating a new session.
	 */
	import ModalDialog from '$components/elements/modals/ModalDialog.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import Select from '$components/elements/typography/Select.svelte';
	import Input from '$components/elements/typography/Input.svelte';
	import ToggleSwitch from '../ToggleSwitch.svelte';
	import type { SessionMode } from '$lib/shared_types';
	import type { TemplateStub } from '$lib/templates/types';
	import DatePicker from '$components/elements/typography/DatePicker.svelte';
	import { toast } from '$lib/stores/toast_store';

	let {
		open = $bindable(false),
		templates = [],
		onclose = () => {
			open = false;
		},
		onCreate = async (
			templateId: string,
			title: string,
			activationDate?: string,
			setSessionMode?: SessionMode
		) => {
			console.warn('onCreate handler not provided');
		}
	}: {
		open?: boolean;
		templates?: TemplateStub[];
		onclose?: () => void;
		onCreate?: (
			templateId: string,
			title: string,
			activationDate?: string,
			setSessionMode?: SessionMode
		) => void | Promise<void>;
	} = $props();

	let selectedTemplateId = $state<string>('');
	let sessionTitle = $state<string>('');
	let isSubmitting = $state(false);
	let activationDate = $state<string>(''); // For YYYY-MM-DD format from <input type="date">
	let sessionMode = $state<SessionMode>('teacher-paced');
	let planSession = $state(false);

	function getOptions(): { value: string; label: string }[] {
		const options = [{ value: '', label: 'Select a template...' }];
		templates.forEach((t) =>
			options.push({ value: t.id, label: `${t.title} (#${t.id.substring(1)})` })
		);
		return options;
	}

	const templateOptions = $derived(getOptions());

	const modeOptions = [
		{ value: 'teacher-paced', label: 'Teacher Paced' },
		{ value: 'student-paced', label: 'Student Paced' }
	];

	const todayString = new Date().toISOString().split('T')[0];

	// Auto-fill session title based on selected template
	$effect(() => {
		if (selectedTemplateId) {
			const selectedTemplate = templates.find((t) => t.id === selectedTemplateId);
			if (selectedTemplate) {
				sessionTitle = selectedTemplate.title;
			}
		} else {
			sessionTitle = '';
		}
	});
	// Reset form fields when modal opens
	$effect(() => {
		if (open) {
			selectedTemplateId = '';
			sessionTitle = '';
			isSubmitting = false;
			activationDate = todayString;
			sessionMode = 'teacher-paced';
			planSession = false;
		}
	});

	// When a date is picked, default mode to student-paced
	$effect(() => {
		if (activationDate) {
			sessionMode = 'student-paced';
		} else {
			sessionMode = 'teacher-paced';
		}
	});

	/**
	 * Handles the form submission to create a new session.
	 * Validates input and calls the onCreate callback with the session data.
	 */
	async function handleSubmit(): Promise<void> {
		if (!selectedTemplateId) {
			toast.show('Please select a template.', 'error');
			return;
		}
		if (isSubmitting) return;

		isSubmitting = true;
		try {
			const finalTitle =
				sessionTitle.trim() ||
				templates.find((t) => t.id === selectedTemplateId)?.title ||
				'Session';
			await onCreate(
				selectedTemplateId,
				finalTitle,
				planSession ? activationDate : undefined,
				sessionMode
			);
			requestClose();
		} catch (err) {
			toast.show(
				`Failed to create session: ${err instanceof Error ? err.message : 'Unknown error'}`,
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

	const titleId = 'create-session-title';
</script>

<ModalDialog bind:open {onclose} width="sm" {titleId}>
	<h2 id={titleId} class="create-session-modal__title">Add new Session</h2>

	<form onsubmit={handleSubmit} class="create-session-modal__form">
		<Select
			label="Select Template *"
			id="template-select-modal"
			options={templateOptions}
			bind:value={selectedTemplateId}
			required
			disabled={isSubmitting}
			width="full"
		/>

		<Input
			label="Session Title (Optional)"
			id="session-title-modal"
			bind:value={sessionTitle}
			placeholder="Defaults to template name"
			disabled={isSubmitting}
		/>

		<Select
			label="Session Mode"
			id="session-mode-select-modal"
			options={modeOptions}
			bind:value={sessionMode}
			disabled={isSubmitting}
			width="full"
		/>

		<div class="create-session-modal__activation-date">
			<label for="plan-toggle">Plan Session</label>
			<ToggleSwitch label="Plan Session" bind:checked={planSession} disabled={isSubmitting} />
		</div>

		{#if planSession}
			<DatePicker
				id="activation-date-picker-modal"
				bind:value={activationDate}
				disabled={isSubmitting}
				required
				label="Activation Date *"
			/>
		{/if}

		<div class="create-session-modal__actions">
			<Button type="button" variant="outline" onclick={requestClose} disabled={isSubmitting}
				>Cancel</Button
			>
			<Button type="submit" variant="primary" disabled={isSubmitting || !selectedTemplateId}>
				{#if isSubmitting}Creating...{:else}Create{/if}
			</Button>
		</div>
	</form>
</ModalDialog>

<style lang="scss">
	.create-session-modal {
		&__title {
			font-size: $font-size-xl;
			font-weight: $font-weight-semibold;
			text-align: center;
			margin-bottom: $spacing-lg;
			padding-top: $spacing-sm;
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
			margin-top: $spacing-xl;
			border-top: 1px solid $color-border-light;
			padding-top: $spacing-lg;
		}

		&__activation-date {
			display: flex;
			align-items: center;
			gap: $spacing-sm;
			font-size: $font-size-sm;
			color: $color-text-secondary;
			justify-content: space-between;
			label {
				cursor: pointer;
			}
		}
	}
</style>
