<script lang="ts">
	/**
	 * @file Session Activities Page
	 * This page displays all activities associated with a specific session.
	 */
	import { page } from '$app/stores';
	import SessionActivityItem from '$components/activities/SessionActivityItem.svelte';
	import { getActivitiesFromSession } from '$lib/activities/activity_utils';
	import type { Activity } from '$lib/activities/types';
	import { onMount } from 'svelte';

	let session_id = $page.params.session_id;

	let activities = $state<Activity[]>([]);

	onMount(async () => {
		try {
			activities = await getActivitiesFromSession(session_id);
		} catch (error) {
			console.error('Failed to load activities:', error);
		}
	});
</script>

<svelte:head>
	<title>EngaGenie | Session {session_id} - Activities</title>
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
