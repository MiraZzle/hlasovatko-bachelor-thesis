<script lang="ts">
	/**
	 * @file Reusable component for displaying the top bar of a session.
	 * Provides actions to start/stop the session and share it with participants.
	 */
	import { goto } from '$app/navigation';
	import Button from '$components/elements/typography/Button.svelte';
	import { startSession, stopSession } from '$lib/functions/session_actions';
	import ShareSessionModal from '$components/elements/modals/ShareSessionModal.svelte';
	import { getManageSessionLink, getParticipateSessionLink } from '$lib/router/external_routes';
	import { get } from 'svelte/store';
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
	let participateUrl = $state(getParticipateSessionLink(sessionId));
	let manageUrl = $state(getManageSessionLink(sessionId));

	function handleStartSession(): void {
		isShareModalOpen = true;
		startSession(sessionId);
	}
</script>

<h1 class="session-layout__title">{sessionTitle}</h1>
<span class="session-layout__status session-layout__status--{sessionStatus.toLowerCase()}">
	{sessionStatus}
</span>
<div class="session-layout__topbar-spacer"></div>

{#if sessionStatus.toLowerCase() == 'active'}
	<Button variant="danger" onclick={() => stopSession(sessionId)}
		>Stop Session {sessionStatus}</Button
	>
{:else if sessionStatus.toLowerCase() == 'inactive'}
	<Button variant="primary" onclick={handleStartSession}>Start Session</Button>
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
	.session-layout__topbar-spacer {
		flex-grow: 1;
	}
</style>
