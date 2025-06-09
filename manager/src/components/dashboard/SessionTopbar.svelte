<script lang="ts">
	import { goto } from '$app/navigation';
	import Button from '$components/elements/typography/Button.svelte';
	import { startSession, stopSession } from '$lib/functions/session_actions';
	import ShareSessionModal from '$components/elements/ShareSessionModal.svelte';

	let {
		sessionId,
		sessionTitle,
		sessionStatus
	}: {
		sessionId: string;
		sessionTitle: string;
		sessionStatus: 'Active' | 'Inactive' | 'Finished';
	} = $props();

	let isShareModalOpen = $state(false);

	function handleShare() {
		isShareModalOpen = true;
	}
	function handlePresent() {
		console.log('Presenting session:', sessionId);
		goto(`/sessions/${sessionId}/present`);
	}
</script>

<h1 class="session-layout__title">{sessionTitle}</h1>
<span class="session-layout__status session-layout__status--{sessionStatus.toLowerCase()}">
	{sessionStatus}
</span>
<div class="session-layout__topbar-spacer"></div>

<Button variant="outline" onclick={handleShare}>Share</Button>
<Button variant="primary" onclick={handlePresent}>Present</Button>

{#if sessionStatus === 'Active'}
	<Button onclick={() => stopSession(sessionId)}>Stop Session</Button>
{:else if sessionStatus === 'Inactive'}
	<Button onclick={() => startSession(sessionId)}>Start Session</Button>
{/if}

<ShareSessionModal
	open={isShareModalOpen}
	url={`https://example.com/sessions/${sessionId}`}
	code={sessionId}
	onclose={() => (isShareModalOpen = false)}
/>

<style lang="scss">
	@import '../../styles/variables.scss';

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
