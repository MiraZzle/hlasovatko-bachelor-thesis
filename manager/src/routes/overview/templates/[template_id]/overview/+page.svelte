<script lang="ts">
	import { page } from '$app/stores';
	import Button from '$components/elements/typography/Button.svelte';
	import ToggleSwitch from '$components/elements/ToggleSwitch.svelte';
	import SessionActivityItem from '$components/activities/SessionActivityItem.svelte';
	import RawJsonDisplay from '$components/activities/RawJson.svelte';
	import type { SessionActivity } from '$lib/activity_types';
	import { onMount } from 'svelte';
	import TextArea from '$components/elements/typography/utils/TextArea.svelte';
	import { tick } from 'svelte';
	import { getTemplateById } from '$lib/templates/template_utils';
	import type { Template } from '$lib/templates/types';

	let { template_id } = $page.params;

	// state
	let viewMode = $state<'json' | 'visual'>('json');
	let templateDefinition = $state<Template>();
	let isLoading = $state(true);
	let error = $state<string | null>(null);
	let fileName = $state<string | null>(null);
	let fileInputRef: HTMLInputElement | null = null;

	// json state
	let templateJsonString = $state('');
	let jsonParseError = $state<string | null>(null);
	let isSavingJson = $state(false);
	let saveSuccessMessage = $state<string | null>(null);

	// --- Dummy Data / Fetching ---
	// In real app, fetch template JSON based on template_id in a load function or onMount
	let dummyJson = getTemplateById(template_id);

	onMount(() => {
		setTimeout(() => {
			templateDefinition = dummyJson;
			isLoading = false;
		}, 500);
	});

	$effect(() => {
		if (templateDefinition) {
			try {
				templateJsonString = JSON.stringify(templateDefinition, null, 2); // Pretty print
				jsonParseError = null; // Clear errors if object updates successfully
			} catch (e) {
				console.error('Error stringifying template definition:', e);
				templateJsonString = 'Error displaying JSON.';
				jsonParseError = 'Could not display template definition as JSON.';
			}
		} else {
			templateJsonString = ''; // Clear if no definition
		}
	});

	function triggerFileInput(): void {
		fileInputRef?.click();
	}

	function handleFileSelect(event: Event): void {
		const target = event.target as HTMLInputElement;
		const files = target.files;
		error = null;
		fileName = null;
		isLoading = true;

		if (files && files.length > 0) {
			const file = files[0];
			fileName = file.name;
			console.log('Selected file:', file.name);

			if (!file.name.toLowerCase().endsWith('.json') && file.type !== 'application/json') {
				error = 'Please select a valid JSON file (.json).';
				isLoading = false;
				return;
			}

			const reader = new FileReader();
			reader.onload = (e) => {
				try {
					const content = e.target?.result as string;
					const jsonData = JSON.parse(content);
					console.log('Imported JSON content:', jsonData);
					templateDefinition = jsonData;
				} catch (err) {
					console.error('Error reading/parsing file:', err);
					error = `Error reading file: ${err instanceof Error ? err.message : 'Invalid JSON format'}`;
				} finally {
					isLoading = false;
				}
			};
			reader.onerror = (e) => {
				console.error('FileReader error:', e);
				error = 'Error reading file.';
				isLoading = false;
			};
			reader.readAsText(file);
		} else {
			console.log('No file selected.');
			isLoading = false;
		}
		if (target) target.value = '';
	}

	function handleViewModeToggle(event: { checked: boolean }): void {
		viewMode = event.checked ? 'visual' : 'json';
	}

	function formatActivityForDisplay(activityData: any, index: number): SessionActivity {
		return {
			id: activityData.id ?? `temp-act-${index}`,
			type: activityData.type ?? 'Unknown',
			title: activityData.title ?? activityData.question ?? 'Untitled Activity',
			definition: activityData.definition ?? activityData, // Pass full data if no definition field
			status: 'Pending', // Example status for display
			order: index + 1
			// Add dummy counts if SessionActivityItem uses them
			// participantCount: 0,
			// responseCount: 0
		};
	}

	// --- NEW: Handle JSON Textarea Input ---
	function handleJsonInput(event: Event & { currentTarget: HTMLTextAreaElement }): void {
		const currentJsonString = event.currentTarget.value;
		templateJsonString = currentJsonString; // Update the state bound to textarea
		saveSuccessMessage = null; // Clear success message on edit

		// Validate JSON syntax
		try {
			JSON.parse(currentJsonString);
			jsonParseError = null; // Clear error if valid JSON
		} catch (e) {
			if (e instanceof SyntaxError) {
				jsonParseError = `Invalid JSON: ${e.message}`;
			} else {
				jsonParseError = 'Error parsing JSON.';
			}
		}
	}

	// --- NEW: Handle Saving Edited JSON ---
	async function saveJsonChanges(): Promise<void> {
		if (jsonParseError || isSavingJson) {
			alert('Cannot save, JSON is invalid.');
			return;
		}
		saveSuccessMessage = null;
		isSavingJson = true;
		console.log('Saving updated JSON definition...');

		try {
			// Update the main templateDefinition object with the valid parsed JSON
			templateDefinition = JSON.parse(templateJsonString);

			// --- TODO: API Call to save the updated templateDefinition object ---
			await new Promise((resolve) => setTimeout(resolve, 1000)); // Simulate network

			console.log('JSON Save successful (simulation)');
			saveSuccessMessage = 'Template saved successfully!';
			await tick();
			setTimeout(() => {
				saveSuccessMessage = null;
			}, 3000);
		} catch (err) {
			console.error('Error saving template:', err);
			// Display error to user (could use jsonParseError state or a dedicated saveError state)
			jsonParseError = `Failed to save: ${err instanceof Error ? err.message : 'Unknown error'}`;
		} finally {
			isSavingJson = false;
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
			<Button variant="outline" onclick={triggerFileInput}>
				{#if isLoading && !fileName}
					Loading...
				{:else}
					Choose session definition
				{/if}
			</Button>
			{#if fileName}
				<span class="template-overview-page__file-name">{fileName}</span>
			{:else if !isLoading}
				<span
					class="template-overview-page__file-name template-overview-page__file-name--placeholder"
					>No file chosen</span
				>
			{/if}
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
				Please choose a template definition file to view.
			</p>
		{:else if viewMode === 'json'}
			<div class="template-overview-page__json-editor">
				<TextArea bind:value={templateJsonString} oninput={handleJsonInput} rows={20} />
				{#if jsonParseError}
					<p
						id="json-error-msg"
						class="template-overview-page__message template-overview-page__message--error json-error-message"
					>
						{jsonParseError}
					</p>
				{/if}
				{#if saveSuccessMessage}
					<p class="profile-card__message profile-card__message--success json-save-success">
						{saveSuccessMessage}
					</p>
				{/if}
				<div class="template-overview-page__json-actions">
					<Button
						variant="primary"
						onclick={saveJsonChanges}
						disabled={!!jsonParseError || isSavingJson}
					>
						{#if isSavingJson}
							Saving...
						{:else}
							Save JSON Changes
						{/if}
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
				Cannot display visual mode. Template definition might be invalid or missing an 'activities'
				array.
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

		&__file-name {
			font-size: $font-size-sm;
			color: $color-text-secondary;
			&--placeholder {
				font-style: italic;
			}
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
