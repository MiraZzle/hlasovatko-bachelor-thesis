<script lang="ts">
	// --- Imports ---
	import { page } from '$app/stores';
	import { goto } from '$app/navigation'; // Still needed for Present button
	import Button from '$components/elements/typography/Button.svelte'; // Verify path
	// No sidebar import needed here

	// --- Route Parameter ---
	let { session_id } = $page.params;

	// --- Dummy Data (Load actual data as needed, e.g., in a load function) ---
	let sessionTitle = $state(`Session ${session_id}`);
	let sessionStatus = $state<'Active' | 'Inactive' | 'Finished'>('Active'); // Example status

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
	function handleShare() {
		console.log('Sharing session:', session_id);
		alert(`Sharing session ${session_id} (placeholder)`);
	}
	function handlePresent() {
		console.log('Presenting session:', session_id);
		// Assuming the present page is within this layout group
		goto(`/sessions/${session_id}/present`);
	}
</script>

<div class="session-layout__main">
	<header class="session-layout__topbar">
		<h1 class="session-layout__title">{sessionTitle}</h1>
		<span class="session-layout__status session-layout__status--{sessionStatus.toLowerCase()}">
			{sessionStatus}
		</span>
		<div class="session-layout__topbar-spacer"></div>

		<Button variant="outline" onclick={handleShare}>Share</Button>
		<Button variant="primary" onclick={handlePresent}>Present</Button>

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

<style lang="scss">
	@import '../../../../styles/variables.scss'; // Adjust path depth

	// Styles for the elements defined *within this layout file*

	// Element: Main content area wrapper (within the parent layout's structure)
	.session-layout__main {
		flex-grow: 1; // Takes space from parent layout's flex container
		display: flex;
		flex-direction: column;
		height: 100vh; // Use viewport height for independent scrolling within its area
		overflow-y: hidden; // Prevent this container scrolling, content will scroll
		width: 100%; // Ensure it takes full width
	}

	// Element: Top Bar
	.session-layout__topbar {
		display: flex;
		align-items: center;
		padding: $spacing-md $spacing-lg;
		background-color: $color-surface;
		border-bottom: $border-width-thin solid $color-border-light;
		height: 60px; // Consistent height
		flex-shrink: 0;
		gap: $spacing-sm; // Consistent gap
	}

	// Element: Title in Top Bar
	.session-layout__title {
		font-size: $font-size-lg;
		font-weight: $font-weight-semibold;
		color: $color-text-primary;
		margin: 0;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
	}

	// Element: Status indicator in Top Bar
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

	// Element: Spacer in Top Bar
	.session-layout__topbar-spacer {
		flex-grow: 1;
	}

	// Element: Content Area
	.session-layout__content {
		flex-grow: 1;
		padding: $spacing-xl;
		overflow-y: auto; // Make content scrollable
		background-color: $color-background; // Set background for content area
	}
</style>
