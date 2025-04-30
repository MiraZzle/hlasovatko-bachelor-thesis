<script lang="ts">
	import ModalDialog from '$components/elements/ModalDialog.svelte';
	import Button from '$components/elements/typography/Button.svelte'; // Verify path
	import Input from '$components/elements/typography/Input.svelte'; // Verify path
	import Select from '$components/elements/typography/Select.svelte'; // Verify path

	// Define expected type for existing templates (for derive options)
	interface TemplateStub {
		id: string;
		title: string;
	}

	// --- Component Props ---
	type Props = {
		open?: boolean; // Bindable visibility state
		templates?: TemplateStub[]; // List of existing templates for "Derive from"
		onclose?: () => void; // Callback when modal requests close (Cancel, X, Esc, Overlay)
		// Callback when primary action (Create) is confirmed
		onCreate?: (data: { name: string; deriveFromId: string }) => void | Promise<void>;
	};

	let {
		open = $bindable(false),
		templates = [],
		onclose = () => {
			open = false;
		}, // Default close action if only bind:open is used
		onCreate = async (data) => {
			console.warn('onCreate handler not provided', data);
		}
	}: Props = $props();

	// --- Internal State for the Form ---
	let name = $state('');
	let deriveFromId = $state('none'); // Default to 'none'
	let isSubmitting = $state(false); // Prevent double-clicks

	function getOptions() {
		const options = [{ value: 'none', label: 'None' }];
		templates.forEach((t) => options.push({ value: t.id, label: t.title }));
		return options;
	}

	// --- Generate Select Options ---
	const deriveOptions = $derived(getOptions());

	// --- Reset form when modal opens ---
	$effect(() => {
		if (open) {
			name = '';
			deriveFromId = 'none';
			isSubmitting = false;
		}
	});

	// --- Form Submit Handler ---
	async function handleSubmit() {
		if (!name.trim()) {
			alert('Please enter a template name.'); // Basic validation
			return;
		}
		if (isSubmitting) return; // Prevent double submission

		isSubmitting = true;
		try {
			// Call the parent's onCreate function
			await onCreate({ name: name.trim(), deriveFromId: deriveFromId });
			// If onCreate succeeds without error, request close
			requestClose();
		} catch (err) {
			console.error('Error during template creation:', err);
			alert(`Failed to create template: ${err instanceof Error ? err.message : 'Unknown error'}`);
			// Optionally keep modal open on error? Or handle specific errors?
		} finally {
			isSubmitting = false;
		}
	}

	// --- Request Close Handler (used by Cancel button) ---
	function requestClose() {
		if (onclose) {
			onclose();
		}
	}

	// Define unique IDs for accessibility
	const titleId = 'create-template-title';
</script>

<ModalDialog bind:open {onclose} width="sm" {titleId}>
	<h2 id={titleId} class="create-template-modal__title">Create template</h2>

	<form onsubmit={handleSubmit} class="create-template-modal__form">
		<Input
			label="Template name"
			id="template-name-modal"
			bind:value={name}
			required
			placeholder="e.g., Weekly Physics Quiz"
			disabled={isSubmitting}
		/>

		<Select
			label="Derive from"
			id="derive-from-modal"
			options={deriveOptions}
			bind:value={deriveFromId}
			disabled={isSubmitting}
		/>

		<div class="create-template-modal__actions">
			<Button type="button" variant="outline" onclick={requestClose} disabled={isSubmitting}
				>Cancel</Button
			>
			<Button type="submit" variant="primary" disabled={isSubmitting}>
				{#if isSubmitting}Creating...{:else}Create{/if}
			</Button>
		</div>
	</form>
</ModalDialog>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path if needed

	// BEM block for this specific modal's content
	.create-template-modal {
		&__title {
			font-size: $font-size-xl;
			font-weight: $font-weight-semibold;
			text-align: center;
			margin-bottom: $spacing-lg;
			// Adjust top padding if needed due to absolute close button
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
