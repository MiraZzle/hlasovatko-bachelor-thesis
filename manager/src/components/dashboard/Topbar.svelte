<script lang="ts">
	/**
	 * @file Reusable component for the top bar in the dashboard.
	 * Provides a header with customizable content and styling.
	 */
	import type { Snippet } from 'svelte';

	let {
		class: customClass = '',
		children
	}: {
		class?: string;
		children: Snippet;
	} = $props();
	let combinedClass = `topbar ${customClass}`.trim();
</script>

<header class={combinedClass}>
	{@render children()}
</header>

<style lang="scss">
	.topbar {
		display: flex;
		align-items: center;
		padding: $spacing-md $spacing-lg;
		background-color: $color-surface;
		border-bottom: $border-width-thin solid $color-border-light;
		height: 60px;
		flex-shrink: 0;
		gap: $spacing-sm;
	}
</style>
