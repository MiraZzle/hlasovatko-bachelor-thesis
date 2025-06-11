<script lang="ts">
	import Button from '$components/elements/typography/Button.svelte';
	import Select from '$components/elements/typography/Select.svelte';
	import ActivityCard from '$components/dashboard/ActivityCard.svelte';
	import AddActivityModal from '$components/elements/modals/AddActivityModal.svelte';
	import { get } from 'svelte/store';

	// Define type locally or import
	interface Activity {
		id: string;
		type: string;
		question: string;
		tags: string[];
		category?: string; // Added for filtering
	}

	interface NewActivityData {
		title: string;
		type: string;
		definition: string;
		categories: string[];
	}

	// --- State for Modal ---
	let isCreateActivityModalOpen = $state(false);
	// Options needed for the modal's select dropdown
	const availableActivityTypes = $state([
		{ value: 'quiz', label: 'Quiz' },
		{ value: 'rating', label: 'Rating' },
		{ value: 'poll', label: 'Poll' },
		{ value: 'word_cloud', label: 'Word Cloud' }
		// Add other types here
	]);

	let fileInputRef: HTMLInputElement | null = null; // Reference variable

	// --- Modal Handlers ---
	function openCreateActivityModal(): void {
		isCreateActivityModalOpen = true;
	}
	function closeCreateActivityModal(): void {
		isCreateActivityModalOpen = false;
	}
	function handleAddActivitySubmit(data: NewActivityData): void {
		console.log('Adding activity (from page):', data);
		// --- TODO: Call API to save activity ---

		// Example: Add to dummy list (adapt structure as needed)
		activities.push({
			id: `act-${Math.random().toString(16).substring(2, 8)}`,
			// title: data.title, // Title isn't part of ActivityCard, maybe add?
			type: data.type, // Might need mapping label back from value?
			question: data.definition.substring(0, 50) + '...', // Example: Use definition snippet as question for card
			tags: data.categories
			// definition: data.definition // Store full definition if needed
			// category: ??? // Determine category somehow?
		});
		activities = activities; // Trigger reactivity

		// Modal closes itself via onclose binding/callback
	}

	// --- Import Activity Handlers ---
	// Updated: This function now triggers the hidden file input
	function triggerImport(): void {
		// Optional: Reset file input value before opening dialog
		if (fileInputRef) {
			fileInputRef.value = '';
		}
		fileInputRef?.click(); // Programmatically click the hidden input
	}

	// NEW: Handles the file selection after the user chooses a file
	async function handleFileSelected(event: Event): Promise<void> {
		const target = event.target as HTMLInputElement;
		const files = target.files;

		if (files && files.length > 0) {
			const file = files[0];
			console.log('Selected file:', file.name, file.type, file.size);

			// Basic validation
			if (!file.name.toLowerCase().endsWith('.json') && file.type !== 'application/json') {
				alert('Please select a valid JSON file (.json).');
				return;
			}

			// --- TODO: Process the file ---
			// Option 1: Read content client-side (if small / needed for preview)
			// const reader = new FileReader();
			// reader.onload = (e) => {
			//     try {
			//         const content = e.target?.result as string;
			//         const jsonData = JSON.parse(content);
			//         console.log('Imported JSON content:', jsonData);
			//         // Add logic to create activity from jsonData
			//         alert(`Successfully read ${file.name}`);
			//     } catch (err) {
			//         console.error("Error reading/parsing file:", err);
			//         alert(`Error reading file: ${err instanceof Error ? err.message : 'Invalid JSON format'}`);
			//     }
			// };
			// reader.onerror = (e) => {
			//     console.error("FileReader error:", e);
			//     alert('Error reading file.');
			// };
			// reader.readAsText(file);

			// Option 2: Send file to server (more common)
			console.log(`Placeholder: Uploading ${file.name} to server...`);
			alert(`Importing ${file.name}... (Placeholder)`);
			// const formData = new FormData();
			// formData.append('activityFile', file);
			// try {
			//      const response = await fetch('/api/activities/import', { method: 'POST', body: formData });
			//      // ... handle response ...
			// } catch (err) { console.error(err); alert('Import failed'); }
		} else {
			console.log('No file selected.');
		}

		// It's generally good practice to clear the input value after handling
		// But since we clear it *before* click, it might not be needed here unless
		// there was an error during processing.
		// if (target) target.value = '';
	}

	// --- State ---
	let selectedCategory = $state<string>('all'); // Default or fetched initial value
	let selectedActivityType = $state<string>('all'); // Default or fetched initial value

	// Dummy options (replace with fetched data)
	const categoryOptions = [
		{ value: 'all', label: 'All Categories' },
		{ value: 'physics', label: 'Physics' },
		{ value: 'chemistry', label: 'Chemistry' },
		{ value: 'general', label: 'General' }
	];
	const activityTypeOptions = [
		{ value: 'all', label: 'All Activity Types' },
		{ value: 'quiz', label: 'Quiz' },
		{ value: 'rating', label: 'Rating' },
		{ value: 'poll', label: 'Poll' },
		{ value: 'word_cloud', label: 'Word Cloud' }
	];

	// Dummy activity data (replace with fetched data)
	let activities = $state<Activity[]>([
		{
			id: 'ab1',
			type: 'Quiz',
			question: "What word comes to mind when you hear 'physics'?",
			tags: ['Physics', 'Event'],
			category: 'physics'
		},
		{
			id: 'ab2',
			type: 'Rating',
			question: "Rate today's lecture on a scale of 1-5.",
			tags: ['Event', 'Feedback'],
			category: 'general'
		},
		{
			id: 'ab3',
			type: 'Quiz',
			question: "How confident are you with today's topic?",
			tags: ['Event'],
			category: 'general'
		},
		{
			id: 'ab4',
			type: 'Quiz',
			question: 'What is the main concept of Thermodynamics?',
			tags: ['Physics', 'Definitions'],
			category: 'physics'
		},
		{
			id: 'ab5',
			type: 'Word Cloud',
			question: 'Describe "Entropy" in one word.',
			tags: ['Physics', 'Concept'],
			category: 'physics'
		},
		{
			id: 'ab6',
			type: 'Poll',
			question: 'Should the next lecture cover topic A or B?',
			tags: ['Planning'],
			category: 'general'
		}
	]);

	// --- Handlers ---
	function createActivity() {
		console.log('Navigate to create activity');
		// goto('/activity-bank/new');
	}
	function importActivity() {
		console.log('Open import activity modal/page');
	}
	function handleCardClick(id: string) {
		console.log(`Card ${id} clicked - open details or select`);
		// goto(`/activity-bank/${id}`);
	}

	function getFilteredActivities() {
		return activities.filter((activity) => {
			const categoryMatch =
				selectedCategory === 'all' || activity.category?.toLowerCase() === selectedCategory;
			const typeMatch =
				selectedActivityType === 'all' || activity.type.toLowerCase() === selectedActivityType;
			return categoryMatch && typeMatch;
		});
	}

	// --- Filtering Logic ---
	let filteredActivities = $derived(getFilteredActivities());
</script>

<svelte:head>
	<title>Activity Bank - EngaGenie</title>
</svelte:head>

<div class="activity-bank-page">
	<h1 class="activity-bank-page__title">Activity bank</h1>

	<div class="activity-bank-page__controls">
		<div class="activity-bank-page__actions">
			<Button variant="primary" onclick={openCreateActivityModal}>Create Activity</Button>
			<Button variant="outline" onclick={triggerImport}>Import from JSON</Button>
			<input
				type="file"
				accept=".json,application/json"
				bind:this={fileInputRef}
				on:change={handleFileSelected}
				style="display: none;"
				aria-hidden="true"
				tabindex="-1"
			/>
		</div>
		<div class="activity-bank-page__filters">
			<Select
				label="Category"
				options={categoryOptions}
				bind:value={selectedCategory}
				ariaLabel="Filter by category"
			/>
			<Select
				label="Activity type"
				options={activityTypeOptions}
				bind:value={selectedActivityType}
				ariaLabel="Filter by activity type"
			/>
		</div>
	</div>

	<div class="activity-bank-page__grid">
		{#each filteredActivities as activity}
			<ActivityCard {activity} onclick={handleCardClick} />
		{:else}
			<p class="activity-bank-page__no-results">No activities found matching your filters.</p>
		{/each}
	</div>

	<AddActivityModal
		bind:open={isCreateActivityModalOpen}
		activityTypes={availableActivityTypes}
		onAdd={handleAddActivitySubmit}
		onclose={closeCreateActivityModal}
	/>
</div>

<style lang="scss">
	@import '../../../styles/variables.scss';

	// Block: activity-bank-page
	.activity-bank-page {
		// No specific styles needed usually, padding handled by layout

		// Element: Title
		&__title {
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			margin-bottom: $spacing-lg; // Space below title
		}

		// Element: Controls container (actions and filters)
		&__controls {
			display: flex;
			justify-content: space-between;
			flex-direction: column; // Align filter labels/buttons nicely
			flex-wrap: wrap; // Allow wrapping
			gap: $spacing-lg; // Gap between actions/filters groups
			margin-bottom: $spacing-xl;
		}

		// Element: Actions buttons group
		&__actions {
			display: flex;
			gap: $spacing-md;
		}

		// Element: Filters group
		&__filters {
			display: flex;
			gap: $spacing-md;
			flex-wrap: wrap; // Allow filters to wrap too
		}

		// Element: Grid for cards
		&__grid {
			display: grid;
			// Responsive grid - adjust minmax/columns as desired
			grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
			gap: $spacing-lg;
		}

		// Element: Message when no results
		&__no-results {
			color: $color-text-secondary;
			font-style: italic;
			grid-column: 1 / -1; // Span full grid width if grid is empty
			text-align: center;
			padding: $spacing-xl;
		}
	}
</style>
