<script lang="ts">
	import { goto } from '$app/navigation';
	import Button from '$components/elements/typography/Button.svelte'; // Verify path

	// --- Component Props ---
	// Expects data needed for the session top bar
	type Props = {
		sessionId: string;
		sessionTitle: string;
		sessionStatus: 'Active' | 'Inactive' | 'Finished';
		// Add any other needed data or handlers as props
	};

	let { sessionId, sessionTitle, sessionStatus }: Props = $props();

	// --- Handlers (These could also be passed as props if more complex) ---
	function stopSession() {
		console.log(`Stopping session ${sessionId}...`);
		// TODO: API call & update state (likely via parent)
		alert('Stopping session (placeholder)');
	}
	function startSession() {
		console.log(`Starting session ${sessionId}...`);
		// TODO: API call & update state (likely via parent)
		alert('Starting session (placeholder)');
	}
	function handleShare() {
		console.log('Sharing session:', sessionId);
		alert(`Sharing session ${sessionId} (placeholder)`);
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
	<Button onclick={stopSession}>Stop Session</Button>
{:else if sessionStatus === 'Inactive'}
	<Button onclick={startSession}>Start Session</Button>
{/if}

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

	// --- Styles for elements rendered by this component ---
	// Use the same BEM element names as defined in the session layout's style block
	// to ensure consistent styling within the top bar container.
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
	// --- End of styles ---
</style>
