<script lang="ts">
	import type { Snippet } from 'svelte';

	// Props allow passing custom classes if needed
	type Props = {
		class?: string; // Allow passing additional classes
		children: Snippet; // Default slot for content
	};

	let { class: customClass = '', children }: Props = $props();

	// Combine default and custom classes
	let combinedClass = `topbar ${customClass}`.trim();
</script>

<header class={combinedClass}>
	{@render children()}
</header>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path if needed

	// Block: topbar
	.topbar {
		// Provides the basic container style for all top bars
		display: flex;
		align-items: center;
		padding: $spacing-md $spacing-lg;
		background-color: $color-surface;
		border-bottom: $border-width-thin solid $color-border-light;
		height: 60px; // Consistent height
		flex-shrink: 0; // Prevent shrinking in flex layouts
		gap: $spacing-sm; // Default gap between items placed inside
	}
</style>
