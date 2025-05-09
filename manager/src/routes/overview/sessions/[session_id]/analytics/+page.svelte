<script lang="ts">
	import { page } from '$app/stores';
	import SessionAnalyticsItem from '$components/analytics/SessionAnalyticsItem.svelte';
	import type { SessionActivity } from '$lib/activity_types'; // Adjust path

	let { session_id } = $page.params;

	// --- Dummy Data (Replace with actual fetch for session activities AND results) ---
	let activitiesWithResults = $state<SessionActivity[]>([
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
			order: 1,
			participantCount: 20,
			responseCount: 18,
			results: [
				{ id: 'o1', text: 'Topic A', count: 12 },
				{ id: 'o2', text: 'Topic B', count: 6 }
			]
		},
		{
			id: 'sact2',
			type: 'MultipleChoice',
			title: 'What is the powerhouse of the cell?',
			definition: {
				type: 'MultipleChoice',
				options: [
					{ id: 'm1', text: 'Nucleus' },
					{ id: 'm2', text: 'Ribosome' },
					{ id: 'm3', text: 'Mitochondrion' }
				],
				correctOptionId: 'm3'
			},
			status: 'Closed',
			order: 2,
			participantCount: 20,
			responseCount: 19,
			results: [
				{ id: 'm1', text: 'Nucleus', count: 2 },
				{ id: 'm2', text: 'Ribosome', count: 1 },
				{ id: 'm3', text: 'Mitochondrion', count: 16 }
			]
		},
		{
			id: 'sact3',
			type: 'ScaleRating',
			title: 'Rate your understanding (1-5)',
			definition: {
				type: 'ScaleRating',
				min: 1,
				max: 5,
				minLabel: 'Confused',
				maxLabel: 'Confident'
			},
			status: 'Closed',
			order: 3,
			participantCount: 20,
			responseCount: 17,
			results: [
				{ rating: 1, count: 0 },
				{ rating: 2, count: 1 },
				{ rating: 3, count: 5 },
				{ rating: 4, count: 8 },
				{ rating: 5, count: 3 }
			]
		},
		{
			id: 'sact4',
			type: 'OpenEnded',
			title: 'Any remaining questions?',
			definition: { type: 'OpenEnded' },
			status: 'Closed',
			order: 4,
			participantCount: 20,
			responseCount: 5,
			results: [
				'No questions.',
				'Explain slide 10 again.',
				'Can we have more examples?',
				'None',
				'What is the deadline?'
			]
		},
		{
			id: 'sact5',
			type: 'UnknownType',
			title: 'Custom Activity Format',
			definition: { customField: 'value', structure: { nested: true } },
			status: 'Closed',
			order: 5,
			participantCount: 20,
			responseCount: 0,
			results: null // No results or unknown format
		}
	]);
</script>

<svelte:head>
	<title>Analytics for Session {session_id} - EngaGenie</title>
</svelte:head>

<div class="session-analytics-page">
	<header class="session-analytics-page__header">
		<h1 class="session-analytics-page__title">Session Analytics</h1>
	</header>

	<div class="session-analytics-page__list">
		{#each activitiesWithResults as activity}
			<SessionAnalyticsItem {activity} />
		{:else}
			<p class="session-analytics-page__no-results">
				No activities have been run in this session yet.
			</p>
		{/each}
	</div>
</div>

<style lang="scss">
	@import '../../../../../styles/variables.scss'; // Adjust path

	.session-analytics-page {
		&__header {
			margin-bottom: $spacing-xl;
			// Add flex if putting stats next to title
		}
		&__title {
			font-size: $font-size-2xl;
			font-weight: $font-weight-bold;
			margin-bottom: $spacing-md; // Space below title if stats are separate
		}
		&__list {
			display: flex;
			flex-direction: column;
			gap: $spacing-lg;
		}
	}
</style>
