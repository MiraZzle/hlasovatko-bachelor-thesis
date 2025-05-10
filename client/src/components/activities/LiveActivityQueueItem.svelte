<script lang="ts">
	import type { SessionActivity } from '$lib/types'; // Adjust path
	import Button from '$components/elements/typography/Button.svelte'; // Verify path

	type Props = {
		activity: SessionActivity;
		isCurrent: boolean; // Is this the currently live activity?
		isNextUp?: boolean; // Is this cued as the next one? (optional)
		onActivate: (activityId: string) => void; // Callback to make this activity live
	};

	let { activity, isCurrent = false, isNextUp = false, onActivate }: Props = $props();

	// Placeholder icons
	const IconPlay = '▶️';
	const IconCheck = '✔️';
</script>

<div
	class="live-activity-queue-item"
	class:live-activity-queue-item--current={isCurrent}
	class:live-activity-queue-item--next-up={isNextUp && !isCurrent}
	aria-current={isCurrent ? 'step' : undefined}
>
	<div class="live-activity-queue-item__info">
		<span class="live-activity-queue-item__type">{activity.type}</span>
		<p class="live-activity-queue-item__title">{activity.title}</p>
		{#if activity.status === 'Closed'}
			<span class="live-activity-queue-item__status-indicator">{IconCheck} Run</span>
		{/if}
	</div>
	<div class="live-activity-queue-item__actions">
		{#if !isCurrent && activity.status !== 'Closed'}
			<Button variant="outline" onclick={() => onActivate(activity.id)}>Activate</Button>
		{/if}
		{#if isCurrent}
			<span class="live-activity-queue-item__current-indicator">LIVE</span>
		{/if}
	</div>
</div>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

	// Block: live-activity-queue-item
	.live-activity-queue-item {
		display: flex;
		justify-content: space-between;
		align-items: center;
		padding: $spacing-sm $spacing-md;
		background-color: $color-surface;
		border: 1px solid $color-border-light;
		border-radius: $border-radius-md;
		transition: all $transition-duration-fast;
		gap: $spacing-md;

		// Element: Info section
		&__info {
			display: flex;
			flex-direction: column;
			gap: $spacing-xs * 0.5;
			flex-grow: 1;
			overflow: hidden; // Prevent long titles from breaking layout
		}

		// Element: Activity Type
		&__type {
			font-size: $font-size-xs;
			color: $color-text-secondary;
			font-weight: $font-weight-medium;
			text-transform: uppercase;
		}

		// Element: Activity Title
		&__title {
			font-size: $font-size-sm;
			font-weight: $font-weight-medium;
			color: $color-text-primary;
			margin: 0;
			white-space: nowrap;
			overflow: hidden;
			text-overflow: ellipsis;
		}

		// Element: Status Indicator (e.g., if already run)
		&__status-indicator {
			font-size: $font-size-xs;
			color: $color-success;
			font-weight: $font-weight-medium;
			display: flex;
			align-items: center;
			gap: $spacing-xs * 0.5;
		}

		// Element: Actions container
		&__actions {
			flex-shrink: 0;
		}

		// Element: Current Indicator
		&__current-indicator {
			font-size: $font-size-xs;
			font-weight: $font-weight-bold;
			color: $color-primary;
			padding: $spacing-xs $spacing-sm;
			background-color: rgba($color-primary, 0.1);
			border-radius: $border-radius-sm;
			text-transform: uppercase;
		}

		// Modifier: Current activity
		&--current {
			border-left: 4px solid $color-primary;
			background-color: lighten($color-surface-alt, 2%);
			box-shadow: $box-shadow-sm;
		}

		// Modifier: Next up activity (optional visual cue)
		&--next-up {
			border-left: 4px solid $color-secondary;
		}

		&:not(&--current):hover {
			background-color: $color-surface-alt;
		}
	}
</style>
