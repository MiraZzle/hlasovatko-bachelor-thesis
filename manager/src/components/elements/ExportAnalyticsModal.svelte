<script lang="ts">
	import ModalDialog from '$components/elements/ModalDialog.svelte';
	import Button from '$components/elements/typography/Button.svelte'; // Verify path
	import Select from '$components/elements/typography/Select.svelte'; // Verify path

	type ExportFormat = 'csv' | 'xlsx' | 'json'; // Example formats

	// --- Component Props ---
	type Props = {
		open?: boolean;
		timeFrameLabel?: string; // e.g., "the last week", "the last 30 days"
		onclose?: () => void;
		onExport?: (format: ExportFormat) => void | Promise<void>;
	};

	let {
		open = $bindable(false),
		timeFrameLabel = 'the selected period', // Default description
		onclose = () => {
			open = false;
		},
		onExport = async (format) => {
			console.warn('onExport handler not provided', format);
		}
	}: Props = $props();

	// --- Internal State ---
	let selectedFormat = $state<ExportFormat>('csv'); // Default format
	let isSubmitting = $state(false);

	// --- Format Options ---
	const formatOptions: { value: ExportFormat; label: string }[] = [
		{ value: 'csv', label: 'CSV' },
		{ value: 'xlsx', label: 'Excel (XLSX)' },
		{ value: 'json', label: 'JSON' }
	];

	// --- Reset form when modal opens ---
	$effect(() => {
		if (open) {
			selectedFormat = 'csv'; // Reset to default
			isSubmitting = false;
		}
	});

	// --- Form Submit Handler ---
	async function handleSubmit() {
		if (isSubmitting) return;
		isSubmitting = true;
		try {
			await onExport(selectedFormat);
			requestClose(); // Close on success
		} catch (err) {
			console.error('Error during export:', err);
			alert(`Failed to export statistics: ${err instanceof Error ? err.message : 'Unknown error'}`);
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
	const titleId = 'export-analytics-title';
	const descriptionId = 'export-analytics-desc';
</script>

<ModalDialog bind:open {onclose} width="sm" {titleId} {descriptionId}>
	<h2 id={titleId} class="export-analytics-modal__title">Global Analytics Overview</h2>
	<p id={descriptionId} class="export-analytics-modal__description">
		Export your sessions statistics from {timeFrameLabel}.
	</p>

	<form onsubmit={handleSubmit} class="export-analytics-modal__form">
		<Select
			label="Format"
			id="export-format-modal"
			options={formatOptions}
			bind:value={selectedFormat}
			required
			disabled={isSubmitting}
			width="full"
		/>

		<div class="export-analytics-modal__actions">
			<Button type="button" variant="outline" onclick={requestClose} disabled={isSubmitting}
				>Cancel</Button
			>
			<Button type="submit" variant="primary" disabled={isSubmitting}>
				{#if isSubmitting}Exporting...{:else}Export Statistics{/if}
			</Button>
		</div>
	</form>
</ModalDialog>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path if needed

	// BEM block for this modal's content
	.export-analytics-modal {
		&__title {
			font-size: $font-size-xl;
			font-weight: $font-weight-semibold;
			text-align: center;
			margin-bottom: $spacing-xs; // Less space below title
			padding-top: $spacing-sm;
		}

		&__description {
			font-size: $font-size-md;
			color: $color-text-secondary;
			text-align: center;
			margin-bottom: $spacing-lg; // Space below description
		}

		&__form {
			display: flex;
			flex-direction: column;
			gap: $spacing-lg; // Space between select and actions
		}

		&__actions {
			display: flex;
			justify-content: flex-end;
			gap: $spacing-sm;
			margin-top: $spacing-md; // Reduced space above buttons
			// Optional border if needed:
			// border-top: 1px solid $color-border-light;
			// padding-top: $spacing-lg;
		}
	}
</style>
