<script lang="ts">
	import { page } from '$app/stores';
	import SessionAnalyticsItem from '$components/analytics/SessionAnalyticsItem.svelte';
	import type { SessionActivity } from '$lib/activity_types';
	import { getActivityResultsForSession } from '$lib/analytics/analytics_utils';

	let { session_id } = $page.params;
	let activitiesWithResults = getActivityResultsForSession(session_id);
</script>

<svelte:head>
	<title>EngaGenie | Session {session_id} - Analytics</title>
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
