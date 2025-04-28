<script lang="ts">
	import type { Snippet } from 'svelte';

	let {
		title,
		icon = null as Snippet | null, // Allow passing icon as a snippet
		children // Default slot for content
	}: {
		title: string;
		icon?: Snippet | null;
		children: Snippet; // Default slot for content
	} = $props();
</script>

<div class="stat-card">
	<div class="stat-card__header">
		{#if icon}
			<div class="stat-card__icon" aria-hidden="true">
				{@render icon()}
			</div>
		{/if}
		<h3 class="stat-card__title">{title}</h3>
	</div>
	<div class="stat-card__content">
		{@render children()}
	</div>
</div>

<style lang="scss">
	@import '../../styles/variables.scss';

	.stat-card {
		background-color: $color-surface;
		border-radius: $border-radius-lg;
		padding: $spacing-lg;
		box-shadow: $box-shadow-sm;
		border: $border-width-thin solid $color-border-light;
		height: 100%; // Allow card to fill grid cell height
		display: flex;
		flex-direction: column;
	}

	.stat-card__header {
		display: flex;
		align-items: center;
		gap: $spacing-sm;
		margin-bottom: $spacing-md;
		color: $color-text-secondary; // Icon/title color default
	}

	.stat-card__icon {
		// Style the icon container or the icon SVG itself if needed
		display: inline-flex; // Helps with alignment
		width: 24px; // Example size
		height: 24px;
		flex-shrink: 0;
	}

	.stat-card__title {
		font-size: $font-size-md;
		font-weight: $font-weight-semibold;
		color: $color-text-primary;
		margin: 0; // Reset heading margin
	}

	.stat-card__content {
		flex-grow: 1; // Allow content to take remaining space
		font-size: $font-size-sm;
		color: $color-text-secondary;

		// Example styling for key-value pairs often found in stat cards
		dl {
			margin: 0;
		}
		dt {
			float: left;
			clear: left;
			margin-right: $spacing-sm;
			font-weight: $font-weight-medium;
			color: $color-text-primary;
		}
		dd {
			margin-left: 0; // Reset browser default
			text-align: right;
			font-weight: $font-weight-semibold;
			color: $color-text-primary;
			margin-bottom: $spacing-xs;
		}

		// Example styling for breakdown list
		ul {
			list-style: none;
			padding: 0;
			margin: 0;
		}
		li {
			display: flex;
			justify-content: space-between;
			margin-bottom: $spacing-xs;
			padding-bottom: $spacing-xs;
			border-bottom: 1px dotted $color-border-light;
			&:last-child {
				border-bottom: none;
				margin-bottom: 0;
			}
		}
		.label {
			/* for list item label */
		}
		.value {
			/* for list item value */
			font-weight: $font-weight-semibold;
			color: $color-text-primary;
		}
	}
</style>
