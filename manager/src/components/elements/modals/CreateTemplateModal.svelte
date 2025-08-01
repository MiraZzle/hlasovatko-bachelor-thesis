<script lang="ts">
	/**
	 * @file Modal dialog component for creating a new template.
	 */
	import ModalDialog from '$components/elements/modals/ModalDialog.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import Input from '$components/elements/typography/Input.svelte';
	import Select from '$components/elements/typography/Select.svelte';
	import type { Template } from '$lib/templates/types';
	import { toast } from '$lib/stores/toast_store';

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
		templates?: Template[];
		onclose?: () => void;
		onCreate?: (data: { name: string }) => void | Promise<void>;
	} = $props();

	let name = $state('');
	let isSubmitting = $state(false);

	function getOptions(): { value: string; label: string }[] {
		const options = [{ value: 'none', label: 'None' }];
		templates.forEach((t) => options.push({ value: t.id, label: t.settings!.title }));
		return options;
	}

	const deriveOptions = $derived(getOptions());

	// Reset form fields when the modal opens
	$effect(() => {
		if (open) {
			name = '';
			isSubmitting = false;
		}
	});

	/**
	 * Handles the form submission to create a new template.
	 * Validates input and calls the onCreate callback with the new template data.
	 */
	async function handleSubmit(): Promise<void> {
		if (!name.trim()) {
			toast.show('Please enter a template name.', 'error');
			return;
		}
		if (isSubmitting) return;

		isSubmitting = true;
		try {
			await onCreate({ name: name.trim() });
			requestClose();
		} catch (err) {
			console.error('Error during template creation:', err);
			toast.show(
				`Failed to create template: ${err instanceof Error ? err.message : 'Unknown error'}`,
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

	const titleId = 'create-template-title';
</script>

<ModalDialog bind:open {onclose} width="sm" {titleId}>
	<h2 id={titleId} class="create-template-modal__title">Add new Template</h2>

	<form onsubmit={handleSubmit} class="create-template-modal__form">
		<Input
			label="Template name"
			id="template-name-modal"
			bind:value={name}
			required
			placeholder="e.g., Weekly Physics Quiz"
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
	.create-template-modal {
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
