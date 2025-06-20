<script lang="ts">
	/**
	 * @file Modal dialog component for exporting global analytics data.
	 * Allows users to select a format and export their session statistics.
	 */
	import ModalDialog from '$components/elements/modals/ModalDialog.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import Select from '$components/elements/typography/Select.svelte';

	type ExportFormat = 'csv' | 'xlsx' | 'json';

	type Props = {
		open?: boolean;
		timeFrameLabel?: string;
		onclose?: () => void;
		onExport?: (format: ExportFormat) => void | Promise<void>;
	};

	let {
		open = $bindable(false),
		timeFrameLabel = 'the selected period',
		onclose = () => {
			open = false;
		},
		onExport = async (format) => {
			console.warn('onExport handler not provided', format);
		}
	}: Props = $props();

	let selectedFormat = $state<ExportFormat>('csv');
	let isSubmitting = $state(false);

	const formatOptions: { value: ExportFormat; label: string }[] = [
		{ value: 'csv', label: 'CSV' },
		{ value: 'xlsx', label: 'Excel (XLSX)' },
		{ value: 'json', label: 'JSON' }
	];

	$effect(() => {
		if (open) {
			selectedFormat = 'csv';
			isSubmitting = false;
		}
	});

	/**
	 * Handles the form submission to export statistics.
	 * Validates input and calls the onExport callback with the selected format.
	 */
	async function handleSubmit(): Promise<void> {
		if (isSubmitting) return;
		isSubmitting = true;
		try {
			await onExport(selectedFormat);
			requestClose();
		} catch (err) {
			console.error('Error during export:', err);
			alert(`Failed to export statistics: ${err instanceof Error ? err.message : 'Unknown error'}`);
		} finally {
			isSubmitting = false;
		}
	}

	function requestClose(): void {
		if (onclose) {
			onclose();
		}
	}

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
	.export-analytics-modal {
		&__title {
			font-size: $font-size-xl;
			font-weight: $font-weight-semibold;
			text-align: center;
			margin-bottom: $spacing-xs;
			padding-top: $spacing-sm;
		}

		&__description {
			font-size: $font-size-md;
			color: $color-text-secondary;
			text-align: center;
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
		}
	}
</style>
