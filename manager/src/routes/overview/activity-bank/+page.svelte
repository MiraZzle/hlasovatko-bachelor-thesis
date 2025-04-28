<script lang="ts">
	import Button from '$components/elements/typography/Button.svelte';
	import Select from '$components/elements/typography/Select.svelte';
	import ActivityCard from '$components/dashboard/ActivityCard.svelte';
	import { get } from 'svelte/store';

	// Define type locally or import
	interface Activity {
		id: string;
		type: string;
		question: string;
		tags: string[];
		category?: string; // Added for filtering
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
			<Button variant="primary" onclick={createActivity}>Create Activity</Button>
			<Button variant="outline" onclick={importActivity}>Import</Button>
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
