<script lang="ts">
	import { page } from '$app/stores';
	import SessionActivityItem from '$components/activities/SessionActivityItem.svelte';
	import { getActivitiesFromSession } from '$lib/activities/activity_utils';
	import type { Activity } from '$lib/activities/types';

	let { session_id } = $page.params;

	let activities = $state<Activity[]>(getActivitiesFromSession(session_id));
</script>

<svelte:head>
	<title>Activities for Session {session_id} - EngaGenie</title>
</svelte:head>

<div class="session-activities-page">
	<header class="session-activities-page__header">
		<h1 class="session-activities-page__title">Session Activities</h1>
	</header>

	<div class="session-activities-page__list">
		{#each activities as activity}
			<SessionActivityItem {activity} />
		{:else}
			<p class="session-activities-page__no-results">No activities added to this session yet.</p>
		{/each}
	</div>
</div>

<style lang="scss">
	.session-activities-page {
		&__header {
			display: flex;
			justify-content: space-between;
			align-items: center;
			margin-bottom: $spacing-xl;
			flex-wrap: wrap;
			gap: $spacing-md;
		}

		&__title {
			font-size: $font-size-2xl;
			font-weight: $font-weight-bold;
			margin: 0;
		}

		&__list {
			display: flex;
			flex-direction: column;
			gap: $spacing-lg;
		}

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
