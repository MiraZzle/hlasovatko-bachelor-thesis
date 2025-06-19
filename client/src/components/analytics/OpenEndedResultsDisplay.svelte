<script lang="ts">
	/**
	 * @file Component for displaying the results of an open-ended activity.
	 * It shows a list of responses submitted by participants.
	 */
	import type { OpenEndedActivityResult } from '$lib/activities/definition_types';

	let {
		results
	}: {
		results: OpenEndedActivityResult;
	} = $props();
</script>

<div class="open-ended-results-display">
	{#if results.length > 0}
		<ul class="open-ended-results-display__list">
			{#each results as response, i (i)}
				<li class="open-ended-results-display__item">{response}</li>
			{/each}
		</ul>
	{:else}
		<p class="open-ended-results-display__no-results">No responses received yet.</p>
	{/if}
	<p class="open-ended-results-display__total">Total Responses: {results.length}</p>
</div>

<style lang="scss">
	.open-ended-results-display {
		max-height: 300px;
		overflow-y: auto;
		border: 1px solid $color-border-light;
		border-radius: $border-radius-md;
		padding: $spacing-sm;
		background-color: $color-surface-alt;

		&__list {
			list-style: none;
			padding: 0;
			margin: 0;
		}
		&__item {
			padding: $spacing-sm;
			background-color: $color-surface;
			border-radius: $border-radius-sm;
			margin-bottom: $spacing-sm;
			font-size: $font-size-sm;
			color: $color-text-primary;
			&:last-child {
				margin-bottom: 0;
			}
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
			padding-right: $spacing-sm;
		}
	}
</style>
