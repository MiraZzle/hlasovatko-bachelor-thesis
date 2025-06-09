<script lang="ts">
	import type { MultipleChoiceDefinition } from '$lib/activity_types';

	let { definition }: { definition: MultipleChoiceDefinition } = $props();
	const CHECK_ICON = '✔';
	const CIRCLE_ICON = '○';

	/**
	 * Checks if a given option ID is correct based on the definition.
	 *
	 * @param optionId - The ID of the option to check.
	 * @returns `true` if the option is correct; otherwise, `false`.
	 */
	function isCorrect(optionId: string): boolean {
		if (!definition.correctOptionId) return false;
		if (Array.isArray(definition.correctOptionId)) {
			return definition.correctOptionId.includes(optionId);
		}
		return definition.correctOptionId === optionId;
	}
</script>

<div class="activity-display activity-display--multiple-choice">
	<ul class="activity-display__options-list">
		{#each definition.options as option (option.id)}
			<li
				class="activity-display__option"
				class:activity-display__option--correct={isCorrect(option.id)}
			>
				<span class="activity-display__option-marker"
					>{isCorrect(option.id) ? CHECK_ICON : CIRCLE_ICON}</span
				>
				<span class="activity-display__option-text">{option.text}</span>
			</li>
		{/each}
	</ul>
	{#if definition.allowMultiple}
		<p class="activity-display__note">(Multiple answers allowed)</p>
	{/if}
</div>

<style lang="scss">
	@import '../../styles/variables.scss';

	.activity-display {
		margin-top: $spacing-sm;

		&__options-list {
			list-style: none;
			padding: 0;
			margin: 0;
			display: flex;
			flex-direction: column;
			gap: $spacing-xs;
		}
		&__option {
			display: flex;
			align-items: center;
			gap: $spacing-sm;
			padding: $spacing-xs $spacing-sm;
			border-radius: $border-radius-sm;
			background-color: $color-surface-alt;

			&--correct {
				background-color: rgba($color-success, 0.1);
				border: 1px solid rgba($color-success, 0.3);
			}
		}
		&__option-marker {
			font-weight: $font-weight-bold;
			color: $color-text-secondary;
			&--correct {
				color: $color-success;
			}
		}
		&__option-text {
			color: $color-text-primary;
		}
		&__note {
			font-size: $font-size-xs;
			color: $color-text-secondary;
			margin-top: $spacing-sm;
			font-style: italic;
		}
	}
</style>
