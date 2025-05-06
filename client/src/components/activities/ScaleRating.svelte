<script lang="ts">
	import type { ScaleRatingDefinition } from '$lib/types'; // Adjust path

	let { definition }: { definition: ScaleRatingDefinition } = $props();
</script>

<div class="activity-display activity-display--scale-rating">
	<div class="scale-container">
		<span class="scale-label scale-label--min">{definition.minLabel ?? definition.min}</span>
		<div class="scale-bar">
			{#each Array(definition.max - definition.min + 1) as _, i}
				<span class="scale-point">{definition.min + i}</span>
			{/each}
		</div>
		<span class="scale-label scale-label--max">{definition.maxLabel ?? definition.max}</span>
	</div>
</div>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

	.activity-display {
		margin-top: $spacing-sm;
	}
	.scale-container {
		display: flex;
		align-items: center;
		gap: $spacing-md;
		background-color: $color-surface-alt;
		padding: $spacing-sm $spacing-md;
		border-radius: $border-radius-md;
	}
	.scale-label {
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
	.scale-bar {
		flex-grow: 1;
		display: flex;
		justify-content: space-between;
		border: 1px solid $color-border-light;
		padding: $spacing-xs;
		border-radius: $border-radius-sm;
	}
	.scale-point {
		font-size: $font-size-sm;
		color: $color-text-primary;
		// Add more styling if needed
	}
</style>
