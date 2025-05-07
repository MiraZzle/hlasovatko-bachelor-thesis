<script lang="ts">
	import { page } from '$app/stores'; // To determine the active route and params
	import type { Snippet } from 'svelte'; // Needed for icon definitions

	// --- Define Link Structure ---
	interface NavLink {
		href: string;
		label: string;
		icon: String; // Expect Snippet type for icons
		subLinks?: NavLink[]; // Optional array for nested links
	}

	// --- Placeholder Icons (Replace with actual components/SVGs) ---
	const IconTemplates = '📄';
	const IconSessions = '▶️';
	const IconAnalytics = '📊';
	const IconActivityBank = '🏦';
	const IconOverview = '👀'; // Example for session overview
	const IconSessionActivities = '⚡';
	const IconSessionAnalytics = '📈';

	const OverViewBasePath = '/overview';

	function getMainNavLinks() {
		const params = $page.params;
		const currentSessionId = params.session_id; // Check if we have a session_id param

		// Base links
		const links: NavLink[] = [
			{ href: `${OverViewBasePath}/templates`, label: 'My Templates', icon: IconTemplates },
			{ href: `${OverViewBasePath}/sessions`, label: 'My Sessions', icon: IconSessions },
			{ href: `${OverViewBasePath}/analytics`, label: 'Analytics', icon: IconAnalytics },
			{ href: `${OverViewBasePath}/activity-bank`, label: 'Activity Bank', icon: IconActivityBank }
		];

		// If we are inside a specific session route, add sub-links to "My Sessions"
		if (currentSessionId && $page.url.pathname.startsWith(`${OverViewBasePath}/sessions/`)) {
			const sessionLinkIndex = links.findIndex(
				(link) => link.href === `${OverViewBasePath}/sessions` // <-- CORRECTED HREF
			);
			console.log('Session Link Index:', sessionLinkIndex); // Debugging line
			if (sessionLinkIndex !== -1) {
				links[sessionLinkIndex].subLinks = [
					{
						href: `${OverViewBasePath}/sessions/${currentSessionId}/overview`,
						label: 'Overview',
						icon: IconOverview
					},
					{
						href: `${OverViewBasePath}/sessions/${currentSessionId}/activities`,
						label: 'Activities',
						icon: IconSessionActivities
					},
					{
						href: `${OverViewBasePath}/sessions/${currentSessionId}/analytics`,
						label: 'Analytics',
						icon: IconSessionAnalytics
					}
				];
			}
		}
		return links;
	}

	// --- Reactive Navigation Structure ---
	let mainNavLinks = $derived(getMainNavLinks());

	// --- Helper Function for Active State ---
	function isActive(href: string, isParent = false): boolean {
		const currentPath = $page.url.pathname;
		if (isParent) {
			// Parent is active if the current path STARTS with the parent href (e.g., /sessions active for /sessions/123/overview)
			// Special case for exact match too.
			return currentPath === href || currentPath.startsWith(href + '/');
		} else {
			// Exact match for sublinks or non-parent links
			return currentPath === href;
		}
	}
</script>

<aside class="sidebar">
	<a href="/" class="sidebar__logo" aria-label="Homepage"> EngaGenie </a>

	<nav class="sidebar__nav sidebar__nav--main" aria-label="Main">
		<ul>
			{#each mainNavLinks as link (link.href)}
				<li class="sidebar__item">
					<a
						href={link.href}
						class="sidebar__link"
						class:sidebar__link--active={isActive(link.href, true)}
						aria-current={isActive(link.href, true) ? 'page' : undefined}
					>
						<span class="sidebar__link-icon" aria-hidden="true">{link.icon}</span>
						<span class="sidebar__link-text">{link.label}</span>
					</a>

					{#if link.subLinks && isActive(link.href, true)}
						<ul class="sidebar__subnav">
							{#each link.subLinks as subLink (subLink.href)}
								<li class="sidebar__subitem">
									<a
										href={subLink.href}
										class="sidebar__sublink"
										class:sidebar__sublink--active={isActive(subLink.href)}
										aria-current={isActive(subLink.href) ? 'page' : undefined}
									>
										<span class="sidebar__sublink-icon" aria-hidden="true">{subLink.icon}</span>
										<span class="sidebar__sublink-text">{subLink.label}</span>
									</a>
								</li>
							{/each}
						</ul>
					{/if}
				</li>
			{/each}
		</ul>
	</nav>
</aside>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

	// Block: sidebar
	.sidebar {
		background-color: $color-surface;
		border-right: $border-width-thin solid $color-border-light;
		padding: $spacing-lg $spacing-md;
		display: flex;
		flex-direction: column;
		gap: $spacing-xl;
		width: 240px;
		height: 100%;
		flex-shrink: 0;

		// Element: Logo
		&__logo {
			font-size: $font-size-xl;
			font-weight: $font-weight-bold;
			color: $color-text-primary;
			text-decoration: none;
			padding: $spacing-sm $spacing-xs;
			&:hover {
				text-decoration: none;
				color: $color-primary;
			}
		}

		// Element: Main Navigation Area
		&__nav {
			&--main {
				flex-grow: 1;
			}
			ul {
				list-style: none;
				padding: 0;
				margin: 0;
			}
		}

		// Element: Top-Level List Item
		&__item {
			margin-bottom: $spacing-xs; // Space between top-level items
		}

		// Element: Top-Level Link
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
				text-decoration: none;
			}
			// Modifier: Active Link
			&--active {
				background-color: rgba($color-primary, 0.05);
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

		// Element: Sub-navigation List
		&__subnav {
			list-style: none;
			padding: $spacing-xs 0 0 $spacing-lg; // Indent subnav
			margin: $spacing-xs 0 0 0; // Space above subnav
			border-left: 2px solid $color-border-light; // Indentation line
			margin-left: $spacing-xs + 2px; // Align with icon roughly
		}

		// Element: Sub-navigation List Item
		&__subitem {
			margin-bottom: $spacing-xs * 0.5;
		}

		// Element: Sub-navigation Link
		&__sublink {
			display: flex;
			align-items: center;
			gap: $spacing-sm;
			padding: $spacing-xs;
			border-radius: $border-radius-md;
			text-decoration: none;
			font-size: $font-size-sm;
			font-weight: $font-weight-regular;
			color: $color-text-secondary;
			transition:
				background-color $transition-duration-fast,
				color $transition-duration-fast;
			&:hover {
				background-color: $color-surface-alt;
				color: $color-text-primary;
				text-decoration: none;
			}
			// Modifier: Active Sublink
			&--active {
				background-color: rgba($color-primary, 0.1);
				color: $color-primary;
				font-weight: $font-weight-medium;
			}
		}
		// Element: Sublink Icon
		&__sublink-icon {
			display: inline-flex;
			width: 16px;
			height: 16px;
			align-items: center;
			justify-content: center;
			flex-shrink: 0;
			opacity: 0.8;
		}
		// Element: Sublink Text
		&__sublink-text {
			/* styles */
		}

		// Element: Footer (Optional)
		// &__footer { /* styles */ }
	}
</style>
