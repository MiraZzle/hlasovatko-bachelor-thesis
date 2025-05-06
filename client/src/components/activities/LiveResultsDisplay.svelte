<script lang="ts">
	import type { SessionActivity } from '$lib/types'; // Adjust path

	type Props = {
		activity: SessionActivity | null;
		// results: any; // This would be the actual results data
		showResultsToParticipants: boolean;
	};

	let {
		activity,
		// results,
		showResultsToParticipants
	}: Props = $props();

	function getDummyResults() {
		if (!activity) return null;
		switch (activity.type) {
			case 'MultipleChoice':
			case 'Poll':
				return {
					type: 'bar',
					data: [
						{ label: 'Option A', value: 10 },
						{ label: 'Option B', value: 15 },
						{ label: 'Option C', value: 5 }
					]
				};
			case 'ScaleRating':
				return { type: 'distribution', data: [2, 5, 10, 8, 3] }; // Counts for 1,2,3,4,5
			case 'OpenEnded':
				return { type: 'list', data: ['Great session!', 'More examples please.', 'Loved it!'] };
			default:
				return { type: 'text', data: 'Results will appear here.' };
		}
	}

	// Dummy results based on type for placeholder
	let dummyResults = $derived(getDummyResults());
</script>

<div class="live-results-display">
	{#if !activity}
		<p class="live-results-display__placeholder">No activity is currently active.</p>
	{:else if !dummyResults}
		<p class="live-results-display__placeholder">Waiting for results...</p>
	{:else}
		<h4 class="live-results-display__title">Live Results: {activity.title}</h4>
		{#if showResultsToParticipants}
			<p class="live-results-display__sharing-status live-results-display__sharing-status--visible">
				Results are VISIBLE to participants
			</p>
		{:else}
			<p class="live-results-display__sharing-status live-results-display__sharing-status--hidden">
				Results are HIDDEN from participants
			</p>
		{/if}

		<div class="live-results-display__chart-area">
			{#if dummyResults.type === 'bar'}
				<p>Bar Chart Placeholder for: {JSON.stringify(dummyResults.data)}</p>
			{:else if dummyResults.type === 'distribution'}
				<p>Distribution/Scale Placeholder for: {JSON.stringify(dummyResults.data)}</p>
			{:else if dummyResults.type === 'list'}
				<ul>
					{#each dummyResults.data as item}<li>{item}</li>{/each}
				</ul>
			{:else}
				<p>{dummyResults.data}</p>
			{/if}
		</div>
	{/if}
</div>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

	.live-results-display {
		padding: $spacing-md;
		background-color: $color-surface;
		border: 1px solid $color-border-light;
		border-radius: $border-radius-lg;
		min-height: 200px; // Ensure some height
		display: flex;
		flex-direction: column;

		&__placeholder {
			font-style: italic;
			color: $color-text-secondary;
			text-align: center;
			margin: auto; // Center placeholder text
		}

		&__title {
			font-size: $font-size-md;
			font-weight: $font-weight-semibold;
			margin-bottom: $spacing-sm;
			color: $color-text-primary;
		}

		&__sharing-status {
			font-size: $font-size-xs;
			padding: $spacing-xs * 0.5 $spacing-sm;
			border-radius: $border-radius-sm;
			margin-bottom: $spacing-md;
			text-align: center;

			&--visible {
				background-color: rgba($color-success, 0.1);
				color: darken($color-success, 10%);
			}
			&--hidden {
				background-color: $color-surface-alt;
				color: $color-text-secondary;
			}
		}

		&__chart-area {
			flex-grow: 1;
			// Placeholder for actual chart styling
			border: 1px dashed $color-border;
			display: flex;
			align-items: center;
			justify-content: center;
			padding: $spacing-md;
			font-size: $font-size-sm;
			color: $color-text-disabled;
		}
	}
</style>
