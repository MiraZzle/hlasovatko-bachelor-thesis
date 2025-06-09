<script lang="ts">
	import type { ScaleRatingDefinition } from '$lib/activity_types';

	let { definition }: { definition: ScaleRatingDefinition } = $props();

	const range = Array.from(
		{ length: definition.max - definition.min + 1 },
		(_, i) => definition.min + i
	);
</script>

<div class="activity-display activity-display--scale-rating">
	<div class="activity-display__scale-container">
		<span class="activity-display__scale-label activity-display__scale-label--min">
			{definition.minLabel ?? definition.min}
		</span>
		<div class="activity-display__scale-bar">
			{#each range as point}
				<span class="activity-display__scale-point">{point}</span>
			{/each}
		</div>
		<span class="activity-display__scale-label activity-display__scale-label--max">
			{definition.maxLabel ?? definition.max}
		</span>
	</div>
</div>

<style lang="scss">
	@import '../../styles/variables.scss';

	.activity-display {
		margin-top: $spacing-sm;

		&__scale-container {
			display: flex;
			align-items: center;
			gap: $spacing-md;
			background-color: $color-surface-alt;
			padding: $spacing-sm $spacing-md;
			border-radius: $border-radius-md;
		}

		&__scale-label {
			font-size: $font-size-xs;
			color: $color-text-secondary;
			white-space: nowrap;

			&--min {
				text-align: right;
			}
			&--max {
				text-align: left;
			}
		}

		&__scale-bar {
			flex-grow: 1;
			display: flex;
			justify-content: space-between;
			border: 1px solid $color-border-light;
			padding: $spacing-xs;
			border-radius: $border-radius-sm;
		}

		&__scale-point {
			font-size: $font-size-sm;
			color: $color-text-primary;
		}
	}
</style>
