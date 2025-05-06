<script lang="ts">
	import { page } from '$app/stores';
	import Button from '$components/elements/typography/Button.svelte';
	import LiveActivityQueueItem from '$components/activities/LiveActivityQueueItem.svelte';
	import LiveResultsDisplay from '$components/activities/LiveResultsDisplay.svelte';
	// Import display components for the teacher's view of the current activity
	import MultipleChoice from '$components/activities/MultipleChoice.svelte';
	import Poll from '$components/activities/Poll.svelte';
	import ScaleRating from '$components/activities/ScaleRating.svelte';
	import OpenEnded from '$components/activities/OpenEnded.svelte';
	import RawJson from '$components/activities/RawJson.svelte';
	import { onMount } from 'svelte';

	import type { SessionActivity, KnownActivityDefinition } from '$lib/types';
	import {
		getKnownDefinition,
		isMultipleChoice,
		isPoll,
		isScaleRating,
		isOpenEnded
	} from '$lib/types';

	// --- Get Session ID ---
	let { session_id } = $page.params;

	// --- State ---
	// In a real app, this would be fetched and updated via WebSockets
	let participantCount = $state(Math.floor(Math.random() * 50)); // Dummy
	let sessionCode = $state('ABCXYZ'); // Dummy
	let sessionJoinLink = $derived(`${$page.url.origin}/join/${sessionCode}`); // Example

	let allActivities = $state<SessionActivity[]>([
		// Dummy data from previous activity list example, ensure definitions have 'type'
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
			order: 1
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
			order: 2
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
			order: 3
		},
		{
			id: 'sact4',
			type: 'OpenEnded',
			title: 'Any remaining questions?',
			definition: { type: 'OpenEnded' },
			status: 'Pending',
			order: 4
		}
	]);

	let currentActivityId = $state<string | null>(null);
	let showResultsToParticipants = $state(false);

	// --- Derived State ---
	// --- Derived State ---
	let currentActivity = $derived(allActivities.find((act) => act.id === currentActivityId) ?? null);

	function getCurrentActivityParsedDef() {
		const activity = currentActivity; // Get current value of the signal
		if (!activity?.definition) return null;

		let def = activity.definition;
		if (typeof def === 'string') {
			try {
				def = JSON.parse(def);
			} catch (e) {
				console.error(`Failed to parse definition string for activity ${activity.id}:`, e);
				return { error: 'Invalid JSON in definition string', original: activity.definition };
			}
		}

		if (typeof def !== 'object' || def === null) {
			console.warn(
				`Activity ${activity.id} definition is not a valid object after potential parsing.`
			);
			return { error: 'Definition is not a valid object', original: activity.definition };
		}

		// Ensure the 'type' property is correctly set in the definition object
		// This is crucial for the type guards to work.
		if (!('type' in def) || def.type !== activity.type) {
			// console.warn(`Injecting/correcting type '${activity.type}' into definition for activity ${activity.id}`);
			return { ...def, type: activity.type }; // Return a new object with the correct type
		}
		return def;
	}

	// currentActivityParsedDef will hold the processed definition object or null/string on error
	let currentActivityParsedDef = $derived(getCurrentActivityParsedDef());

	// --- Handlers ---
	function activateActivity(activityId: string) {
		console.log(`Teacher activating activity: ${activityId}`);
		// Set previous current to 'Closed' if it was active
		if (currentActivityId) {
			allActivities = allActivities.map((a) =>
				a.id === currentActivityId ? { ...a, status: 'Closed' } : a
			);
		}
		currentActivityId = activityId;
		showResultsToParticipants = false; // Reset visibility
		// Update status in the list
		allActivities = allActivities.map((a) =>
			a.id === activityId ? { ...a, status: 'Active' } : a
		);
		// TODO: Send WebSocket message to clients to show this activity
	}

	function stopCurrentActivity() {
		if (currentActivityId) {
			console.log(`Teacher stopping activity: ${currentActivityId}`);
			allActivities = allActivities.map((a) =>
				a.id === currentActivityId ? { ...a, status: 'Closed' } : a
			);
			// currentActivityId = null; // Or move to next, or wait for teacher to pick
			// TODO: Send WebSocket message
		}
	}

	function toggleResultsVisibility() {
		showResultsToParticipants = !showResultsToParticipants;
		console.log(`Results visibility for participants: ${showResultsToParticipants}`);
		// TODO: Send WebSocket message
	}

	function advanceToNext() {
		if (!currentActivity) {
			// If no activity is current, start the first pending one
			const firstPending = allActivities.find((a) => a.status === 'Pending');
			if (firstPending) activateActivity(firstPending.id);
			return;
		}
		const currentIndex = allActivities.findIndex((act) => act.id === currentActivityId);
		// Find next pending activity
		let nextActivity = null;
		for (let i = currentIndex + 1; i < allActivities.length; i++) {
			if (allActivities[i].status === 'Pending') {
				nextActivity = allActivities[i];
				break;
			}
		}
		if (nextActivity) {
			activateActivity(nextActivity.id);
		} else {
			console.log('No more pending activities.');
			// Maybe set currentActivityId to null if all are done
			if (currentActivityId) {
				allActivities = allActivities.map((a) =>
					a.id === currentActivityId ? { ...a, status: 'Closed' } : a
				);
			}
			currentActivityId = null;
			alert('End of activity queue!');
		}
	}

	function endSession() {
		console.log('Ending session:', session_id);
		alert('Session Ended (Placeholder)');
		// TODO: API call, WebSocket messages
		// goto('/sessions');
	}

	// Mock participant count updates
	onMount(() => {
		const interval = setInterval(() => {
			participantCount = participantCount + Math.floor(Math.random() * 3) - 1;
			if (participantCount < 0) participantCount = 0;
		}, 5000);
		return () => clearInterval(interval);
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
		<Button onclick={endSession}>End Session</Button>
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

		<section class="live-session-page__current-activity-panel">
			<h2 class="live-session-page__panel-title">Current Activity</h2>
			{#if currentActivity && currentActivityParsedDef}
				<div class="current-activity-card">
					<div class="current-activity-card__header">
						<span class="current-activity-card__type">{currentActivity.type}</span>
						<h3 class="current-activity-card__title">{currentActivity.title}</h3>
					</div>
					<div class="current-activity-card__body">
						{#if isMultipleChoice(currentActivityParsedDef)}
							<MultipleChoice definition={currentActivityParsedDef} />
						{:else if isPoll(currentActivityParsedDef)}
							<Poll definition={currentActivityParsedDef} />
						{:else if isScaleRating(currentActivityParsedDef)}
							<ScaleRating definition={currentActivityParsedDef} />
						{:else if isOpenEnded(currentActivityParsedDef)}
							<OpenEnded />
						{:else}
							<RawJson definition={currentActivityParsedDef} />
						{/if}
					</div>
					<div class="current-activity-card__controls">
						{#if currentActivity.status === 'Active'}
							<Button onclick={stopCurrentActivity}>Stop Activity</Button>
						{:else if currentActivity.status === 'Pending' || currentActivity.status === 'Closed'}
							<Button onclick={() => activateActivity(currentActivity.id)}
								>Start This Activity</Button
							>
						{/if}
						<Button variant="outline" onclick={toggleResultsVisibility}>
							{showResultsToParticipants ? 'Hide Results' : 'Show Results'}
						</Button>
						<Button variant="primary" onclick={advanceToNext}>Next Activity &rarr;</Button>
					</div>
				</div>
			{:else}
				<div class="current-activity-panel__placeholder">
					<p>No activity selected or active.</p>
					<Button variant="primary" onclick={advanceToNext}>Start First Activity</Button>
				</div>
			{/if}
		</section>

		<aside class="live-session-page__results-panel">
			<h2 class="live-session-page__panel-title">Live Results</h2>
			<LiveResultsDisplay activity={currentActivity} {showResultsToParticipants} />
		</aside>
	</div>
</div>

<style lang="scss">
	@import '../../../styles/variables.scss'; // Adjust path

	// Block: live-session-page
	.live-session-page {
		display: flex;
		flex-direction: column;
		height: 100%; // Fill the content slot of the parent layout

		// Element: Top Control Bar
		&__top-bar {
			display: flex;
			justify-content: space-between;
			align-items: center;
			padding: $spacing-sm $spacing-md;
			background-color: darken($color-surface, 3%); // Slightly different from main content
			border-bottom: 1px solid $color-border;
			flex-shrink: 0;
		}

		// Element: Session Info in Top Bar
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
			// Adjust columns for responsiveness: 1 for mobile, 3 for desktop
			grid-template-columns: 1fr; // Mobile default
			grid-template-rows: auto auto auto; // Stack panels on mobile
			gap: $spacing-lg;
			padding: $spacing-md; // Padding for the grid itself
			flex-grow: 1;
			overflow: auto; // Allow grid to scroll if content overflows

			@media (min-width: $breakpoint-lg) {
				// Example: 1fr (queue) 2fr (main) 1.5fr (results)
				grid-template-columns: 0.75fr 1.5fr 1fr;
				grid-template-rows: 1fr; // Single row for desktop
				padding: $spacing-lg;
			}
		}

		// Element: Panel Title (common for queue, current, results)
		&__panel-title {
			font-size: $font-size-md;
			font-weight: $font-weight-semibold;
			color: $color-text-secondary;
			margin: 0 0 $spacing-sm 0;
			text-transform: uppercase;
			letter-spacing: 0.5px;
		}

		// Element: Activity Queue Panel
		&__activity-queue {
			background-color: $color-surface-alt;
			border-radius: $border-radius-md;
			padding: $spacing-md;
			display: flex;
			flex-direction: column;
			overflow-y: auto; // Scrollable queue list
			gap: $spacing-sm;
		}
		&__queue-list {
			display: flex;
			flex-direction: column;
			gap: $spacing-sm;
		}

		// Element: Current Activity Panel
		&__current-activity-panel {
			display: flex;
			flex-direction: column;
			.current-activity-card {
				// BEM block for the card itself
				background-color: $color-surface;
				border-radius: $border-radius-lg;
				padding: $spacing-lg;
				border: 1px solid $color-border;
				box-shadow: $box-shadow-md;
				display: flex;
				flex-direction: column;
				gap: $spacing-md;
				flex-grow: 1; // Take available space

				&__header {
					display: flex;
					align-items: baseline;
					gap: $spacing-sm;
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
				}
				&__controls {
					display: flex;
					gap: $spacing-sm;
					flex-wrap: wrap;
					justify-content: flex-end;
					border-top: 1px solid $color-border-light;
					padding-top: $spacing-md;
					margin-top: $spacing-md;
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

		// Element: Results Panel
		&__results-panel {
			display: flex;
			flex-direction: column;
			// Styles for the results panel container
			// LiveResultsDisplay component will have its own internal styles
		}
	}
</style>
