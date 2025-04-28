<script lang="ts">
	import { page } from '$app/stores'; // To determine the active route

	// Dummy icons - replace with actual SVG components later
	const IconTemplates = '📄';
	const IconSessions = '▶️';
	const IconAnalytics = '📊';
	const IconActivityBank = '🏦';

	const linkBase = '/overview'; // Base path for the sidebar links

	const mainNavLinks = [
		{ href: `${linkBase}/templates`, label: 'My Templates', icon: IconTemplates },
		{ href: `${linkBase}/sessions`, label: 'My Sessions', icon: IconSessions }, // Assuming route exists
		{ href: `${linkBase}/analytics`, label: 'Analytics', icon: IconAnalytics },
		{ href: `${linkBase}/activity-bank`, label: 'Activity Bank', icon: IconActivityBank }
	];

	function isActive(href: string): boolean {
		// Check if the current page pathname starts with the link's href
		// Handles nested routes like /templates/new
		return $page.url.pathname === href || $page.url.pathname.startsWith(href + '/');
	}
</script>

<aside class="sidebar">
	<a href="/" class="sidebar__logo" aria-label="Homepage"> EngaGenie </a>

	<nav class="sidebar__nav sidebar__nav--main" aria-label="Main">
		<ul>
			{#each mainNavLinks as link}
				<li>
					<a
						href={link.href}
						class="sidebar__link"
						class:sidebar__link--active={isActive(link.href)}
						aria-current={isActive(link.href) ? 'page' : undefined}
					>
						<span class="sidebar__link-icon" aria-hidden="true">{link.icon}</span>
						<span class="sidebar__link-text">{link.label}</span>
					</a>
				</li>
			{/each}
		</ul>
	</nav>
</aside>

<style lang="scss">
	@import '../../styles/variables.scss';

	.sidebar {
		background-color: $color-surface; // Sidebar background
		border-right: $border-width-thin solid $color-border-light;
		padding: $spacing-lg $spacing-md;
		display: flex;
		flex-direction: column;
		gap: $spacing-xl; // Space between logo, navs, footer
		width: 240px; // Fixed width for the sidebar
		height: 100%; // Takes full height of its container
		flex-shrink: 0; // Prevent shrinking if container is flex
	}

	.sidebar__logo {
		font-size: $font-size-xl;
		font-weight: $font-weight-bold;
		color: $color-text-primary;
		text-decoration: none;
		padding: $spacing-sm $spacing-xs; // Padding around logo area

		&:hover {
			text-decoration: none;
			color: $color-primary;
		}
	}

	.sidebar__nav {
		ul {
			list-style: none;
			padding: 0;
			margin: 0;
			display: flex;
			flex-direction: column;
			gap: $spacing-xs; // Small gap between links
		}

		// Add space between main and secondary nav sections if needed
		&--main {
			flex-grow: 1; // Allow main nav to push secondary nav down
		}
	}

	.sidebar__link {
		display: flex;
		align-items: center;
		gap: $spacing-md;
		padding: $spacing-sm $spacing-xs; // Padding inside link
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

		// Active state
		&--active {
			background-color: rgba($color-primary, 0.1); // Light primary background
			color: $color-primary;
			font-weight: $font-weight-semibold;

			.sidebar__link-icon {
				// Optional: slightly bolder icon color on active
				color: $color-primary;
			}
		}
	}

	.sidebar__link-icon {
		width: 20px; // Ensure icons align
		height: 20px;
		display: inline-flex;
		align-items: center;
		justify-content: center;
		flex-shrink: 0;
		// Color is inherited usually, but can be set specifically
	}

	.sidebar__link-text {
		// Styles for text if needed
	}
</style>
