<script lang="ts">
	import type { ChoiceActivityResult, MultipleChoiceDefinition } from '$lib/activity_types'; // Adjust path

	type Props = {
		results: ChoiceActivityResult;
		definition?: MultipleChoiceDefinition | null; // Optional definition to mark correct answer
	};
	let { results, definition = null }: Props = $props();

	let totalVotes = $derived(results.reduce((sum, option) => sum + option.count, 0));

	// Calculate percentage safely
	function getPercentage(count: number): number {
		return totalVotes > 0 ? Math.round((count / totalVotes) * 100) : 0;
	}

	// Check correctness based on optional definition
	function isCorrect(optionId: string): boolean | undefined {
		if (!definition || !definition.correctOptionId) return undefined; // Not applicable or no correct answer defined
		if (Array.isArray(definition.correctOptionId)) {
			return definition.correctOptionId.includes(optionId);
		}
		return definition.correctOptionId === optionId;
	}
</script>

<div class="choice-results-display">
	{#if totalVotes > 0}
		<ul class="choice-results-display__list">
			{#each results as optionResult (optionResult.id)}
				{@const percentage = getPercentage(optionResult.count)}
				{@const correctStatus = isCorrect(optionResult.id)}
				<li
					class="choice-results-display__item"
					class:choice-results-display__item--correct={correctStatus === true}
					class:choice-results-display__item--incorrect={correctStatus === false}
				>
					<div class="choice-results-display__label-container">
						<span class="choice-results-display__label">{optionResult.text}</span>
						{#if correctStatus !== undefined}
							<span class="choice-results-display__correct-marker">
								{correctStatus ? '✔ Correct' : '✘'}
							</span>
						{/if}
					</div>
					<div class="choice-results-display__bar-container">
						<div class="choice-results-display__bar" style:width="{percentage}%"></div>
					</div>
					<span class="choice-results-display__value">{optionResult.count} ({percentage}%)</span>
				</li>
			{/each}
		</ul>
	{:else}
		<p class="choice-results-display__no-results">No responses received yet.</p>
	{/if}
	<p class="choice-results-display__total">Total Responses: {totalVotes}</p>
</div>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

	.choice-results-display {
		&__list {
			list-style: none;
			padding: 0;
			margin: 0;
			display: flex;
			flex-direction: column;
			gap: $spacing-sm;
		}
		&__item {
			display: grid;
			grid-template-columns: minmax(100px, 1fr) 2fr auto;
			align-items: center;
			gap: $spacing-sm;
			font-size: $font-size-sm;
			padding-bottom: $spacing-sm;
			border-bottom: 1px dotted $color-border-light;
			&:last-child {
				border-bottom: none;
			}
		}
		&__label-container {
			display: flex;
			flex-direction: column;
			align-items: flex-start;
		}
		&__label {
			color: $color-text-primary;
			word-break: break-word;
		}
		&__correct-marker {
			font-size: $font-size-xs;
			color: $color-error;
			font-weight: $font-weight-medium;
		}
		&__item--correct &__correct-marker {
			color: $color-success;
		}
		&__bar-container {
			background-color: $color-surface-alt;
			border-radius: $border-radius-sm;
			height: 16px;
			overflow: hidden;
		}
		&__bar {
			height: 100%;
			background-color: $color-primary-light;
			border-radius: $border-radius-sm;
			transition: width 0.3s ease-out;
		}
		&__item--correct &__bar {
			background-color: $color-success;
		} // Correct answer bar color
		&__value {
			font-weight: $font-weight-medium;
			color: $color-text-secondary;
			white-space: nowrap;
			text-align: right;
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
