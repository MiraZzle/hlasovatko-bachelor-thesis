<script lang="ts">
	import { page } from '$app/stores';
	import Button from '$components/elements/typography/Button.svelte'; // Verify path
	import SessionActivityItem from '$components/activities/SessionActivityItem.svelte'; // Verify path
	import type { SessionActivity } from '$lib/activity_types'; // Adjust path

	// --- Get Session ID ---
	let { session_id } = $page.params;

	// --- Dummy Data (Replace with actual fetch based on session_id) ---
	let activities = $state<SessionActivity[]>([
		{
			id: 'sact1',
			type: 'Poll',
			title: 'Which topic should we cover next?',
			definition: {
				type: 'Poll',
				options: [
					{ id: 'o1', text: 'Topic A' },
					{ id: 'o2', text: 'Topic B' }
				]
			},
			status: 'Closed',
			order: 1
		},
		{
			id: 'sact2',
			type: 'MultipleChoice', // Type on the main object
			title: 'What is the powerhouse of the cell?',
			// Add 'type' inside definition
			definition: {
				type: 'MultipleChoice', // <<< ADDED
				options: [
					{ id: 'm1', text: 'Nucleus' },
					{ id: 'm2', text: 'Ribosome' },
					{ id: 'm3', text: 'Mitochondrion' },
					{ id: 'm4', text: 'Chloroplast' }
				],
				correctOptionId: 'm3',
				allowMultiple: false
			},
			status: 'Active',
			order: 2
		},
		{
			id: 'sact3',
			type: 'ScaleRating', // Type on the main object
			title: 'Rate your understanding (1-5)',
			// Add 'type' inside definition
			definition: {
				type: 'ScaleRating',
				min: 1,
				max: 5,
				minLabel: 'Confused',
				maxLabel: 'Confident'
			},
			status: 'Pending',
			order: 3
		},
		{
			id: 'sact4',
			type: 'OpenEnded', // Type on the main object
			title: 'Any remaining questions?',
			// Add 'type' inside definition
			definition: { type: 'OpenEnded' },
			status: 'Pending',
			order: 4
		},
		{
			id: 'sact5',
			type: 'UnknownType', // Type on the main object
			title: 'Custom Activity Format',
			// Definition might or might not have a 'type' matching the outer one
			definition: { customField: 'value', structure: { nested: true } },
			status: 'Pending',
			order: 5
		}
	]);

	// --- Handlers ---
	function addActivity() {
		console.log('Open Add Activity modal/page for session:', session_id);
		// Could open a modal or navigate to a dedicated add page
	}

	function handleStartActivity(activityId: string) {
		console.log(`Starting activity ${activityId} in session ${session_id}`);
		// TODO: API Call
		// Update local state for immediate feedback
		activities = activities.map((act) =>
			act.id === activityId ? { ...act, status: 'Active' } : act
		);
	}

	function handleStopActivity(activityId: string) {
		console.log(`Stopping activity ${activityId} in session ${session_id}`);
		// TODO: API Call
		activities = activities.map((act) =>
			act.id === activityId ? { ...act, status: 'Closed' } : act
		);
	}

	function handleViewResults(activityId: string) {
		console.log(`Viewing results for activity ${activityId} in session ${session_id}`);
		// TODO: Navigate to results page or open results modal
		alert(`Viewing results for ${activityId} (Placeholder)`);
	}

	// --- Sorting (Optional) ---
	// Use $derived if you want activities sorted by order

	function getSortedActivities(activities: SessionActivity[]) {
		return [...activities].sort((a, b) => (a.order ?? 0) - (b.order ?? 0));
	}

	let sortedActivities = $derived(getSortedActivities(activities));
</script>

<svelte:head>
	<title>Activities for Session {session_id} - EngaGenie</title>
</svelte:head>

<div class="session-activities-page">
	<header class="session-activities-page__header">
		<h1 class="session-activities-page__title">Session Activities</h1>
		<Button variant="primary" onclick={addActivity}>+ Add Activity</Button>
	</header>

	<div class="session-activities-page__list">
		{#each sortedActivities as activity}
			<SessionActivityItem
				{activity}
				onStart={handleStartActivity}
				onStop={handleStopActivity}
				onViewResults={handleViewResults}
			/>
		{:else}
			<p class="session-activities-page__no-results">No activities added to this session yet.</p>
		{/each}
	</div>
</div>

<style lang="scss">
	@import '../../../../styles/variables.scss'; // Adjust path depth

	// Block: session-activities-page
	.session-activities-page {
		// Element: Header
		&__header {
			display: flex;
			justify-content: space-between;
			align-items: center;
			margin-bottom: $spacing-xl;
			flex-wrap: wrap;
			gap: $spacing-md;
		}

		// Element: Title
		&__title {
			font-size: $font-size-2xl; // Slightly smaller than main page titles?
			font-weight: $font-weight-bold;
			margin: 0;
		}

		// Element: List Container
		&__list {
			display: flex;
			flex-direction: column;
			gap: $spacing-lg; // Space between activity items
		}

		// Element: No Results Message
		&__no-results {
			color: $color-text-secondary;
			font-style: italic;
			text-align: center;
			padding: $spacing-xl;
			background-color: $color-surface;
			border-radius: $border-radius-md;
			border: 1px dashed $color-border-light;
		}
	}
</style>
