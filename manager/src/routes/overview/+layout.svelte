<script lang="ts">
	import { page } from '$app/stores';
	import Sidebar from '$components/dashboard/Sidebar.svelte';
	import DashboardLayout from '$components/dashboard/DashboardLayout.svelte'; // Use the layout component
	import Topbar from '$components/dashboard/Topbar.svelte'; // Generic Topbar container
	// Import BOTH possible top bar contents
	import UserProfileBadge from '$components/dashboard/UserProfileBadge.svelte';
	import SessionTopbar from '$components/dashboard/SessionTopbar.svelte'; // Specific session topbar content

	// Dummy user data for default topbar
	let userInitials = 'MF';

	// Check if sessionDataForTopbar exists in the loaded page data
	let sessionData = $derived($page.data.sessionDataForTopbar);
</script>

<div class="overview-layout-wrapper">
	<Sidebar />

	<DashboardLayout>
		<svelte:fragment slot="topbar">
			<Topbar>
				{#if sessionData}
					<SessionTopbar
						sessionId={sessionData.id}
						sessionTitle={sessionData.title}
						sessionStatus={sessionData.status}
					/>
				{:else}
					<div class="overview-layout__topbar-spacer"></div>
					<UserProfileBadge initials={userInitials} />
				{/if}
			</Topbar>
		</svelte:fragment>
		<svelte:fragment slot="content">
			<slot />
		</svelte:fragment>
	</DashboardLayout>
</div>

<style lang="scss">
	// Styles for THIS layout file

	.overview-layout-wrapper {
		display: flex;
		height: 100vh;
		overflow: hidden;
	}

	// Style for the default topbar spacer (only used when sessionData is not present)
	.overview-layout__topbar-spacer {
		flex-grow: 1;
	}
</style>
