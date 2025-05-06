<script lang="ts">
	import type { PollDefinition } from '$lib/types'; // Adjust path

	let { definition }: { definition: PollDefinition } = $props();
</script>

<div class="activity-display activity-display--poll">
	<ul class="activity-display__options-list">
		{#each definition.options as option (option.id)}
			<li class="activity-display__option">
				<span class="activity-display__option-marker">▫</span>
				<span class="activity-display__option-text">{option.text}</span>
			</li>
		{/each}
	</ul>
</div>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

	// Inherit styles from MultipleChoice or define separately
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
		}
		&__option-marker {
			color: $color-text-secondary;
		}
		&__option-text {
			color: $color-text-primary;
		}
	}
</style>
