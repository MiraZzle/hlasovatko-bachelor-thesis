<script lang="ts">
	/**
	 * @file Component for displaying analytics results for an activity.
	 */
	import type {
		MultipleChoiceDefinition,
		PollDefinition,
		ScaleRatingDefinition
	} from '$lib/activities/definition_types';
	import type { ActivityResult } from '$lib/activities/types';
	import {
		isChoiceResult,
		isScaleRatingResult,
		isOpenEndedResult
	} from '$lib/analytics/result_utils';
	import ChoiceResultsDisplay from '$components/analytics/ChoiceResultsDisplay.svelte';
	import ScaleRatingResultsDisplay from '$components/analytics/ScaleRatingResultsDisplay.svelte';
	import OpenEndedResultsDisplay from '$components/analytics/OpenEndedResultsDisplay.svelte';

	let { activityResult }: { activityResult: ActivityResult } = $props();
	const activityRef = $derived(activityResult.activityRef);
	const results = $derived(activityResult.results);
</script>

<div class="analytics-item">
	<div class="analytics-item__header">
		<span class="analytics-item__type">{activityRef.type.replace('_', ' ')}</span>
		<h3 class="analytics-item__title">{activityRef.title}</h3>
	</div>

	<div class="analytics-item__body">
		{#if (activityRef.type === 'multiple_choice' || activityRef.type === 'poll') && isChoiceResult(results)}
			<ChoiceResultsDisplay
				{results}
				definition={activityRef.definition as MultipleChoiceDefinition | PollDefinition}
			/>
		{:else if activityRef.type === 'scale_rating' && isScaleRatingResult(results)}
			<ScaleRatingResultsDisplay
				{results}
				definition={activityRef.definition as ScaleRatingDefinition}
			/>
		{:else if activityRef.type === 'open_ended' && isOpenEndedResult(results)}
			<OpenEndedResultsDisplay {results} />
		{:else}
			<p class="analytics-item__no-data">
				No results available or this activity type does not support analytics.
			</p>
		{/if}
	</div>
</div>

<style lang="scss">
	.analytics-item {
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
		&__no-data {
			font-style: italic;
			color: $color-text-secondary;
			font-size: $font-size-sm;
			text-align: center;
			padding: $spacing-lg 0;
		}
	}
</style>
