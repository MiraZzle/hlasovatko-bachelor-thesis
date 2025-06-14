<script lang="ts">
	import ModalDialog from '$components/elements/modals/ModalDialog.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import Input from '$components/elements/typography/Input.svelte';
	import Select from '$components/elements/typography/Select.svelte';

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
		onCreate?: (data: { name: string; deriveFromId: string }) => void | Promise<void>;
	} = $props();

	let name = $state('');
	let deriveFromId = $state('none');
	let isSubmitting = $state(false);

	function getOptions(): { value: string; label: string }[] {
		const options = [{ value: 'none', label: 'None' }];
		templates.forEach((t) => options.push({ value: t.id, label: t.title }));
		return options;
	}

	const deriveOptions = $derived(getOptions());

	$effect(() => {
		if (open) {
			name = '';
			deriveFromId = 'none';
			isSubmitting = false;
		}
	});

	async function handleSubmit(): Promise<void> {
		if (!name.trim()) {
			alert('Please enter a template name.');
			return;
		}
		if (isSubmitting) return;

		isSubmitting = true;
		try {
			await onCreate({ name: name.trim(), deriveFromId: deriveFromId });
			requestClose();
		} catch (err) {
			console.error('Error during template creation:', err);
			alert(`Failed to create template: ${err instanceof Error ? err.message : 'Unknown error'}`);
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
			width="full"
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
