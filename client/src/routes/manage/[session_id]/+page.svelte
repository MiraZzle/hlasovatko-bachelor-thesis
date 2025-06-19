<script lang="ts">
	/**
	 * @file Page for presenting a live session, showing the current activity,
	 * upcoming activities, and completed activities. Manager redirects to this page
	 * handle teh session flow.
	 */

	import { onMount } from 'svelte';
	import { page } from '$app/stores';
	import { getActivityResultsForSession } from '$lib/analytics/analytics_utils';
	import type { ActivityResult, Activity } from '$lib/activities/types';
	import Button from '$components/elements/typography/Button.svelte';
	import ActivityItemDisplay from '$components/activities/display/ActivityItemDisplay.svelte';
	import SessionAnalyticsItem from '$components/analytics/SessionAnalyticsItem.svelte';
	import { getSessionById } from '$lib/sessions/session_utils';

	const session_id = $page.params.session_id;
	const sessionInfo = $derived(getSessionById(session_id));
	let sessionCode = $derived(sessionInfo?.joinCode ?? 'N/A');
	let allActivityResults = $state<ActivityResult[]>([]);
	let currentIndex = $state(-1);

	/**
	 * Retrieves the current activity results based on the current index.
	 * If the index is out of bounds, it returns null.
	 */
	function getCurrentActivityResults(): ActivityResult | null {
		return allActivityResults[currentIndex] ?? null;
	}

	let currentActivityResult = $derived(getCurrentActivityResults());
	let currentActivity = $derived<Activity | null>(currentActivityResult?.activityRef ?? null);
	let upcomingActivities = $derived(
		currentIndex < allActivityResults.length - 1 ? allActivityResults.slice(currentIndex + 1) : []
	);
	let completedActivities = $derived(
		currentIndex > 0 ? allActivityResults.slice(0, currentIndex) : []
	);

	// Fetch the session data in result format on mount
	onMount(() => {
		allActivityResults = getActivityResultsForSession(session_id);
	});

	/**
	 * Determines the text for the advance button based on the current index
	 * and the total number of activities.
	 */
	function getAdvanceButtonText(): string {
		if (currentIndex === -1) return 'Start Session';
		if (currentIndex < allActivityResults.length - 1) return 'Next Activity →';
		return 'End Session';
	}

	let advanceButtonText = $derived(getAdvanceButtonText());

	/**
	 * Advances the session to the next activity in the queue. If it's the end of
	 * the queue, it handles the session end.
	 */
	function advanceToNext(): void {
		if (currentIndex < allActivityResults.length - 1) {
			currentIndex++;
		} else {
			alert('Session has ended!');
		}
	}
</script>

<svelte:head>
	<title>EngaGenie | Manage Session {sessionInfo?.title}</title>
</svelte:head>

<div class="live-session-page">
	<header class="live-session-page__top-bar">
		<div class="live-session-page__session-info">
			<span>Session Code: <strong>{sessionCode}</strong></span>
		</div>
		<Button variant="primary" onclick={advanceToNext}>
			{advanceButtonText}
		</Button>
	</header>

	<div class="live-session-page__main-grid">
		<!-- Left Column: Activity Queue -->
		<aside class="live-session-page__queue-panel">
			<!-- Current Activity in the Queue -->
			{#if currentActivity}
				<div class="queue-section">
					<h3 class="queue-section__title">Active Activity</h3>
					<div class="queue-item queue-item--active">
						<span class="queue-item__index">{currentIndex + 1}</span>
						<span class="queue-item__title">{currentActivity.title}</span>
					</div>
				</div>
			{/if}

			<!-- Upcoming Activities -->
			{#if upcomingActivities.length > 0}
				<div class="queue-section">
					<h3 class="queue-section__title">Up Next</h3>
					<div class="queue-list">
						{#each upcomingActivities as result, i (result.activityRef.id)}
							<div class="queue-item queue-item--pending">
								<span class="queue-item__index">{currentIndex + i + 2}</span>
								<span class="queue-item__title">{result.activityRef.title}</span>
							</div>
						{/each}
					</div>
				</div>
			{/if}

			<!-- Completed Activities -->
			{#if completedActivities.length > 0}
				<div class="queue-section">
					<h3 class="queue-section__title">Completed</h3>
					<div class="queue-list">
						{#each completedActivities as result (result.activityRef.id)}
							<div class="queue-item queue-item--closed">
								<span class="queue-item__icon">✓</span>
								<span class="queue-item__title">{result.activityRef.title}</span>
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
							<p>Click "Start Session" to begin.</p>
						</div>
					{/if}
				</div>
			</section>

			<!-- Analytics Display -->
			<section class="content-card">
				<h2 class="content-card__title">Live Analytics</h2>
				<div class="content-card__body">
					{#if currentActivityResult}
						<SessionAnalyticsItem activityResult={currentActivityResult} />
					{:else}
						<div class="placeholder">
							<p>Results will appear here when an activity is active.</p>
						</div>
					{/if}
				</div>
			</section>
		</main>
	</div>
</div>

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

		&__session-info {
			font-size: $font-size-md;
			color: $color-text-secondary;
			strong {
				color: $color-text-primary;
				font-weight: $font-weight-semibold;
			}
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
			background-color: lighten($color-info, 35%);
			border-color: $color-info;
			font-weight: $font-weight-semibold;
			color: darken($color-info, 20%);
		}
		&--closed {
			background-color: $color-surface-alt;
			color: $color-text-secondary;
			.queue-item__title {
				text-decoration: line-through;
			}
		}

		&__index {
			font-size: $font-size-sm;
			font-weight: $font-weight-semibold;
			flex-shrink: 0;
		}
		&__icon {
			font-weight: $font-weight-bold;
			color: $color-success;
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
</style>
