<script lang="ts">
	import { page } from '$app/stores';
	import Sidebar from '$components/dashboard/Sidebar.svelte';
	import DashboardLayout from '$components/dashboard/DashboardLayout.svelte';
	import Topbar from '$components/dashboard/Topbar.svelte';
	import UserProfileBadge from '$components/dashboard/UserProfileBadge.svelte';
	import SessionTopbar from '$components/dashboard/SessionTopbar.svelte';
	import { onMount } from 'svelte';
	import { goto } from '$app/navigation';
	import { getInitials } from '$lib/functions/utils';

	let userInitials = $state('XY');
	let sessionData = $derived($page.data.sessionDataForTopbar);

	onMount(async () => {
		const raw = localStorage.getItem('user');
		if (!raw) {
			console.warn('User not found in localStorage');
			goto('/login');
			return;
		}
		const user = JSON.parse(raw);
		userInitials = getInitials(user.name);
	});
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
	.overview-layout-wrapper {
		display: flex;
		height: 100vh;
		overflow: hidden;
	}

	.overview-layout__topbar-spacer {
		flex-grow: 1;
	}
</style>
