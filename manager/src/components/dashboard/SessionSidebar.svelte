<script lang="ts">
	import { page } from '$app/stores';
	import { goto } from '$app/navigation';
	import Button from '$components/elements/typography/Button.svelte'; // Verify path
	import type { Snippet } from 'svelte';

	// --- Component Props ---
	// Define the structure for a navigation link
	interface NavLink {
		href: string;
		label: string;
		icon: string; // Expecting an icon component/snippet
	}

	type Props = {
		sessionId: string; // ID is needed for dynamic links
		links: NavLink[]; // Pass the navigation links as a prop
		// Add props for action handlers if they need to be controlled by parent
		onShare?: (id: string) => void;
		onPresent?: (id: string) => void;
		onBack?: () => void; // Handler for the back button
	};

	let {
		sessionId,
		links,
		onShare = (id) => console.log('Default Share action for:', id),
		onPresent = (id) => console.log('Default Present action for:', id),
		onBack = () => goto('/sessions') // Default back action
	}: Props = $props();

	// --- Helper Function ---
	// Checks if a given link href matches the current page URL path
	function isActive(href: string): boolean {
		// Ensure $page.url is available (it should be in most component contexts)
		return $page?.url?.pathname === href || $page?.url?.pathname?.startsWith(href + '/');
	}

	// --- Placeholder Icons (Import or define actual icons) ---
	const IconBack = () => '🔙'; // Placeholder for back icon
	const IconShare = () => '🔗'; // Placeholder
	const IconPresent = () => '💡'; // Placeholder
</script>

<aside class="session-sidebar">
	<button class="session-sidebar__back-button" onclick={onBack} aria-label="Back to all sessions">
		Back to Sessions
	</button>

	<nav class="session-sidebar__nav" aria-label="Session Sections">
		<ul>
			{#each links as link (link.href)}
				<li>
					<a
						href={link.href}
						class="session-sidebar__link"
						class:session-sidebar__link--active={isActive(link.href)}
						aria-current={isActive(link.href) ? 'page' : undefined}
					>
						<span class="session-sidebar__link-icon" aria-hidden="true">{link.icon}</span>
						<span class="session-sidebar__link-text">{link.label}</span>
					</a>
				</li>
			{/each}
		</ul>
	</nav>

	<div class="session-sidebar__actions">
		<Button variant="outline" onclick={() => onShare(sessionId)}>Share</Button>
		<Button variant="primary" onclick={() => onPresent(sessionId)}>Present</Button>
	</div>
</aside>

<style lang="scss">
	@import '../../styles/variables.scss';

	// Block: session-sidebar
	.session-sidebar {
		background-color: $color-surface;
		border-right: $border-width-thin solid $color-border-light;
		padding: $spacing-lg $spacing-md;
		display: flex;
		flex-direction: column;
		width: 240px;
		height: 100%;
		flex-shrink: 0;
		gap: $spacing-xl; // Consistent gap

		// Element: Back button
		&__back-button {
			display: inline-flex;
			align-items: center;
			gap: $spacing-sm;
			background: none;
			border: none;
			padding: $spacing-xs;
			margin: -$spacing-xs;
			cursor: pointer;
			color: $color-text-secondary;
			font-size: $font-size-sm;
			font-weight: $font-weight-medium;
			border-radius: $border-radius-md;
			text-align: left;
			&:hover {
				background-color: $color-surface-alt;
				color: $color-text-primary;
			}
			&:focus-visible {
				outline: 2px solid $color-primary-light;
				outline-offset: 1px;
			}
			svg {
				stroke-width: 2;
				height: 16px;
				width: 16px;
			}
		}

		// Element: Navigation
		&__nav {
			margin-bottom: $spacing-xl;
			flex-grow: 1; // Push actions down
			ul {
				list-style: none;
				padding: 0;
				margin: 0;
				display: flex;
				flex-direction: column;
				gap: $spacing-xs;
			}
		}

		// Element: Navigation Link
		&__link {
			display: flex;
			align-items: center;
			gap: $spacing-md;
			padding: $spacing-sm $spacing-xs;
			border-radius: $border-radius-md;
			text-decoration: none;
			font-size: $font-size-md;
			font-weight: $font-weight-medium;
			color: $color-text-secondary;
			transition:
				background-color $transition-duration-fast,
				color $transition-duration-fast;
			&:hover {
				background-color: $color-surface-alt;
				color: $color-text-primary;
			}
			// Modifier: Active Link
			&--active {
				background-color: rgba($color-primary, 0.1);
				color: $color-primary;
				font-weight: $font-weight-semibold;
			}
		}
		// Element: Link Icon
		&__link-icon {
			display: inline-flex;
			width: 20px;
			height: 20px;
			align-items: center;
			justify-content: center;
			flex-shrink: 0;
		}
		// Element: Link Text
		&__link-text {
			/* styles */
		}

		// Element: Actions container
		&__actions {
			display: flex;
			flex-direction: column;
			gap: $spacing-md;
			// Ensure buttons are full width within sidebar context
			:global(.button) {
				// Use :global necessary here as Button is separate component
				width: 100%;
			}
		}
	}
</style>
