<script lang="ts">
	// --- Imports ---
	import { page } from '$app/stores';
	import { goto } from '$app/navigation';
	import Button from '$components/elements/typography/Button.svelte'; // Still needed for top bar
	// --- Import the new sidebar component ---
	import SessionSidebar from '$components/dashboard/SessionSidebar.svelte'; // Verify path
	import type { Snippet } from 'svelte'; // Needed for icon definitions
	import ChevronIcon from '$components/icons/ChevronIcon.svelte';

	// --- Route Parameter ---
	let { session_id } = $page.params;

	// --- Dummy Data (Load actual data as needed) ---
	let sessionTitle = $state(`Session ${session_id}`);
	let sessionStatus = $state<'Active' | 'Inactive' | 'Finished'>('Active');

	// --- Top Bar Handlers ---
	function stopSession() {
		console.log(`Stopping session ${session_id}...`);
		sessionStatus = 'Finished';
		// TODO: API call
	}
	function startSession() {
		console.log(`Starting session ${session_id}...`);
		sessionStatus = 'Active';
		// TODO: API call
	}

	// --- Sidebar Configuration ---
	// Placeholder icons
	const IconOverview = '📋';
	const IconActivities = '⚡';
	const IconAnalytics = '📈';
	const IconSettings = '⚙️';
	const IconBack = '🔙'; // Placeholder for back icon

	// Define links to pass to the sidebar component
	const sidebarLinks = [
		{ href: `/sessions/${session_id}/overview`, label: 'Overview', icon: IconOverview },
		{ href: `/sessions/${session_id}/activities`, label: 'Activities', icon: IconActivities },
		{ href: `/sessions/${session_id}/analytics`, label: 'Analytics', icon: IconAnalytics }
	];

	// --- Handlers passed to Sidebar ---
	function handleSidebarBack() {
		goto('/overview/sessions');
	}
	function handleSidebarShare(id: string) {
		console.log('Share action triggered from layout for session:', id);
		alert(`Sharing session ${id} (placeholder)`);
	}
	function handleSidebarPresent(id: string) {
		console.log('Present action triggered from layout for session:', id);
		alert(`Presenting session ${id} (placeholder)`);
	}
</script>

<div class="session-layout">
	<SessionSidebar
		sessionId={session_id}
		links={sidebarLinks}
		onBack={handleSidebarBack}
		onShare={handleSidebarShare}
		onPresent={handleSidebarPresent}
	/>

	<div class="session-layout__main">
		<header class="session-layout__topbar">
			<h1 class="session-layout__title">{sessionTitle}</h1>
			<span class="session-layout__status session-layout__status--{sessionStatus.toLowerCase()}">
				{sessionStatus}
			</span>
			<div class="session-layout__topbar-spacer"></div>
			{#if sessionStatus === 'Active'}
				<Button onclick={stopSession}>Stop Session</Button>
			{:else if sessionStatus === 'Inactive'}
				<Button onclick={startSession}>Start Session</Button>
			{/if}
		</header>

		<main class="session-layout__content">
			<slot />
		</main>
	</div>
</div>

<style lang="scss">
	@import '../../../styles/variables.scss'; // Adjust path depth

	// Block: session-layout (Overall container)
	.session-layout {
		display: flex;
		height: 100vh;
		background-color: $color-background;
		overflow: hidden;
	}

	// --- Sidebar styles are now in SessionSidebar.svelte ---

	// Block Element: Main content area wrapper
	.session-layout__main {
		flex-grow: 1;
		display: flex;
		flex-direction: column;
		height: 100vh;
		overflow-y: hidden;
	}

	// Block Element: Top Bar
	.session-layout__topbar {
		display: flex;
		align-items: center;
		padding: $spacing-md $spacing-lg;
		background-color: $color-surface;
		border-bottom: $border-width-thin solid $color-border-light;
		height: 60px;
		flex-shrink: 0;
		gap: $spacing-md;
	}

	// Block Element: Title in Top Bar
	.session-layout__title {
		font-size: $font-size-lg;
		font-weight: $font-weight-semibold;
		color: $color-text-primary;
		margin: 0;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
	}

	// Block Element: Status indicator in Top Bar
	.session-layout__status {
		display: inline-block;
		padding: $spacing-xs $spacing-sm;
		border-radius: $border-radius-pill;
		font-size: $font-size-xs;
		font-weight: $font-weight-medium;
		text-transform: uppercase;
		white-space: nowrap;
		margin-left: $spacing-sm;
		&--inactive {
			background-color: $color-surface-alt;
			color: $color-text-secondary;
		}
		&--active {
			background-color: rgba($color-success, 0.15);
			color: darken($color-success, 10%);
		}
		&--finished {
			background-color: darken($color-surface-alt, 5%);
			color: $color-text-disabled;
		}
	}

	// Block Element: Spacer in Top Bar
	.session-layout__topbar-spacer {
		flex-grow: 1;
	}

	// Block Element: Content Area
	.session-layout__content {
		flex-grow: 1;
		padding: $spacing-xl;
		overflow-y: auto;
	}
</style>
