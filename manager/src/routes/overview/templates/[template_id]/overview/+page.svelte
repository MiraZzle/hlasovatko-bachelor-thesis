<script lang="ts">
	import { page } from '$app/stores';
	import Button from '$components/elements/typography/Button.svelte';
	import ToggleSwitch from '$components/elements/ToggleSwitch.svelte';
	import SessionActivityItem from '$components/activities/SessionActivityItem.svelte';
	import RawJsonDisplay from '$components/activities/RawJson.svelte';
	import { onMount } from 'svelte';
	import TextArea from '$components/elements/typography/utils/TextArea.svelte';
	import { tick } from 'svelte';
	import { getTemplateById } from '$lib/templates/template_utils';
	import type { Template } from '$lib/templates/types';
	import { updateTemplate } from '$lib/templates/template_utils';
	import type { Activity } from '$lib/activities/types';

	let template_id = $page.params.template_id;

	// State management
	let viewMode = $state<'json' | 'visual'>('visual'); // default to visual mode
	let templateDefinition = $state<Template | null>(null);
	let isLoading = $state(true);
	let error = $state<string | null>(null);

	// JSON editor state
	let templateJsonString = $state('');
	let jsonParseError = $state<string | null>(null);
	let isSaving = $state(false);
	let saveSuccessMessage = $state<string | null>(null);

	// File input state
	let fileInputRef: HTMLInputElement | null = null;

	/*
	 * Lifecycle hooks
	 * Load the template definition when the component mounts
	 */
	onMount(async () => {
		isLoading = true;
		error = null;
		try {
			const data = await getTemplateById(template_id);
			if (!data) {
				throw new Error('Template not found.');
			}
			templateDefinition = data;
		} catch (e) {
			console.error('Failed to load template:', e);
			error = e instanceof Error ? e.message : 'An unknown error occurred.';
		} finally {
			isLoading = false;
		}
	});

	/*
	 * Effect to update the JSON string whenever the template definition changes
	 * This allows the JSON editor to always reflect the current template state
	 */
	$effect(() => {
		if (templateDefinition) {
			try {
				templateJsonString = JSON.stringify(templateDefinition, null, 2);
				jsonParseError = null;
			} catch (e) {
				jsonParseError = 'Error: Could not stringify template data.';
			}
		}
	});

	/*
	 * Function to trigger the file input click event
	 */
	function triggerFileInput(): void {
		fileInputRef?.click();
	}

	/*
	 * Reads the selected JSON file and updates the template definition
	 */
	function handleFileSelect(event: Event): void {
		// Prevent default behavior
		const target = event.target as HTMLInputElement;
		const file = target.files?.[0];
		if (!file) return;

		error = null;
		isLoading = true;

		// Read the file as text
		const reader = new FileReader();
		reader.onload = (e) => {
			try {
				const content = e.target?.result as string;
				templateDefinition = JSON.parse(content);
			} catch (err) {
				error = `Error reading file: ${err instanceof Error ? err.message : 'Invalid JSON'}`;
			} finally {
				isLoading = false;
			}
		};
		reader.readAsText(file);
		if (target) target.value = '';
	}

	/*
	 * Toggles between JSON and visual view modes
	 */
	function handleViewModeToggle(event: { checked: boolean }): void {
		viewMode = event.checked ? 'visual' : 'json';
	}

	/*
	 * Formats activity data for display in the visual mode
	 */
	function formatActivityForDisplay(activityData: any, index: number): Activity {
		return {
			id: activityData.id ?? `temp-act-${index}`,
			type: activityData.type ?? 'Unknown',
			title: activityData.title ?? activityData.question ?? 'Untitled Activity',
			definition: activityData.definition ?? activityData
		};
	}

	/*
	 * Handles input changes in the JSON editor
	 * Validates the JSON and updates the state accordingly
	 */
	function handleJsonInput(event: Event & { currentTarget: HTMLTextAreaElement }): void {
		const currentJsonString = event.currentTarget.value;
		templateJsonString = currentJsonString;
		saveSuccessMessage = null;

		try {
			JSON.parse(currentJsonString);
			jsonParseError = null;
		} catch (e) {
			jsonParseError = `Invalid JSON: ${e instanceof SyntaxError ? e.message : 'Syntax error'}`;
		}
	}

	/*
	 * Saves the template changes to the server
	 */
	async function saveTemplateChanges(): Promise<void> {
		if (jsonParseError || isSaving || !templateDefinition) {
			return;
		}

		isSaving = true;
		saveSuccessMessage = null;
		error = null;

		try {
			const updatedTemplateObject = JSON.parse(templateJsonString);
			const success = await updateTemplate(template_id, updatedTemplateObject);
			if (!success) {
				throw new Error('Server returned an error on save.');
			}

			templateDefinition = updatedTemplateObject;
			saveSuccessMessage = 'Template saved successfully!';
			setTimeout(() => (saveSuccessMessage = null), 3000);
		} catch (err) {
			console.error('Error saving template:', err);
			error = err instanceof Error ? err.message : 'An unknown error occurred during save.';
		} finally {
			isSaving = false;
		}
	}
</script>

<svelte:head>
	<title>EngaGenie | Template {template_id} - Overview</title>
</svelte:head>

<div class="template-overview-page">
	<header class="template-overview-page__header">
		<div class="template-overview-page__file-loader">
			<input
				type="file"
				accept=".json,application/json"
				bind:this={fileInputRef}
				onchange={handleFileSelect}
				hidden
				id="template-file-input"
			/>
			<Button variant="outline" onclick={triggerFileInput}>Import/Replace JSON</Button>
		</div>
		<div class="template-overview-page__view-toggle">
			<label for="view-mode-toggle">Visual mode</label>
			<ToggleSwitch
				checked={viewMode === 'visual'}
				onchange={handleViewModeToggle}
				disabled={!templateDefinition || isLoading}
				label="Switch between JSON and Visual mode"
			/>
		</div>
	</header>

	<div class="template-overview-page__content">
		{#if isLoading}
			<p>Loading template data...</p>
		{:else if error}
			<p class="template-overview-page__message--error">Error: {error}</p>
		{:else if !templateDefinition}
			<p class="template-overview-page__message--info">
				Template could not be loaded. You can import a definition file to begin.
			</p>
		{:else if viewMode === 'json'}
			<div class="template-overview-page__json-editor">
				<TextArea bind:value={templateJsonString} oninput={handleJsonInput} rows={20} />
				{#if jsonParseError}
					<p class="template-overview-page__message template-overview-page__message--error">
						{jsonParseError}
					</p>
				{/if}
				{#if saveSuccessMessage}
					<p class="template-overview-page__message template-overview-page__message--success">
						{saveSuccessMessage}
					</p>
				{/if}
				<div class="template-overview-page__json-actions">
					<Button
						variant="primary"
						onclick={saveTemplateChanges}
						disabled={!!jsonParseError || isSaving}
					>
						{#if isSaving}Saving...{:else}Save Changes{/if}
					</Button>
				</div>
			</div>
		{:else if viewMode === 'visual' && Array.isArray(templateDefinition.definition)}
			<div class="template-overview-page__activity-list">
				{#each templateDefinition.definition as activityData, index (activityData.id ?? index)}
					{@const activity = formatActivityForDisplay(activityData, index)}
					<SessionActivityItem {activity} isOnlyView={true} />
				{:else}
					<p class="template-overview-page__message--info">Template has no activities defined.</p>
				{/each}
			</div>
		{:else}
			<p class="template-overview-page__message--error">
				Cannot display visual mode. Template 'definition' property might not be an array.
			</p>
			<RawJsonDisplay definition={templateDefinition} />
		{/if}
	</div>
</div>

<style lang="scss">
	.template-overview-page {
		display: flex;
		flex-direction: column;
		gap: $spacing-lg;

		&__header {
			display: flex;
			justify-content: space-between;
			align-items: center;
			flex-wrap: wrap;
			gap: $spacing-md;
		}

		&__file-loader {
			display: flex;
			align-items: center;
			gap: $spacing-sm;
		}

		&__view-toggle {
			display: flex;
			align-items: center;
			gap: $spacing-sm;
			font-size: $font-size-sm;
			color: $color-text-secondary;
			label {
				cursor: pointer;
			}
		}

		&__content {
			background-color: $color-surface;
			border-radius: $border-radius-md;
			padding: $spacing-lg;
			border: 1px solid $color-border-light;
			min-height: 300px;
		}

		&__activity-list {
			display: flex;
			flex-direction: column;
			gap: $spacing-lg;
		}

		&__message {
			&--error {
				color: $color-error;
				font-style: italic;
				text-align: center;
				padding: $spacing-lg;
			}
			&--info {
				color: $color-text-secondary;
				font-style: italic;
				text-align: center;
				padding: $spacing-lg;
			}
		}
	}
</style>
