<script lang="ts">
	import Sidebar from '$components/dashboard/Sidebar.svelte';
	import UserProfileBadge from '$components/dashboard/UserProfileBadge.svelte';

	// Dummy user data - replace with actual auth data later
	let userInitials = 'MF';
</script>

<div class="dashboard-layout">
	<Sidebar />

	<div class="dashboard-layout__main">
		<header class="dashboard-layout__topbar">
			<div class="dashboard-layout__topbar-spacer"></div>
			<UserProfileBadge initials={userInitials} />
		</header>

		<main class="dashboard-layout__content">
			<slot />
		</main>
	</div>
</div>

<style lang="scss">
	@import '../../styles/variables.scss';

	.dashboard-layout {
		display: flex;
		height: 100vh; // Full viewport height
		background-color: $color-background; // Main background for content area
		overflow: hidden; // Prevent body scroll if sidebar/content handles it
	}

	.dashboard-layout__main {
		flex-grow: 1; // Takes remaining width
		display: flex;
		flex-direction: column;
		height: 100vh; // Full height
		overflow-y: hidden; // Prevent this container from scrolling, content area will scroll
	}

	.dashboard-layout__topbar {
		display: flex;
		align-items: center;
		padding: $spacing-md $spacing-lg; // Padding for top bar
		background-color: $color-surface; // White background for top bar
		border-bottom: $border-width-thin solid $color-border-light;
		height: 60px; // Fixed height for top bar (adjust as needed)
		flex-shrink: 0; // Prevent shrinking
	}

	.dashboard-layout__topbar-spacer {
		flex-grow: 1; // Pushes user badge to the right
	}

	.dashboard-layout__content {
		flex-grow: 1; // Takes remaining vertical space
		padding: $spacing-xl; // Padding around the main page content
		overflow-y: auto; // Make the content area scrollable
	}
</style>
