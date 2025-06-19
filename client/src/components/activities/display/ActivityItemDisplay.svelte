<script lang="ts">
	/**
	 * @file Component for displaying a single activity item in the session view.
	 * Based on activity type, it renders the appropriate display component.
	 */

	import { fade } from 'svelte/transition';
	import type {
		MultipleChoiceDefinition,
		PollDefinition,
		ScaleRatingDefinition
	} from '$lib/activities/definition_types';
	import OpenEndedDisplay from '$components/activities/display/OpenEndedDisplay.svelte';
	import MultipleChoiceDisplay from '$components/activities/display/MultipleChoiceDisplay.svelte';
	import PollDisplay from '$components/activities/display/PollDisplay.svelte';
	import ScaleRatingDisplay from '$components/activities/display/ScaleRatingDisplay.svelte';
	import RawJsonDisplay from '$components/activities/display/RawJsonDisplay.svelte';

	import type { Activity } from '$lib/activities/types';

	let {
		activity
	}: {
		activity: Activity;
	} = $props();
</script>

<div class="session-activity-item" transition:fade|local>
	<div class="session-activity-item__header">
		<span class="session-activity-item__type">{activity.type.replace('_', ' ')}</span>
		<h3 class="session-activity-item__title">{activity.title}</h3>
	</div>

	<!-- Render the appropriate display component based on the activity type -->
	<div class="session-activity-item__body">
		{#if activity.type === 'multiple_choice'}
			<MultipleChoiceDisplay definition={activity.definition as MultipleChoiceDefinition} />
		{:else if activity.type === 'poll'}
			<PollDisplay definition={activity.definition as PollDefinition} />
		{:else if activity.type === 'scale_rating'}
			<ScaleRatingDisplay definition={activity.definition as ScaleRatingDefinition} />
		{:else if activity.type === 'open_ended'}
			<OpenEndedDisplay />
		{:else}
			<RawJsonDisplay definition={activity.definition} />
		{/if}
	</div>
</div>

<style lang="scss">
	.session-activity-item {
		background-color: $color-surface;
		border-radius: $border-radius-md;
		border: 1px solid $color-border-light;
		box-shadow: $box-shadow-sm;
		padding: $spacing-md;
		display: flex;
		flex-direction: column;
		gap: $spacing-sm;

		&__header {
			display: flex;
			align-items: baseline;
			gap: $spacing-sm;
			padding-bottom: $spacing-sm;
			border-bottom: 1px solid $color-border-light;
		}

		&__type {
			background-color: $color-surface-alt;
			color: $color-text-secondary;
			padding: $spacing-xs * 0.5 $spacing-sm;
			border-radius: $border-radius-sm;
			font-size: $font-size-xs;
			font-weight: $font-weight-medium;
			text-transform: uppercase;
			white-space: nowrap;
			flex-shrink: 0;
		}

		&__title {
			font-size: $font-size-md;
			font-weight: $font-weight-medium;
			color: $color-text-primary;
			margin: 0;
			flex-grow: 1;
		}
	}
</style>
