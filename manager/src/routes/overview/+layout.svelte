<script lang="ts">
	/**
	 * @file Overview Layout includes a sidebar, topbar, and content area.
	 */
	import { page } from '$app/stores';
	import Sidebar from '$components/dashboard/Sidebar.svelte';
	import DashboardLayout from '$components/dashboard/DashboardLayout.svelte';
	import Topbar from '$components/dashboard/Topbar.svelte';
	import UserProfileBadge from '$components/dashboard/UserProfileBadge.svelte';
	import SessionTopbar from '$components/dashboard/SessionTopbar.svelte';
	import TemplateTopbar from '$components/dashboard/TemplateTopbar.svelte';
	import { onMount } from 'svelte';
	import { goto } from '$app/navigation';
	import { getInitials } from '$lib/functions/utils';
	import Desktop from '$components/utils/Desktop.svelte';
	import Mobile from '$components/utils/Mobile.svelte';
	import Button from '$components/elements/typography/Button.svelte';

	let userInitials = $state('XY');
	let sessionData = $derived($page.data.sessionDataForTopbar);
	let templateData = $derived($page.data.templateDataForTopBar);

	let isSidebarOpen = $state(false);

	function toggleSidebar(): void {
		isSidebarOpen = !isSidebarOpen;
	}

	function closeSidebar(): void {
		isSidebarOpen = false;
	}

	/*
	 * Called on mount to retrieve user data from localStorage.
	 * If user data is not found, redirects to the login page.
	 */
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
	<Desktop>
		<Sidebar />
	</Desktop>

	{#if isSidebarOpen}
		<button
			class="sidebar-mobile-overlay"
			onclick={closeSidebar}
			tabindex="-1"
			aria-label="Close menu"
		></button>

		<div class="sidebar-mobile-container">
			<Sidebar />
		</div>
	{/if}

	<DashboardLayout>
		<svelte:fragment slot="topbar">
			<Topbar>
				<Mobile>
					<Button variant={'primary'} onclick={toggleSidebar}>☰</Button>
				</Mobile>
				{#if sessionData}
					<SessionTopbar
						sessionId={sessionData.id}
						sessionTitle={sessionData.title}
						sessionStatus={sessionData.status}
						joinCode={sessionData.joinCode}
					/>
				{:else if templateData}
					<TemplateTopbar templateId={templateData.id} templateTitle={templateData.title} />
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

	.sidebar-mobile-overlay {
		position: fixed;
		inset: 0;
		background-color: rgba(0, 0, 0, 0.5);
		z-index: 100;
		animation: fadeInSidebar 0.3s ease;
	}

	.sidebar-mobile-container {
		position: fixed;
		top: 0;
		left: 0;
		bottom: 0;
		z-index: 101;
		animation: slideInSidebar 0.3s ease-out;
	}

	@keyframes fadeInSidebar {
		from {
			opacity: 0;
		}
		to {
			opacity: 1;
		}
	}
	@keyframes slideInSidebar {
		from {
			transform: translateX(-100%);
		}
		to {
			transform: translateX(0);
		}
	}
</style>
