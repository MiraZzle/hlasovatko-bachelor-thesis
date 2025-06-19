<script lang="ts">
	/**
	 * @file Reusable component for displaying a stat card in the dashboard.
	 * Renders a title, optional icon, and content area for statistics.
	 */
	import type { Snippet } from 'svelte';

	let {
		title,
		icon = null as Snippet | null,
		children
	}: {
		title: string;
		icon?: Snippet | null;
		children: Snippet;
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
	.stat-card {
		background-color: $color-surface;
		border-radius: $border-radius-lg;
		padding: $spacing-lg;
		box-shadow: $box-shadow-sm;
		border: $border-width-thin solid $color-border-light;
		height: 100%;
		display: flex;
		flex-direction: column;
	}

	.stat-card__header {
		display: flex;
		align-items: center;
		gap: $spacing-sm;
		margin-bottom: $spacing-md;
		color: $color-text-secondary;
	}

	.stat-card__icon {
		display: inline-flex;
		width: 24px;
		height: 24px;
		flex-shrink: 0;
	}

	.stat-card__title {
		font-size: $font-size-md;
		font-weight: $font-weight-semibold;
		color: $color-text-primary;
		margin: 0;
	}

	.stat-card__content {
		flex-grow: 1;
		font-size: $font-size-sm;
		color: $color-text-secondary;
	}
</style>
