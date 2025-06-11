<script lang="ts">
	import ModalDialog from '$components/elements/modals/ModalDialog.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import Select from '$components/elements/typography/Select.svelte';
	import Input from '$components/elements/typography/Input.svelte';
	import { tick } from 'svelte';

	interface TemplateStub {
		id: string;
		title: string;
	}

	let {
		open = $bindable(false),
		templates = [],
		onclose = () => {
			open = false;
		},
		onCreate = async (data) => {
			console.warn('onCreate handler not provided', data);
		}
	}: {
		open?: boolean;
		templates?: TemplateStub[];
		onclose?: () => void;
		onCreate?: (data: { templateId: string; title: string }) => void | Promise<void>;
	} = $props();

	let selectedTemplateId = $state<string>('');
	let sessionTitle = $state('');
	let isSubmitting = $state(false);

	function getOptions() {
		const options = [{ value: '', label: 'Select a template...' }];
		templates.forEach((t) =>
			options.push({ value: t.id, label: `${t.title} (#${t.id.substring(1)})` })
		);
		return options;
	}

	const templateOptions = $derived(getOptions());

	$effect(() => {
		if (selectedTemplateId && selectedTemplateId !== '') {
			const selectedTemplate = templates.find((t) => t.id === selectedTemplateId);
			if (selectedTemplate && sessionTitle === '') {
				sessionTitle = selectedTemplate.title;
			}
		} else {
			// If 'Select a template...' is chosen, clear the title maybe? Or keep previous?
		}
	});

	$effect(() => {
		if (open) {
			selectedTemplateId = '';
			sessionTitle = '';
			isSubmitting = false;
		}
	});

	async function handleSubmit() {
		if (!selectedTemplateId) {
			alert('Please select a template.');
			return;
		}
		if (isSubmitting) return;

		isSubmitting = true;
		try {
			const finalTitle =
				sessionTitle.trim() ||
				templates.find((t) => t.id === selectedTemplateId)?.title ||
				'Session';
			await onCreate({ templateId: selectedTemplateId, title: finalTitle });
			requestClose();
		} catch (err) {
			console.error('Error during session creation:', err);
			alert(`Failed to start session: ${err instanceof Error ? err.message : 'Unknown error'}`);
		} finally {
			isSubmitting = false;
		}
	}

	function requestClose() {
		if (onclose) {
			onclose();
		}
	}

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
			width="full"
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
	@import '../../../styles/variables.scss';

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
