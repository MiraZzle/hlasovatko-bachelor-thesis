<script lang="ts">
	import { page } from '$app/stores';
	import Button from '$components/elements/typography/Button.svelte';
	import LiveActivityQueueItem from '$components/activities/LiveActivityQueueItem.svelte'; // Verify path
	import LiveResultsDisplay from '$components/activities/LiveResultsDisplay.svelte'; // Verify path
	// Import display components for the teacher's view of the current activity
	import MultipleChoiceDisplay from '$components/activities/MultipleChoice.svelte'; // Verify path
	import PollDisplay from '$components/activities/Poll.svelte'; // Verify path
	import ScaleRatingDisplay from '$components/activities/ScaleRating.svelte'; // Verify path
	import OpenEndedDisplay from '$components/activities/OpenEnded.svelte'; // Verify path
	import RawJsonDisplay from '$components/activities/RawJson.svelte'; // Verify path
	import SessionAnalyticsItem from '$components/analytics/SessionAnalyticsItem.svelte'; // Verify path

	import type {
		MultipleChoiceDefinition,
		PollDefinition,
		ScaleRatingDefinition
	} from '$lib/activity_types'; // Adjust path
	import type { SessionActivity, KnownActivityDefinition } from '$lib/types'; // Adjust path
	import {
		getKnownDefinition,
		isMultipleChoice,
		isPoll,
		isScaleRating,
		isOpenEnded
	} from '$lib/types'; // Adjust path
	import { onMount } from 'svelte';
	import MultipleChoice from '$components/activities/MultipleChoice.svelte';

	// --- Get Session ID ---
	let { session_id } = $page.params;

	// --- State ---
	let participantCount = $state(Math.floor(Math.random() * 50));
	let sessionCode = $state('ABCXYZ');
	let sessionJoinLink = $derived(`${$page.url.origin}/join/${sessionCode}`);

	let allActivities = $state<SessionActivity[]>([
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
			status: 'Pending',
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
				correctOptionId: 'm3',
				allowMultiple: false
			},
			status: 'Pending',
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
			status: 'Pending',
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
			status: 'Pending',
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
		}
	]);

	let currentActivityId = $state<string | null>(null);
	let showResultsToParticipants = $state(false); // Teacher sees results, this controls student visibility

	// --- Derived State ---
	let currentActivity = $derived(allActivities.find((act) => act.id === currentActivityId) ?? null);
	let currentActivityParsedDef = $derived(() => {
		const activity = currentActivity;
		if (!activity?.definition) return null;
		let def = activity.definition;
		if (typeof def === 'string') {
			try {
				def = JSON.parse(def);
			} catch (e) {
				return { error: 'Invalid JSON', original: activity.definition };
			}
		}
		if (
			typeof def === 'object' &&
			def !== null &&
			!('type' in def) &&
			typeof activity.type === 'string'
		) {
			return { ...def, type: activity.type };
		}
		return def;
	});
	// This is not strictly needed if type guards are used on currentActivityParsedDef directly
	// let currentActivityKnownDef = $derived(currentActivity ? getKnownDefinition({ ...currentActivity, definition: currentActivityParsedDef() }) : null);

	// --- Handlers ---
	function activateActivity(activityId: string) {
		if (currentActivityId) {
			allActivities = allActivities.map((a) =>
				a.id === currentActivityId ? { ...a, status: 'Closed' } : a
			);
		}
		currentActivityId = activityId;
		showResultsToParticipants = false;
		allActivities = allActivities.map((a) =>
			a.id === activityId ? { ...a, status: 'Active' } : a
		);
		console.log(`Teacher activating activity: ${activityId}`);
	}

	function stopCurrentActivity() {
		if (currentActivityId) {
			console.log(`Teacher stopping activity: ${currentActivityId}`);
			allActivities = allActivities.map((a) =>
				a.id === currentActivityId ? { ...a, status: 'Closed' } : a
			);
			// currentActivityId = null; // Decide if it clears or just sets to closed
		}
	}

	function toggleResultsVisibility() {
		showResultsToParticipants = !showResultsToParticipants;
		console.log(`Results visibility for participants: ${showResultsToParticipants}`);
	}

	function advanceToNext() {
		const currentIdx = currentActivity
			? allActivities.findIndex((act) => act.id === currentActivityId)
			: -1;
		let nextPendingActivity = null;
		for (let i = currentIdx + 1; i < allActivities.length; i++) {
			if (allActivities[i].status === 'Pending') {
				nextPendingActivity = allActivities[i];
				break;
			}
		}

		if (nextPendingActivity) {
			activateActivity(nextPendingActivity.id);
		} else {
			// If current activity was active, close it
			if (currentActivity && currentActivity.status === 'Active') {
				stopCurrentActivity();
			}
			console.log('No more pending activities or end of queue.');
			alert('End of activity queue!');
			// currentActivityId = null; // Optionally clear current activity
		}
	}

	function endSession() {
		/* ... */
	}
	onMount(() => {
		/* ... mock participant updates ... */
	});
</script>

<svelte:head>
	<title>Presenting Session {session_id} - EngaGenie</title>
</svelte:head>

<div class="live-session-page">
	<header class="live-session-page__top-bar">
		<div class="live-session-page__session-info">
			<span>Code: <strong>{sessionCode}</strong></span>
			<span>Participants: <strong>{participantCount}</strong></span>
		</div>
		<Button variant="danger" onclick={endSession}>End Session</Button>
	</header>

	<div class="live-session-page__main-grid">
		<aside class="live-session-page__activity-queue">
			<h2 class="live-session-page__panel-title">Activity Queue</h2>
			<div class="live-session-page__queue-list">
				{#each allActivities as activity (activity.id)}
					<LiveActivityQueueItem
						{activity}
						isCurrent={activity.id === currentActivityId}
						onActivate={activateActivity}
					/>
				{/each}
			</div>
		</aside>

		<section class="live-session-page__center-panel">
			<div class="live-session-page__current-activity-display">
				<h2 class="live-session-page__panel-title">Current Activity</h2>
				{#if currentActivity && currentActivityParsedDef()}
					<div class="current-activity-card">
						<div class="current-activity-card__header">
							<span class="current-activity-card__type">{currentActivity.type}</span>
							<h3 class="current-activity-card__title">{currentActivity.title}</h3>
						</div>
						<div class="current-activity-card__body">
							{#if isMultipleChoice(currentActivityParsedDef())}
								<MultipleChoiceDisplay
									definition={currentActivityParsedDef() as MultipleChoiceDefinition}
								/>
							{:else if isPoll(currentActivityParsedDef())}
								<PollDisplay definition={currentActivityParsedDef() as PollDefinition} />
							{:else if isScaleRating(currentActivityParsedDef())}
								<ScaleRatingDisplay
									definition={currentActivityParsedDef() as ScaleRatingDefinition}
								/>
							{:else if isOpenEnded(currentActivityParsedDef())}
								<OpenEndedDisplay />
							{:else}
								<RawJsonDisplay definition={currentActivityParsedDef()} />
							{/if}
						</div>
						<div class="current-activity-card__controls">
							{#if currentActivity.status === 'Active'}
								<Button variant="warning" size="sm" onclick={stopCurrentActivity}
									>Stop Activity</Button
								>
							{:else if currentActivity.status === 'Pending' || currentActivity.status === 'Closed'}
								<Button
									variant="success"
									size="sm"
									onclick={() => activateActivity(currentActivity.id)}>Start This Activity</Button
								>
							{/if}
							<Button variant="outline" size="sm" onclick={toggleResultsVisibility}>
								{showResultsToParticipants ? 'Hide Results (Students)' : 'Show Results (Students)'}
							</Button>
							<Button variant="primary" size="sm" onclick={advanceToNext}
								>Next Activity &rarr;</Button
							>
						</div>
					</div>
				{:else}
					<div class="current-activity-panel__placeholder">
						<p>No activity selected or active.</p>
						<Button variant="primary" onclick={advanceToNext}>Start First Activity</Button>
					</div>
				{/if}
			</div>

			<div class="live-session-page__live-results-display">
				<h2 class="live-session-page__panel-title">Live Results</h2>
				{#if currentActivity}
					<SessionAnalyticsItem activity={currentActivity} />
				{:else}
					<div class="live-results-placeholder">
						<p>Activate an activity to see live results.</p>
					</div>
				{/if}
			</div>
		</section>
	</div>
</div>

<style lang="scss">
	@import '../../../styles/variables.scss'; // Adjust path

	// Block: live-session-page
	.live-session-page {
		display: flex;
		flex-direction: column;
		height: 100%; // Fill the content slot of the parent layout
		overflow: hidden; // Prevent overall page scroll

		// Element: Top Control Bar
		&__top-bar {
			display: flex;
			justify-content: space-between;
			align-items: center;
			padding: $spacing-sm $spacing-md;
			background-color: darken($color-surface, 3%);
			border-bottom: 1px solid $color-border;
			flex-shrink: 0;
		}
		&__session-info {
			display: flex;
			gap: $spacing-lg;
			font-size: $font-size-sm;
			color: $color-text-secondary;
			strong {
				color: $color-text-primary;
				font-weight: $font-weight-semibold;
			}
		}

		// Element: Main Content Grid
		&__main-grid {
			display: grid;
			grid-template-columns: 1fr; // Mobile default
			grid-template-rows: auto auto auto; // Stack panels on mobile
			gap: $spacing-lg;
			padding: $spacing-md;
			flex-grow: 1;
			overflow: hidden; // Grid itself should not scroll, panels will

			@media (min-width: $breakpoint-lg) {
				grid-template-columns: 0.5fr 1.75fr;
				grid-template-rows: 1fr; // Single row for desktop, all columns same height
				padding: $spacing-lg;
			}
		}

		// Element: Panel Title (common)
		&__panel-title {
			font-size: $font-size-md;
			font-weight: $font-weight-semibold;
			color: $color-text-secondary;
			margin: 0 0 $spacing-sm 0;
			text-transform: uppercase;
			letter-spacing: 0.5px;
			flex-shrink: 0;
		}

		// Element: Activity Queue Panel
		&__activity-queue {
			background-color: $color-surface-alt;
			border-radius: $border-radius-md;
			padding: $spacing-md;
			display: flex;
			flex-direction: column;
			gap: $spacing-sm;
			min-height: 0; // Needed for flex child to scroll
			overflow-y: auto; // Scrollable queue list
		}
		&__queue-list {
			display: flex;
			flex-direction: column;
			gap: $spacing-sm;
		}

		// Element: Center Panel (New wrapper for Current Activity + Results)
		&__center-panel {
			display: flex;
			flex-direction: column;
			gap: $spacing-lg; // Space between current activity and results
			min-height: 0; // Needed for flex child to scroll
			overflow: hidden; // Prevent this panel from scrolling, children will
		}

		// Element: Current Activity Display Area (within center panel)
		&__current-activity-display {
			display: flex;
			flex-direction: column;
			flex-shrink: 1; // Allow shrinking but prioritize
			min-height: 0; // Allow shrinking for scroll within card body
			// background-color: $color-surface; // Moved to card
			// border-radius: $border-radius-lg; // Moved to card
			// padding: $spacing-lg; // Moved to card
			// border: 1px solid $color-border; // Moved to card
			// box-shadow: $box-shadow-md; // Moved to card

			.current-activity-card {
				background-color: $color-surface;
				border-radius: $border-radius-lg;
				padding: $spacing-lg;
				border: 1px solid $color-border;
				box-shadow: $box-shadow-md;
				display: flex;
				flex-direction: column;
				gap: $spacing-md;
				flex-grow: 1; // Card takes available space
				min-height: 0; // Allow card to shrink and body to scroll

				&__header {
					display: flex;
					align-items: baseline;
					gap: $spacing-sm;
					flex-shrink: 0;
				}
				&__type {
					background-color: $color-primary-light;
					color: $color-text-on-primary;
					padding: $spacing-xs $spacing-sm;
					border-radius: $border-radius-sm;
					font-size: $font-size-xs;
					font-weight: $font-weight-semibold;
					text-transform: uppercase;
				}
				&__title {
					font-size: $font-size-lg;
					font-weight: $font-weight-bold;
					margin: 0;
					flex-grow: 1;
				}
				&__body {
					flex-grow: 1;
					overflow-y: auto; /* If content can be long */
					padding-right: $spacing-xs; /* For scrollbar */
				}
				&__controls {
					display: flex;
					gap: $spacing-sm;
					flex-wrap: wrap;
					justify-content: flex-end;
					border-top: 1px solid $color-border-light;
					padding-top: $spacing-md;
					margin-top: $spacing-md;
					flex-shrink: 0;
				}
			}
			.current-activity-panel__placeholder {
				display: flex;
				flex-direction: column;
				align-items: center;
				justify-content: center;
				text-align: center;
				flex-grow: 1;
				color: $color-text-secondary;
				background-color: $color-surface;
				border-radius: $border-radius-lg;
				padding: $spacing-xl;
				border: 1px dashed $color-border-light;
				p {
					margin-bottom: $spacing-md;
				}
			}
		}

		// Element: Live Results Display Area (within center panel)
		&__live-results-display {
			display: flex;
			flex-direction: column;
			flex-shrink: 1; // Allow shrinking but prioritize
			min-height: 0; // Allow shrinking for scroll within LiveResultsDisplay
			// background-color: $color-surface; // Moved to LiveResultsDisplay component
			// border-radius: $border-radius-lg; // Moved to LiveResultsDisplay component
			// padding: $spacing-md; // Moved to LiveResultsDisplay component
			// border: 1px solid $color-border-light; // Moved to LiveResultsDisplay component
		}

		// Element: Interaction Panel (Rightmost)
		&__interaction-panel {
			background-color: $color-surface-alt;
			border-radius: $border-radius-md;
			padding: $spacing-md;
			display: flex;
			flex-direction: column;
			min-height: 0; // For scrolling
			overflow-y: auto;

			.interaction-panel__placeholder {
				flex-grow: 1;
				display: flex;
				align-items: center;
				justify-content: center;
				color: $color-text-disabled;
				font-style: italic;
			}
		}
	}
</style>
