<script lang="ts">
	// Define the expected shape of the activity data
	interface Activity {
		id: string;
		type: string;
		question: string;
		tags: string[];
		// Add other relevant fields if needed, like category, date, etc.
	}

	// Props: activity data and an optional click handler
	let {
		activity,
		onclick = (id: string) => {
			console.log('Activity card clicked:', id);
		} // Default dummy handler
	}: {
		activity: Activity;
		onclick?: (id: string) => void;
	} = $props();
</script>

<button type="button" class="activity-card" onclick={() => onclick(activity.id)}>
	<div class="activity-card__type">{activity.type}</div>
	<p class="activity-card__question">{activity.question}</p>
	<div class="activity-card__footer">
		<div class="activity-card__tags">
			{#each activity.tags as tag (tag)}
				<span class="activity-card__tag">{tag}</span>
			{/each}
		</div>
	</div>
</button>

<style lang="scss">
	@import '../../styles/variables.scss';

	// Block: activity-card
	.activity-card {
		// Reset button styles
		background: none;
		border: none;
		padding: 0;
		margin: 0;
		font: inherit;
		color: inherit;
		cursor: pointer;
		text-align: left;
		width: 100%; // Take full width of grid cell

		// Card appearance
		display: flex;
		flex-direction: column;
		justify-content: space-between; // Push footer down
		background-color: $color-surface;
		border-radius: $border-radius-md; // Slightly smaller radius than table wrapper?
		padding: $spacing-md;
		box-shadow: $box-shadow-sm;
		border: $border-width-thin solid $color-border-light;
		min-height: 150px; // Example minimum height
		transition:
			box-shadow $transition-duration-fast,
			transform $transition-duration-fast;

		&:hover {
			box-shadow: $box-shadow-md;
			transform: translateY(-2px);
		}
		&:focus-visible {
			outline: 2px solid $color-primary-light;
			outline-offset: 2px;
			box-shadow: $box-shadow-md; // Add shadow on focus too
		}

		// Element: Activity Type (e.g., "Quiz")
		&__type {
			font-size: $font-size-xs;
			font-weight: $font-weight-medium;
			color: $color-text-secondary;
			text-transform: uppercase;
			margin-bottom: $spacing-sm;
		}

		// Element: Main Question/Prompt text
		&__question {
			font-size: $font-size-md;
			color: $color-text-primary;
			line-height: 1.4;
			margin-bottom: $spacing-lg; // Space above footer
			// Allow text to wrap
			white-space: normal;
			// Limit lines shown? (Optional)
			// display: -webkit-box;
			// -webkit-line-clamp: 3; // Show max 3 lines
			// -webkit-box-orient: vertical;
			// overflow: hidden;
			// text-overflow: ellipsis;
		}

		// Element: Footer area containing tags/actions
		&__footer {
			display: flex;
			justify-content: flex-end; // Align tags to the right by default
			align-items: center;
			margin-top: auto; // Ensure footer is pushed down
		}

		// Element: Container for tags
		&__tags {
			display: flex;
			flex-wrap: wrap;
			gap: $spacing-xs;
		}

		// Element: Individual tag
		&__tag {
			background-color: $color-surface-alt;
			color: $color-text-secondary;
			padding: $spacing-xs * 0.5 $spacing-sm; // Slightly smaller padding
			border-radius: $border-radius-sm; // Smaller radius for tags
			font-size: $font-size-xs;
			font-weight: $font-weight-medium;
			white-space: nowrap;
		}
	}
</style>
