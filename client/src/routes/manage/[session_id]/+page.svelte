<script lang="ts">
	/**
	 * @file Page for presenting a live session, showing the current activity,
	 * upcoming activities, and completed activities. Manager redirects to this page
	 * to handle the session flow.
	 */

	import { onMount, onDestroy } from 'svelte';
	import { page } from '$app/stores';
	import { getActivityResults } from '$lib/activities/activity_utils';
	import type { ActivityResult, Activity } from '$lib/activities/types';
	import Button from '$components/elements/typography/Button.svelte';
	import ActivityItemDisplay from '$components/activities/display/ActivityItemDisplay.svelte';
	import SessionAnalyticsItem from '$components/analytics/SessionAnalyticsItem.svelte';
	import {
		getSessionById,
		nextActivity,
		getActivitiesFromSession
	} from '$lib/sessions/session_utils';
	import type { Session } from '$lib/sessions/types';
	import { get } from 'svelte/store';
	import { toast } from '$lib/stores/toast_store';

	const session_id = $page.params.session_id;

	// state management
	let session = $state<Session | null>(null);
	let activities = $state<Activity[]>([]);
	let currentIndex = $state(0);
	let isLoading = $state(true);
	let isAdvancing = $state(false);
	let sessionFinished = $state(false);

	// for displau
	let currentActivityResult = $state<ActivityResult | null>(null);
	let currentActivity = $state<Activity | null>(null);
	let upcomingActivities = $state<Activity[]>([]);
	let completedActivities = $state<Activity[]>([]);
	let advanceButtonText = $state('Loading...');

	// polling
	let resultsPollTimeoutId: any;
	const RESULTS_POLLING_INTERVAL = 3000; // we can tinker with this - perhaps lower it?
	const JITTER_AMOUNT = 1000;

	onMount(async () => {
		await loadInitialData();
	});

	async function loadInitialData() {
		isLoading = true;
		try {
			session = await getSessionById(session_id);
			if (session) {
				activities = await getActivitiesFromSession(session_id);
				currentIndex = session.currentActivity ?? 0;
			}
		} catch (error) {
			console.error('Failed to load initial data:', error);
		} finally {
			isLoading = false;
		}
	}

	onDestroy(() => {
		clearTimeout(resultsPollTimeoutId);
	});

	$effect(() => {
		currentActivity = activities[currentIndex];

		//update the left panel
		upcomingActivities = activities.slice(currentIndex + 1);
		completedActivities = activities.slice(0, currentIndex);

		if (currentIndex >= activities.length - 1 && !isLoading) {
			advanceButtonText = 'All Activities Complete';
			sessionFinished = true;
		} else {
			advanceButtonText = 'Next Activity';
		}

		clearTimeout(resultsPollTimeoutId);
		pollResultsWithJitter();
	});

	/**
	 * Polls the results of the current activity with a random jitter
	 */
	async function pollResultsWithJitter(): Promise<void> {
		if (!currentActivity) return;

		currentActivityResult = await getActivityResults(session_id, currentActivity.id);

		const jitter = Math.random() * JITTER_AMOUNT;
		resultsPollTimeoutId = setTimeout(pollResultsWithJitter, RESULTS_POLLING_INTERVAL + jitter);
	}

	/**
	 * Advances to the next activity in the session.
	 */
	async function advanceToNext(): Promise<void> {
		if (isAdvancing || currentIndex >= activities.length - 1) return;
		isAdvancing = true;

		try {
			const updatedSession = await nextActivity(session_id);
			if (updatedSession) {
				session = updatedSession;
				currentIndex = updatedSession.currentActivity ?? currentIndex;
			}
		} catch (error) {
			console.error('Error going to next session', error);
			toast.show('Failed to advance to next activity. Please try again.', 'error');
		} finally {
			isAdvancing = false;
		}
	}
</script>

<svelte:head>
	<title>EngaGenie | Manage Session {session?.title}</title>
</svelte:head>

{#if isLoading}
	<div class="placeholder-container">
		<p>Loading session details...</p>
	</div>
{:else if session?.mode === 'student-paced'}
	<div class="placeholder-container">
		<h2 class="placeholder-container__title">Student-Paced Session</h2>
		<p class="placeholder-container__message">
			This session is in student-paced mode, so there is nothing to manage here. Participants can
			navigate through activities on their own.
		</p>
	</div>
{:else if activities.length === 0}
	<div class="placeholder-container">
		<h2 class="placeholder-container__title">No Activities to Manage</h2>
		<p class="placeholder-container__message">
			There are 0 activities in this session. Add activities to the session's template to begin.
		</p>
	</div>
{:else}
	<div class="live-session-page">
		<header class="live-session-page__top-bar">
			<Button variant="primary" onclick={advanceToNext} disabled={isAdvancing || sessionFinished}>
				{advanceButtonText}
			</Button>
		</header>

		<div class="live-session-page__main-grid">
			<!-- Left Column: Activity Queue -->
			<aside class="live-session-page__queue-panel">
				<!-- Current Activity in the Queue -->
				{#if currentActivity}
					<div class="queue-section">
						<h3 class="queue-section__title">Current Activity</h3>
						<div class="queue-item queue-item--active">
							<span class="queue-item__index">{currentIndex + 1}</span>
							<span class="queue-item__title">{currentActivity.title}</span>
						</div>
					</div>
				{/if}

				<!-- Upcoming Activities -->
				{#if upcomingActivities.length > 0}
					<div class="queue-section">
						<h3 class="queue-section__title">Upcoming</h3>
						<div class="queue-list">
							{#each upcomingActivities as activityItem, i}
								<div class="queue-item queue-item--pending">
									<span class="queue-item__index">{currentIndex + i + 2}</span>
									<span class="queue-item__title">{activityItem.title}</span>
								</div>
							{/each}
						</div>
					</div>
				{/if}

				{#if completedActivities.length > 0}
					<div class="queue-section">
						<h3 class="queue-section__title">Completed</h3>
						<div class="queue-list">
							{#each completedActivities as activityItem}
								<div class="queue-item queue-item--closed">
									<span class="queue-item__title">{activityItem.title}</span>
								</div>
							{/each}
						</div>
					</div>
				{/if}
			</aside>

			<main class="live-session-page__main-panel">
				<!-- Current Activity Display -->
				<section class="content-card">
					<h2 class="content-card__title">Current Activity</h2>
					<div class="content-card__body">
						{#if currentActivity}
							<ActivityItemDisplay activity={currentActivity} />
						{:else}
							<div class="placeholder">
								<p>Click "Start Session" to begin</p>
							</div>
						{/if}
					</div>
				</section>

				<!-- Analytics Display -->
				<section class="content-card">
					<h2 class="content-card__title">Live Answers</h2>
					<div class="content-card__body">
						{#if currentActivityResult}
							<SessionAnalyticsItem activityResult={currentActivityResult} />
						{:else}
							<div class="placeholder">
								<p>Results will appear here when an activity is active</p>
							</div>
						{/if}
					</div>
				</section>
			</main>
		</div>
	</div>
{/if}

<style lang="scss">
	.live-session-page {
		display: flex;
		flex-direction: column;
		min-height: 100vh;
		background-color: $color-background;
		font-family: $font-family-primary;

		&__top-bar {
			display: flex;
			justify-content: space-between;
			align-items: center;
			padding: $spacing-md $spacing-lg;
			background-color: $color-surface;
			border-bottom: $border-width-thin solid $color-border-light;
			flex-shrink: 0;
			position: sticky;
			top: 0;
			z-index: 10;
		}

		&__main-grid {
			display: grid;
			grid-template-columns: 1fr;
			gap: $spacing-lg;
			padding: $spacing-lg;
			flex-grow: 1;

			@media (min-width: $breakpoint-lg) {
				grid-template-columns: 350px 1fr;
			}
		}

		&__queue-panel {
			background-color: $color-surface;
			border-radius: $border-radius-md;
			padding: $spacing-md;
			border: $border-width-thin solid $color-border-light;
			display: flex;
			flex-direction: column;
			gap: $spacing-lg;
		}

		&__main-panel {
			display: flex;
			flex-direction: column;
			gap: $spacing-lg;
		}
	}

	.queue-section {
		&__title {
			font-size: $font-size-sm;
			font-weight: $font-weight-semibold;
			color: $color-text-secondary;
			margin: 0 0 $spacing-md 0;
			text-transform: uppercase;
			letter-spacing: 0.5px;
			padding-bottom: $spacing-sm;
			border-bottom: $border-width-thin solid $color-border-light;
		}
	}

	.queue-list {
		display: flex;
		flex-direction: column;
		gap: $spacing-sm;
	}

	.queue-item {
		display: flex;
		align-items: center;
		gap: $spacing-md;
		padding: $spacing-sm;
		border-radius: $border-radius-md;
		border: $border-width-thin solid transparent;

		&--pending {
			background-color: $color-surface-alt;
			border-color: $color-border-light;
			color: $color-text-secondary;
		}
		&--active {
			background-color: #e8e7f5;
			border-color: $color-primary-light;
			color: $color-primary-dark;
			font-weight: $font-weight-semibold;
		}
		&--closed {
			background-color: $color-surface-alt;
			color: $color-text-disabled;
			.queue-item__title {
				text-decoration: line-through;
			}
		}

		&__index {
			font-size: $font-size-sm;
			font-weight: $font-weight-semibold;
			flex-shrink: 0;
		}

		&__title {
			white-space: nowrap;
			overflow: hidden;
			text-overflow: ellipsis;
		}
	}

	.content-card {
		background-color: $color-surface;
		border: $border-width-thin solid $color-border-light;
		border-radius: $border-radius-md;
		display: flex;
		flex-direction: column;
		flex-shrink: 0;

		&__title {
			padding: $spacing-md $spacing-lg;
			margin: 0;
			font-size: $font-size-md;
			font-weight: $font-weight-semibold;
			border-bottom: $border-width-thin solid $color-border-light;
			flex-shrink: 0;
		}

		&__body {
			padding: $spacing-lg;
		}
	}

	.placeholder {
		display: flex;
		align-items: center;
		justify-content: center;
		min-height: 150px;
		text-align: center;
		color: $color-text-secondary;
		font-size: $font-size-lg;
		background-color: $color-surface-alt;
		border-radius: $border-radius-md;
	}

	.placeholder-container {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		text-align: center;
		min-height: 80vh;
		padding: $spacing-xl;
		background-color: $color-surface;
		border-radius: $border-radius-lg;

		&__title {
			font-size: $font-size-2xl;
			font-weight: $font-weight-semibold;
			color: $color-text-primary;
			margin-bottom: $spacing-md;
		}

		&__message {
			font-size: $font-size-lg;
			color: $color-text-secondary;
			max-width: 600px;
		}
	}
</style>
