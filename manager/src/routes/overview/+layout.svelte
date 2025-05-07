<script lang="ts">
	import Sidebar from '$components/dashboard/Sidebar.svelte'; // Verify path
	import UserProfileBadge from '$components/dashboard/UserProfileBadge.svelte'; // Verify path

	// Dummy user data - replace with actual auth data later
	let userInitials = 'MF';
</script>

<div class="dashboard-layout">
	<Sidebar />

	<div class="dashboard-layout__main">
		<header class="dashboard-layout__topbar">
			<slot name="topbar">
				<div class="dashboard-layout__topbar-spacer"></div>
				<UserProfileBadge initials={userInitials} />
			</slot>
		</header>

		<main class="dashboard-layout__content">
			<slot />
		</main>
	</div>
</div>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

	.dashboard-layout {
		display: flex;
		height: 100vh;
		background-color: $color-background;
		overflow: hidden;
	}

	.dashboard-layout__main {
		flex-grow: 1;
		display: flex;
		flex-direction: column;
		height: 100vh;
		overflow-y: hidden;
	}

	.dashboard-layout__topbar {
		// Container for the top bar content - provided by slot
		display: flex; // Use flex for alignment of slotted content
		align-items: center;
		padding: $spacing-md $spacing-lg;
		background-color: $color-surface;
		border-bottom: $border-width-thin solid $color-border-light;
		height: 60px;
		flex-shrink: 0;
		gap: $spacing-sm; // Add gap for slotted items
	}

	.dashboard-layout__topbar-spacer {
		// Only used in default slot content
		flex-grow: 1;
	}

	.dashboard-layout__content {
		flex-grow: 1;
		padding: $spacing-xl;
		overflow-y: auto;
		// Removed background-color here, should be set by child page/layout or body
	}
</style>
