<script lang="ts">
	/**
	 * @file Reusable component for displaying the results of a scale rating activity.
	 * Renders a bar chart with ratings and their counts.
	 */
	import type { ScaleRatingDefinition } from '$lib/activities/definition_types';
	import type { ScaleRatingActivityResult } from '$lib/analytics/result_utils';

	let {
		results,
		definition = null
	}: {
		results: ScaleRatingActivityResult;
		definition?: ScaleRatingDefinition | null;
	} = $props();

	// Track hovered item state
	let hoveredRating = $state<number | null>(null);
	let hoveredCount = $state<number | null>(null);

	// Scale points and calculations
	let totalVotes = $derived(results.reduce((sum, item) => sum + item.count, 0));
	let maxCount = $derived(results.length > 0 ? Math.max(...results.map((r) => r.count)) : 0);

	/**
	 * Calculate the total number of votes across all ratings.
	 * @param count - The count of votes for the current rating.
	 * @returns The percentage of votes for the current rating.
	 */
	function getPercentage(count: number): number {
		return totalVotes > 0 ? Math.round((count / totalVotes) * 100) : 0;
	}

	/**
	 * Calculate the height of the bar as a percentage of the maximum count.
	 * Returns 0 if maxCount is 0 to avoid division by zero.
	 * @param count - The count of votes for the current rating.
	 * @return The height percentage of the bar.
	 */
	function getBarHeightPercentage(count: number): number {
		return maxCount > 0 ? Math.round((count / maxCount) * 100) : 0;
	}

	/**
	 * Generate scale points based on the definition and results.
	 * Returns an array of objects containing rating, count, percentage, and bar height.
	 * @returns An array of scale points.
	 */
	function getScalePoints(): {
		rating: number;
		count: number;
		percentage: number;
		barHeight: number;
	}[] {
		if (!definition || totalVotes === 0) return [];
		const points = [];
		for (let i = definition.min; i <= definition.max; i++) {
			const resultItem = results.find((r) => r.rating === i);
			const count = resultItem?.count ?? 0;
			points.push({
				rating: i,
				count: count,
				percentage: getPercentage(count),
				barHeight: getBarHeightPercentage(count)
			});
		}
		return points;
	}

	let scalePoints = $derived(getScalePoints());

	// Handlers for mouse enter and leave events to show hover info
	function handleMouseEnter(rating: number, count: number): void {
		hoveredRating = rating;
		hoveredCount = count;
	}

	// Function to reset hover state when mouse leaves the bar
	function handleMouseLeave(): void {
		hoveredRating = null;
		hoveredCount = null;
	}
</script>

<div class="scale-results-display">
	<div class="scale-results-display__hover-info">
		{#if hoveredRating !== null}
			Rating {hoveredRating}: <strong>{hoveredCount}</strong>
			({totalVotes > 0 ? Math.round(((hoveredCount ?? 0) / totalVotes) * 100) : 0}%)
		{:else}
			&nbsp;
		{/if}
	</div>

	{#if totalVotes > 0 && scalePoints.length > 0}
		<div class="scale-results-display__chart">
			{#each scalePoints as point (point.rating)}
				<div
					tabindex="0"
					role="button"
					aria-roledescription="Rating bar"
					aria-label={`Rating ${point.rating}: ${point.count} (${point.percentage}%)`}
					class="scale-results-display__bar-wrapper"
					title={`Rating ${point.rating}: ${point.count} (${point.percentage}%)`}
					onmouseenter={() => handleMouseEnter(point.rating, point.count)}
					onmouseleave={handleMouseLeave}
					class:scale-results-display__bar-wrapper--hover={hoveredRating === point.rating}
				>
					<div
						class="scale-results-display__bar"
						style:height="{point.barHeight}%"
						class:scale-results-display__bar--hover={hoveredRating === point.rating}
					></div>
					<span class="scale-results-display__rating-label">{point.rating}</span>
				</div>
			{/each}
		</div>
		<div class="scale-results-display__labels">
			<span class="scale-results-display__min-label"
				>{definition?.minLabel ?? definition?.min ?? ''}</span
			>
			<span class="scale-results-display__max-label"
				>{definition?.maxLabel ?? definition?.max ?? ''}</span
			>
		</div>
	{:else}
		<p class="scale-results-display__no-results">No responses received yet.</p>
	{/if}
	<p class="scale-results-display__total">Total Responses: {totalVotes}</p>
</div>

<style lang="scss">
	.scale-results-display {
		&__hover-info {
			min-height: 1.5em;
			text-align: center;
			font-size: $font-size-sm;
			color: $color-text-secondary;
			margin-bottom: $spacing-sm;
			strong {
				color: $color-text-primary;
				font-weight: $font-weight-semibold;
			}
		}

		&__chart {
			display: flex;
			align-items: flex-end;
			gap: $spacing-xs;
			height: 150px;
			border-bottom: 1px solid $color-border;
			padding-bottom: $spacing-sm;
			margin-bottom: $spacing-xs;
		}
		&__bar-wrapper {
			flex: 1;
			display: flex;
			flex-direction: column;
			align-items: center;
			justify-content: flex-end;
			height: 100%;
			cursor: default;
			transition: background-color $transition-duration-fast;
			border-radius: $border-radius-sm;

			&--hover {
				background-color: rgba($color-secondary, 0.1);
			}
		}
		&__bar {
			width: 70%;
			background-color: $color-secondary-light;
			border-radius: $border-radius-sm $border-radius-sm 0 0;
			transition:
				height 0.3s ease-out,
				background-color $transition-duration-fast;

			&--hover {
				background-color: $color-secondary;
			}
		}
		&__rating-label {
			font-size: $font-size-xs;
			color: $color-text-secondary;
			margin-top: $spacing-xs;
		}
		&__labels {
			display: flex;
			justify-content: space-between;
			font-size: $font-size-xs;
			color: $color-text-secondary;
			padding: 0 5%;
		}
		&__no-results {
			color: $color-text-secondary;
			font-style: italic;
		}
		&__total {
			font-size: $font-size-xs;
			color: $color-text-secondary;
			margin-top: $spacing-sm;
			text-align: right;
		}
	}
</style>
