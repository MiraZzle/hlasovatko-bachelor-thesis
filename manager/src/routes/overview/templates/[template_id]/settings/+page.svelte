<script lang="ts">
	import { page } from '$app/stores';
	import Button from '$components/elements/typography/Button.svelte'; // Verify path
	import Select from '$components/elements/typography/Select.svelte'; // Verify path
	import ToggleSwitch from '$components/elements/ToggleSwitch.svelte'; // Verify path
	import Input from '$components/elements/typography/Input.svelte'; // Verify path
	import { onMount, tick } from 'svelte'; // Added tick

	// --- Get Template ID ---
	let { template_id } = $page.params;

	// --- Define Types for Settings ---
	type SessionPacing = 'teacher' | 'student';
	interface TemplateSettingsData {
		sessionPacing: SessionPacing;
		resultsVisibleDefault: boolean;
		title?: string;
		description?: string;
		tags: string[]; // <<< ADDED: Tags array
	}

	// --- State for Settings ---
	let settings = $state<TemplateSettingsData>({
		sessionPacing: 'teacher',
		resultsVisibleDefault: true,
		title: `Template ${template_id}`,
		description: 'Default description for this amazing template.',
		tags: ['Example', 'Physics 101'] // <<< ADDED: Dummy tags
	});

	let isLoading = $state(true);
	let isSaving = $state(false);
	let saveSuccessMessage = $state<string | null>(null);
	let saveErrorMessage = $state<string | null>(null);

	// --- State for Tag Input ---
	let newTagInput = $state('');

	// --- Options for Select ---
	const pacingOptions: { value: SessionPacing; label: string }[] = [
		{ value: 'teacher', label: 'Teacher-Paced (Instructor advances activities)' },
		{ value: 'student', label: 'Student-Paced (Participants move at their own speed)' }
	];

	// --- Fetch Initial Settings (Simulated) ---
	onMount(async () => {
		isLoading = true;
		console.log(`Fetching settings for template ${template_id}...`);
		// --- TODO: API Call to fetch template settings ---
		await new Promise((resolve) => setTimeout(resolve, 500));
		// Example: Update state with fetched data
		// settings.title = `Fetched Title for ${template_id}`;
		// settings.tags = ['Fetched Tag 1', 'Fetched Tag 2'];
		isLoading = false;
	});

	// --- Handlers ---
	async function handleSaveChanges(event: SubmitEvent) {
		event.preventDefault();
		isSaving = true;
		saveSuccessMessage = null;
		saveErrorMessage = null;
		console.log('Saving template settings:', settings);

		// --- TODO: API Call to save settings (including settings.tags) ---
		await new Promise((resolve) => setTimeout(resolve, 1000));

		saveSuccessMessage = 'Settings saved successfully!';
		await tick(); // Ensure DOM updates before timeout
		setTimeout(() => (saveSuccessMessage = null), 3000);

		isSaving = false;
	}

	// --- Tag Management Handlers ---
	function addTag(tagValue: string): void {
		const newTag = tagValue.trim();
		if (newTag && !settings.tags.includes(newTag)) {
			settings.tags = [...settings.tags, newTag];
		}
	}

	function handleNewTagInput(event: KeyboardEvent & { currentTarget: HTMLInputElement }): void {
		if (event.key === 'Enter' || event.key === ',') {
			event.preventDefault();
			addTag(event.currentTarget.value);
			newTagInput = ''; // Clear input
		}
	}
	function handleAddTagButton(): void {
		addTag(newTagInput);
		newTagInput = ''; // Clear input
	}

	function removeTag(tagToRemove: string): void {
		settings.tags = settings.tags.filter((tag) => tag !== tagToRemove);
	}
</script>

<svelte:head>
	<title>Settings for Template {template_id} - EngaGenie</title>
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
					bind:value={settings.title}
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
				{#if settings.tags.length > 0}
					<div class="settings-card__tags-display">
						{#each settings.tags as tag (tag)}
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
						bind:value={settings.sessionPacing}
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
						bind:checked={settings.resultsVisibleDefault}
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
	@import '../../../../../styles/variables.scss'; // Adjust path depth

	// Block: template-settings-page
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
		&__section {
			/* relies on .settings-card */
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

	// Block: settings-card
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
		&__field {
			/* styles */
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
		&__toggle-label {
			/* used above */
		}

		// --- NEW: Tag Display Styles ---
		&__tags-display {
			display: flex;
			flex-wrap: wrap;
			gap: $spacing-sm;
			margin-top: $spacing-xs; // Space below the input field
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
			color: inherit; // Inherit color from tag
			cursor: pointer;
			margin-left: $spacing-xs;
			padding: 0;
			line-height: 1;
			font-size: $font-size-md; // Make 'x' a bit larger
			opacity: 0.7;
			&:hover {
				opacity: 1;
			}
		}
	}
</style>
