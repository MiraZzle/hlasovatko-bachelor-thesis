<script lang="ts">
	/**
	 * @file Reusable component for the dashboard sidebar navigation.
	 * Dynamically generates main navigation links and sublinks based on current page context.
	 */
	import { page } from '$app/stores';

	interface NavLink {
		href: string;
		label: string;
		icon: string;
		subLinks?: NavLink[];
	}

	const IconTemplates = '📄';
	const IconSessions = '▶️';
	const IconAnalytics = '📊';
	const IconActivityBank = '🏦';
	const IconOverview = '👀';
	const IconSessionActivities = '⚡';
	const IconSessionAnalytics = '📈';
	const IconProfile = '👤';
	const IconSettings = '⚙️';

	const OVERVIEW_BASE_PATH = '/overview';

	/*
	 * Dynamically generates the main navigation links for the dashboard sidebar.
	 * Also generates sublinks for selected pages.
	 * Returns an array of Navlinks.
	 */
	function getMainNavLinks(): NavLink[] {
		const params = $page.params;
		const currentSessionId = params.session_id;
		const currentTemplateId = params.template_id;

		// Base links
		const links: NavLink[] = [
			{ href: `${OVERVIEW_BASE_PATH}/templates`, label: 'My Templates', icon: IconTemplates },
			{ href: `${OVERVIEW_BASE_PATH}/sessions`, label: 'My Sessions', icon: IconSessions },
			{ href: `${OVERVIEW_BASE_PATH}/analytics`, label: 'Analytics', icon: IconAnalytics },
			{
				href: `${OVERVIEW_BASE_PATH}/activity-bank`,
				label: 'Activity Bank',
				icon: IconActivityBank
			},
			{
				href: `${OVERVIEW_BASE_PATH}/profile`,
				label: 'Profile',
				icon: IconProfile
			}
		];

		// Register "my templates" sublibks
		if (currentTemplateId && $page.url.pathname.startsWith(`${OVERVIEW_BASE_PATH}/templates/`)) {
			const templateLinkIndex = links.findIndex(
				(link) => link.href === `${OVERVIEW_BASE_PATH}/templates`
			);
			if (templateLinkIndex !== -1) {
				const baseTemplatePath = `${OVERVIEW_BASE_PATH}/templates/${currentTemplateId}`;
				links[templateLinkIndex].subLinks = [
					{
						href: `${baseTemplatePath}/overview`,
						label: 'Overview',
						icon: IconOverview
					},
					{
						href: `${baseTemplatePath}/sessions`,
						label: 'Sessions',
						icon: IconSessions
					},
					{
						href: `${baseTemplatePath}/settings`,
						label: 'Settings',
						icon: IconSettings
					}
				];
			}
		}

		// Register "my sessions" sublinks
		if (currentSessionId && $page.url.pathname.startsWith(`${OVERVIEW_BASE_PATH}/sessions/`)) {
			const sessionLinkIndex = links.findIndex(
				(link) => link.href === `${OVERVIEW_BASE_PATH}/sessions`
			);
			if (sessionLinkIndex !== -1) {
				const baseSessionPath = `${OVERVIEW_BASE_PATH}/sessions/${currentSessionId}`;
				links[sessionLinkIndex].subLinks = [
					{
						href: `${baseSessionPath}/overview`,
						label: 'Overview',
						icon: IconOverview
					},
					{
						href: `${baseSessionPath}/activities`,
						label: 'Activities',
						icon: IconSessionActivities
					},
					{
						href: `${baseSessionPath}/analytics`,
						label: 'Analytics',
						icon: IconSessionAnalytics
					}
				];
			}
		}
		return links;
	}

	let mainNavLinks = $derived(getMainNavLinks());

	/**
	 * Checks if the current page is active based on the provided href.
	 * @param href - The href to check against the current page URL.
	 * @param isParent - Whether to check for parent path matching.
	 * @returns True if the current page matches the href, false otherwise.
	 */
	function isActive(href: string, isParent = false): boolean {
		const currentPath = $page.url.pathname;
		if (isParent) {
			return currentPath === href || currentPath.startsWith(href + '/');
		} else {
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
		max-width: 80vw;
		overflow-y: auto;
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

		&__item {
			margin-bottom: $spacing-xs;
		}

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
			&--active {
				background-color: rgba($color-primary, 0.05);
				color: $color-primary;
				font-weight: $font-weight-semibold;
			}
		}
		&__link-icon {
			display: inline-flex;
			width: 20px;
			height: 20px;
			align-items: center;
			justify-content: center;
			flex-shrink: 0;
		}
		&__subnav {
			list-style: none;
			padding: $spacing-xs 0 0 $spacing-lg;
			margin: $spacing-xs 0 0 0;
			border-left: 2px solid $color-border-light;
			margin-left: $spacing-xs + 2px;
		}

		&__subitem {
			margin-bottom: $spacing-xs * 0.5;
		}

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

			&--active {
				background-color: rgba($color-primary, 0.1);
				color: $color-primary;
				font-weight: $font-weight-medium;
			}
		}

		&__sublink-icon {
			display: inline-flex;
			width: 16px;
			height: 16px;
			align-items: center;
			justify-content: center;
			flex-shrink: 0;
			opacity: 0.8;
		}
	}
</style>
