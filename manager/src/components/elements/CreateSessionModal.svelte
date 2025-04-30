<script lang="ts">
	import ModalDialog from '$components/elements/ModalDialog.svelte';
	import Button from '$components/elements/typography/Button.svelte'; // Verify path
	import Select from '$components/elements/typography/Select.svelte'; // Verify path
	import Input from '$components/elements/typography/Input.svelte'; // Verify path
	import { tick } from 'svelte';

	// --- Component Props ---
	// Define expected type for available templates
	interface TemplateStub {
		id: string;
		title: string;
	}

	type Props = {
		open?: boolean;
		templates?: TemplateStub[]; // List of templates to choose from
		onclose?: () => void;
		// Callback with selected template and custom title
		onCreate?: (data: { templateId: string; title: string }) => void | Promise<void>;
	};

	let {
		open = $bindable(false),
		templates = [],
		onclose = () => {
			open = false;
		},
		onCreate = async (data) => {
			console.warn('onCreate handler not provided', data);
		}
	}: Props = $props();

	// --- Internal State ---
	let selectedTemplateId = $state<string>(''); // Store the ID of the selected template
	let sessionTitle = $state(''); // Optional custom title for the session
	let isSubmitting = $state(false);

	function getOptions() {
		const options = [{ value: '', label: 'Select a template...' }];
		templates.forEach((t) =>
			options.push({ value: t.id, label: `${t.title} (#${t.id.substring(1)})` })
		); // Show title and code
		return options;
	}

	// --- Generate Select Options ---
	// Add a placeholder/disabled option at the start
	const templateOptions = $derived(getOptions());

	// --- Default Session Title ---
	// When a template is selected, default the session title input to the template title
	$effect(() => {
		if (selectedTemplateId && selectedTemplateId !== '') {
			const selectedTemplate = templates.find((t) => t.id === selectedTemplateId);
			if (selectedTemplate && sessionTitle === '') {
				// Only default if title is empty
				sessionTitle = selectedTemplate.title;
			}
		} else {
			// If 'Select a template...' is chosen, clear the title maybe? Or keep previous?
			// sessionTitle = ''; // Optional: clear title if no template selected
		}
	});

	// --- Reset form when modal opens ---
	$effect(() => {
		if (open) {
			selectedTemplateId = ''; // Reset selection
			sessionTitle = ''; // Reset title
			isSubmitting = false;
			// Focus first element? (optional)
			// tick().then(() => { /* focus select or input */ });
		}
	});

	// --- Form Submit Handler ---
	async function handleSubmit() {
		if (!selectedTemplateId) {
			alert('Please select a template.'); // Validation
			return;
		}
		if (isSubmitting) return;

		isSubmitting = true;
		try {
			// Use provided sessionTitle, or default to selected template title if empty
			const finalTitle =
				sessionTitle.trim() ||
				templates.find((t) => t.id === selectedTemplateId)?.title ||
				'Session';
			await onCreate({ templateId: selectedTemplateId, title: finalTitle });
			requestClose(); // Close on success
		} catch (err) {
			console.error('Error during session creation:', err);
			alert(`Failed to start session: ${err instanceof Error ? err.message : 'Unknown error'}`);
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
	const titleId = 'create-session-title';
</script>

<ModalDialog bind:open {onclose} width="sm" {titleId}>
	<h2 id={titleId} class="create-session-modal__title">Start New Session</h2>

	<form onsubmit={handleSubmit} class="create-session-modal__form">
		<Select
			label="Select Template *"
			id="template-select-modal"
			options={templateOptions}
			bind:value={selectedTemplateId}
			required
			disabled={isSubmitting}
		/>

		<Input
			label="Session Title (Optional)"
			id="session-title-modal"
			bind:value={sessionTitle}
			placeholder="Defaults to template name"
			disabled={isSubmitting}
		/>

		<div class="create-session-modal__actions">
			<Button type="button" variant="outline" onclick={requestClose} disabled={isSubmitting}
				>Cancel</Button
			>
			<Button type="submit" variant="primary" disabled={isSubmitting || !selectedTemplateId}>
				{#if isSubmitting}Starting...{:else}Start Session{/if}
			</Button>
		</div>
	</form>
</ModalDialog>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path if needed

	// BEM block for this modal's content
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
	}
</style>
