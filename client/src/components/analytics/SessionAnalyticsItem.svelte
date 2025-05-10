<script lang="ts">
	import type { SessionActivity, KnownActivityDefinition } from '$lib/activity_types';
	import {
		getKnownDefinition,
		isMultipleChoice,
		isPoll,
		isScaleRating,
		isOpenEnded,
		isChoiceResult,
		isScaleRatingResult,
		isOpenEndedResult
	} from '$lib/activity_types'; // Import result type guards too

	// Import result display components
	import ChoiceResultsDisplay from '$components/analytics/ChoiceResultsDisplay.svelte';
	import ScaleRatingResultsDisplay from '$components/analytics/ScaleRatingResultsDisplay.svelte';
	import OpenEndedResultsDisplay from '$components/analytics/OpenEndedResultsDisplay.svelte';

	type Props = {
		activity: SessionActivity;
	};
	let { activity }: Props = $props();

	// Get typed definition for potentially passing to result components (e.g., for labels/correctness)
	let knownDefinition = $derived(getKnownDefinition(activity));
</script>

<div class="session-analytics-item">
	<div class="session-analytics-item__header">
		<span class="session-analytics-item__type">{activity.type}</span>
		<h3 class="session-analytics-item__title">{activity.title}</h3>
		{#if activity.responseCount !== undefined}
			<span class="session-analytics-item__response-count">
				{activity.responseCount} / {activity.participantCount ?? '?'} Responses
			</span>
		{/if}
	</div>
	<div class="session-analytics-item__body">
		{#if (activity.type === 'MultipleChoice' || activity.type === 'Poll') && isChoiceResult(activity.results)}
			<ChoiceResultsDisplay
				results={activity.results}
				definition={isMultipleChoice(knownDefinition) ? knownDefinition : null}
			/>
		{:else if activity.type === 'ScaleRating' && isScaleRatingResult(activity.results)}
			<ScaleRatingResultsDisplay
				results={activity.results}
				definition={isScaleRating(knownDefinition) ? knownDefinition : null}
			/>
		{:else if activity.type === 'OpenEnded' && isOpenEndedResult(activity.results)}
			<OpenEndedResultsDisplay results={activity.results} />
		{:else}
			<p class="session-analytics-item__no-data">
				No results available or activity type not supported for detailed analytics.
			</p>
		{/if}
	</div>
</div>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

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
		&__body {
			/* Container for results */
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
