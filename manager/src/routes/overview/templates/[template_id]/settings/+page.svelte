<script lang="ts">
	/**
	 * @file Template Settings Page
	 * This page allows users to configure settings for a specific template.
	 */
	import { page } from '$app/state';
	import Button from '$components/elements/typography/Button.svelte';
	import Select from '$components/elements/typography/Select.svelte';
	import ToggleSwitch from '$components/elements/ToggleSwitch.svelte';
	import Input from '$components/elements/typography/Input.svelte';
	import { onMount, tick } from 'svelte';
	import type { TemplateSettingsDTO } from '$lib/templates/types';
	import type { SessionMode } from '$lib/shared_types';
	import { getTemplateSettingsById, updateTemplateSettings } from '$lib/templates/template_utils';

	const templateId = $derived(page.params.template_id);
	let settings = $state<TemplateSettingsDTO>();

	// state management
	let isLoading = $state(true);
	let isSaving = $state(false);
	let saveSuccessMessage = $state<string | null>(null);
	let saveErrorMessage = $state<string | null>(null);
	let newTagInput = $state('');

	// options for select
	const pacingOptions: { value: SessionMode; label: string }[] = [
		{ value: 'teacher-paced', label: 'Teacher-Paced (Instructor advances activities)' },
		{ value: 'student-paced', label: 'Student-Paced (Participants move at their own speed)' }
	];

	/**
	 * Handle form submission to save template settings
	 * Prevents default form submission behavior
	 */
	async function handleSaveChanges(event: SubmitEvent): Promise<void> {
		event.preventDefault();
		isSaving = true;
		saveSuccessMessage = null;
		saveErrorMessage = null;

		const settingsSnapshot = $state.snapshot(settings!);
		console.log('Saving template settings snapshot:', settingsSnapshot);

		try {
			const success = await updateTemplateSettings(templateId, settingsSnapshot!);
			if (!success) {
				throw new Error('Failed to save settings');
			}
			saveSuccessMessage = 'Settings saved successfully!';
			await tick();
			setTimeout(() => (saveSuccessMessage = null), 3000);

			isSaving = false;
		} catch (error) {
			console.error('Error saving template settings:', error);
			saveErrorMessage = 'Failed to save settings. Please try again later.';
			isSaving = false;
		}
	}

	/**
	 * Add a tag to the settings
	 * @param tagValue - The tag to add
	 */
	function addTag(tagValue: string): void {
		if (!settings) {
			console.error('Settings not loaded yet');
			return;
		}

		const newTag = tagValue.trim();
		if (newTag && !settings.tags.includes(newTag)) {
			settings.tags = [...settings.tags, newTag];
		}
	}

	/**
	 * Handle new tag input from the user
	 * Adds the tag when Enter or comma is pressed
	 * @param event - The keyboard event
	 */
	function handleNewTagInput(event: KeyboardEvent & { currentTarget: HTMLInputElement }): void {
		if (event.key === 'Enter' || event.key === ',') {
			event.preventDefault();
			addTag(event.currentTarget.value);
			newTagInput = '';
		}
	}

	/**
	 * Remove a tag from the settings
	 * @param tagToRemove - The tag to remove
	 */
	function removeTag(tagToRemove: string): void {
		if (!settings) {
			console.error('Settings not loaded yet');
			return;
		}
		settings.tags = settings.tags.filter((tag) => tag !== tagToRemove);
	}

	// load template settings on mount
	onMount(async () => {
		isLoading = true;
		try {
			settings = await getTemplateSettingsById(templateId);
		} catch (error) {
			console.error('Failed to load template settings:', error);
			saveErrorMessage = 'Failed to load template settings. Please try again later.';
		}
		isLoading = false;
	});
</script>

<svelte:head>
	<title>EngaGenie | Template {templateId} - Settings</title>
</svelte:head>

<div class="template-settings-page">
	<h1 class="template-settings-page__title">Template Settings</h1>
	<p class="template-settings-page__subtitle">
		Configure default behaviors for sessions run from this template.
	</p>

	{#if isLoading}
		<p class="template-settings-page__loading">Loading settings...</p>
	{:else}
		<form class="template-settings-page__form" onsubmit={handleSaveChanges}>
			<section class="template-settings-page__section settings-card">
				<h2 class="settings-card__title">General Information</h2>
				<Input
					label="Template Title"
					id="template-title"
					bind:value={settings!.title}
					placeholder="Enter template title"
					disabled={isSaving}
				/>
			</section>

			<section class="template-settings-page__section settings-card">
				<h2 class="settings-card__title">Tags</h2>
				<div class="settings-card__field">
					<Input
						label="Add a tag"
						id="new-tag-input"
						bind:value={newTagInput}
						placeholder="Type a tag and press Enter or comma"
						onkeydown={handleNewTagInput}
						disabled={isSaving}
					/>
				</div>
				{#if settings!.tags.length > 0}
					<div class="settings-card__tags-display">
						{#each settings!.tags as tag (tag)}
							<span class="settings-card__tag">
								{tag}
								<button
									type="button"
									class="settings-card__tag-remove"
									onclick={() => removeTag(tag)}
									aria-label={`Remove tag ${tag}`}
									disabled={isSaving}
								>
									&times;
								</button>
							</span>
						{/each}
					</div>
				{:else}
					<p class="settings-card__field-description">No tags added yet.</p>
				{/if}
			</section>
			<section class="template-settings-page__section settings-card">
				<h2 class="settings-card__title">Default Session Options</h2>
				<div class="settings-card__field">
					<Select
						label="Session Pacing"
						id="session-pacing"
						options={pacingOptions}
						bind:value={settings!.sessionPacing}
						disabled={isSaving}
						ariaLabel="Default session pacing control"
					/>
					<p class="settings-card__field-description">
						Determines who controls the flow of activities during a live session.
					</p>
				</div>
				<div class="settings-card__field settings-card__field--toggle">
					<label for="results-visibility-toggle" class="settings-card__toggle-label">
						Results Visibility to Participants
					</label>
					<ToggleSwitch
						bind:checked={settings!.resultsVisibleDefault}
						disabled={isSaving}
						label="Toggle default results visibility for participants"
					/>
					<p class="settings-card__field-description">
						If enabled, participants will see activity results by default after they close.
					</p>
				</div>
			</section>

			{#if saveErrorMessage}
				<p class="template-settings-page__message template-settings-page__message--error">
					{saveErrorMessage}
				</p>
			{/if}
			{#if saveSuccessMessage}
				<p class="template-settings-page__message template-settings-page__message--success">
					{saveSuccessMessage}
				</p>
			{/if}

			<div class="template-settings-page__actions">
				<Button type="submit" variant="primary" disabled={isSaving}>
					{#if isSaving}
						Saving...
					{:else}
						Save Settings
					{/if}
				</Button>
			</div>
		</form>
	{/if}
</div>

<style lang="scss">
	.template-settings-page {
		display: flex;
		flex-direction: column;
		gap: $spacing-lg;
		&__title {
			font-size: $font-size-2xl;
			font-weight: $font-weight-bold;
			margin: 0;
		}
		&__subtitle {
			font-size: $font-size-md;
			color: $color-text-secondary;
			margin-top: -$spacing-sm;
			margin-bottom: $spacing-sm;
		}
		&__loading {
			font-style: italic;
			color: $color-text-secondary;
			text-align: center;
			padding: $spacing-xl;
		}
		&__form {
			display: flex;
			flex-direction: column;
			gap: $spacing-xl;
		}
		&__message {
			padding: $spacing-sm $spacing-md;
			border-radius: $border-radius-md;
			font-size: $font-size-sm;
			text-align: center;
			&--error {
				background-color: rgba($color-error, 0.1);
				color: $color-error;
				border: 1px solid rgba($color-error, 0.2);
			}
			&--success {
				background-color: rgba($color-success, 0.1);
				color: darken($color-success, 10%);
				border: 1px solid rgba($color-success, 0.2);
			}
		}
		&__actions {
			display: flex;
			justify-content: flex-end;
			margin-top: $spacing-md;
		}
	}

	.settings-card {
		background-color: $color-surface;
		border-radius: $border-radius-lg;
		padding: $spacing-xl;
		border: 1px solid $color-border-light;
		box-shadow: $box-shadow-sm;
		display: flex;
		flex-direction: column;
		gap: $spacing-lg;
		&__title {
			font-size: $font-size-lg;
			font-weight: $font-weight-semibold;
			margin: 0 0 $spacing-sm 0;
			padding-bottom: $spacing-sm;
			border-bottom: 1px solid $color-border-light;
		}

		&__field-description {
			font-size: $font-size-xs;
			color: $color-text-secondary;
			margin-top: $spacing-xs;
			padding-left: $spacing-xs;
		}
		&__field--toggle {
			display: flex;
			align-items: center;
			justify-content: space-between;
			flex-wrap: wrap;
			gap: $spacing-md;
			.settings-card__toggle-label {
				font-size: $font-size-md;
				font-weight: $font-weight-medium;
				color: $color-text-primary;
				flex-grow: 1;
			}
			.settings-card__field-description {
				width: 100%;
				padding-left: 0;
				margin-top: $spacing-xs;
				order: 3;
			}
		}
		&__tags-display {
			display: flex;
			flex-wrap: wrap;
			gap: $spacing-sm;
			margin-top: $spacing-xs;
		}
		&__tag {
			display: inline-flex;
			align-items: center;
			background-color: $color-primary-light;
			color: $color-text-on-primary;
			padding: $spacing-xs $spacing-sm;
			border-radius: $border-radius-pill;
			font-size: $font-size-sm;
			font-weight: $font-weight-medium;
		}
		&__tag-remove {
			background: none;
			border: none;
			color: inherit;
			cursor: pointer;
			margin-left: $spacing-xs;
			padding: 0;
			line-height: 1;
			font-size: $font-size-md;
			opacity: 0.7;
			&:hover {
				opacity: 1;
			}
		}
	}
</style>
