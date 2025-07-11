<script lang="ts">
	/**
	 * @file Session Analytics Page
	 * This page displays analytics for a specific session, including activity results.
	 */
	import { page } from '$app/stores';
	import SessionAnalyticsItem from '$components/analytics/SessionAnalyticsItem.svelte';
	import { getActivityResultsForSession } from '$lib/analytics/analytics_utils';
	import type { ActivityResult, StaticActivityType } from '$lib/activities/types';
	import { onMount } from 'svelte';

	let sessionId = $page.params.session_id;
	let activitiesWithResults: ActivityResult[] = $state<ActivityResult[]>([]);

	onMount(async () => {
		try {
			activitiesWithResults = await getActivityResultsForSession(sessionId);
			console.log('Loaded activity results:', activitiesWithResults);
		} catch (error) {
			console.error('Failed to load activity results:', error);
		}
	});
</script>

<svelte:head>
	<title>EngaGenie | Session {sessionId} - Analytics</title>
</svelte:head>

<div class="session-analytics-page">
	<header class="session-analytics-page__header">
		<h1 class="session-analytics-page__title">Session Analytics</h1>
	</header>

	<div class="session-analytics-page__list">
		{#each activitiesWithResults as activityResult}
			<SessionAnalyticsItem {activityResult} />
		{:else}
			<p class="session-analytics-page__no-results">
				No activities have been run in this session yet.
			</p>
		{/each}
	</div>
</div>

<style lang="scss">
	.session-analytics-page {
		&__header {
			margin-bottom: $spacing-xl;
		}
		&__title {
			font-size: $font-size-2xl;
			font-weight: $font-weight-bold;
			margin-bottom: $spacing-md;
		}
		&__list {
			display: flex;
			flex-direction: column;
			gap: $spacing-lg;
		}
	}
</style>
