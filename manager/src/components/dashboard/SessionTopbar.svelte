<script lang="ts">
	/**
	 * @file Reusable component for displaying the top bar of a session.
	 * Provides actions to start/stop the session and share it with participants.
	 */
	import { invalidateAll } from '$app/navigation';
	import Button from '$components/elements/typography/Button.svelte';
	import { startSession, stopSession } from '$lib/sessions/session_utils';
	import ShareSessionModal from '$components/elements/modals/ShareSessionModal.svelte';
	import { getManageSessionLink, getParticipateSessionLink } from '$lib/router/external_routes';
	import type { SessionStatus } from '$lib/sessions/types';

	let {
		sessionId,
		sessionTitle,
		sessionStatus,
		joinCode
	}: {
		sessionId: string;
		sessionTitle: string;
		sessionStatus: SessionStatus;
		joinCode: string;
	} = $props();

	let isShareModalOpen = $state(false);
	let isLoading = $state(false);
	let participateUrl = $state(getParticipateSessionLink(sessionId));
	let manageUrl = $state(getManageSessionLink(sessionId));

	async function handleStartSession(): Promise<void> {
		isLoading = true;
		try {
			await startSession(sessionId);
			isShareModalOpen = true;
			await invalidateAll(); // rerefresh the session data
		} catch (error: any) {
			console.error('Failed to start session:', error);
			alert(error.message);
		} finally {
			isLoading = false;
		}
	}

	async function handleStopSession(): Promise<void> {
		isLoading = true;
		try {
			await stopSession(sessionId);
			await invalidateAll();
		} catch (error: any) {
			console.error('Failed to stop session:', error);
			alert(error.message);
		} finally {
			isLoading = false;
		}
	}
</script>

<h1 class="session-layout__title">{sessionTitle}</h1>
<span class="session-layout__status session-layout__status--{sessionStatus.toLowerCase()}">
	{sessionStatus}
</span>
<div class="session-layout__topbar-spacer"></div>

{#if sessionStatus.toLowerCase() === 'active'}
	<Button variant="danger" onclick={handleStopSession} disabled={isLoading}>
		{isLoading ? 'Stopping...' : 'Stop Session'}
	</Button>
{:else if sessionStatus.toLowerCase() === 'inactive' || sessionStatus.toLowerCase() === 'planned'}
	<Button variant="primary" onclick={handleStartSession} disabled={isLoading}>
		{isLoading ? 'Starting...' : 'Start Session'}
	</Button>
{/if}

<ShareSessionModal
	open={isShareModalOpen}
	{participateUrl}
	{manageUrl}
	code={joinCode}
	onclose={() => (isShareModalOpen = false)}
/>

<style lang="scss">
	.session-layout__title {
		font-size: $font-size-lg;
		font-weight: $font-weight-semibold;
		color: $color-text-primary;
		margin: 0;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
		flex-shrink: 1;
		margin-right: $spacing-sm;
	}
	.session-layout__status {
		display: inline-block;
		padding: $spacing-xs $spacing-sm;
		border-radius: $border-radius-pill;
		font-size: $font-size-xs;
		font-weight: $font-weight-medium;
		text-transform: uppercase;
		white-space: nowrap;
		flex-shrink: 0;
		&--inactive,
		&--planned {
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
	.session-layout__topbar-spacer {
		flex-grow: 1;
	}
</style>
