<script lang="ts">
	import type { SessionActivity, KnownActivityDefinition } from '$lib/activity_types';
	import type { ActivityResult } from '$lib/activities/types';
	import {
		getKnownDefinition,
		isMultipleChoice,
		isScaleRating,
		isChoiceResult,
		isScaleRatingResult,
		isOpenEndedResult
	} from '$lib/activity_types';

	// Import result display components
	import ChoiceResultsDisplay from '$components/analytics/ChoiceResultsDisplay.svelte';
	import ScaleRatingResultsDisplay from '$components/analytics/ScaleRatingResultsDisplay.svelte';
	import OpenEndedResultsDisplay from '$components/analytics/OpenEndedResultsDisplay.svelte';

	let {
		activity
	}: {
		activity: ActivityResult;
	} = $props();

	// Get the known definition for the activity
	let knownDefinition = $derived(getKnownDefinition(activity.activityRef));
</script>

<div class="session-analytics-item">
	<div class="session-analytics-item__header">
		<span class="session-analytics-item__type">{activity.activityRef.type}</span>
		<h3 class="session-analytics-item__title">{activity.activityRef.title}</h3>
	</div>
	<div class="session-analytics-item__body">
		{#if (activity.activityRef.type === 'MultipleChoice' || activity.activityRef.type === 'Poll') && isChoiceResult(activity.results)}
			<ChoiceResultsDisplay
				results={activity.results}
				definition={isMultipleChoice(knownDefinition) ? knownDefinition : null}
			/>
		{:else if activity.activityRef.type === 'ScaleRating' && isScaleRatingResult(activity.results)}
			<ScaleRatingResultsDisplay
				results={activity.results}
				definition={isScaleRating(knownDefinition) ? knownDefinition : null}
			/>
		{:else if activity.activityRef.type === 'OpenEnded' && isOpenEndedResult(activity.results)}
			<OpenEndedResultsDisplay results={activity.results} />
		{:else}
			<p class="session-analytics-item__no-data">
				No results available or activity type not supported for detailed analytics.
			</p>
		{/if}
	</div>
</div>

<style lang="scss">
	.session-analytics-item {
		background-color: $color-surface;
		border-radius: $border-radius-lg;
		border: 1px solid $color-border-light;
		box-shadow: $box-shadow-sm;
		padding: $spacing-lg;
		display: flex;
		flex-direction: column;
		gap: $spacing-md;

		&__header {
			display: flex;
			align-items: baseline;
			gap: $spacing-sm;
			padding-bottom: $spacing-md;
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
			font-size: $font-size-lg;
			font-weight: $font-weight-semibold;
			color: $color-text-primary;
			margin: 0;
			flex-grow: 1;
		}
		&__response-count {
			font-size: $font-size-xs;
			color: $color-text-secondary;
			white-space: nowrap;
			flex-shrink: 0;
			margin-left: auto;
		}
		&__no-data {
			font-style: italic;
			color: $color-text-secondary;
			font-size: $font-size-sm;
			text-align: center;
			padding: $spacing-lg 0;
		}
	}
</style>
