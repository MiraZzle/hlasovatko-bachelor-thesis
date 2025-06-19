<script lang="ts">
	/**
	 * @file JoinSession component for joining an existing session using a code.
	 */
	import { goto } from '$app/navigation';
	import Button from '$components/elements/typography/Button.svelte';
	import Input from '$components/elements/typography/Input.svelte';
	import { getSessionIdByJoinCode } from '$lib/sessions/session_utils';

	let gameCode = $state('');
	let isLoading = $state(false);
	let error = $state<string | null>(null);

	async function handleJoinSession(event: SubmitEvent): Promise<void> {
		event.preventDefault();
		const code = gameCode.trim().toUpperCase();

		if (!code) {
			error = 'Please enter a session code.';
			return;
		}
		isLoading = true;
		error = null;
		try {
			const sessionId = getSessionIdByJoinCode(code);
			if (!sessionId) {
				error = 'Invalid session code. Please try again.';
				return;
			}
			await goto(`/participate/${sessionId}`);
		} catch (err) {
			console.error('Error joining session:', err);
			error = 'Failed to join session. Please try again later.';
		} finally {
			isLoading = false;
		}
	}
</script>

<section class="join-session">
	<div class="join-session__container">
		<form class="join-session__form" onsubmit={handleJoinSession}>
			{#if error}
				<p class="join-session__error" role="alert">{error}</p>
			{/if}
			<div class="join-session__input-group">
				<Input
					name="gameCode"
					placeholder="Enter game code"
					ariaLabel="Enter game code to join session"
					bind:value={gameCode}
					disabled={isLoading}
					required
					oninput={() => {
						error = null;
						gameCode = gameCode.toUpperCase();
					}}
				/>
				<Button type="submit" variant="primary" disabled={isLoading || !gameCode.trim()}>
					{#if isLoading}
						Joining...
					{:else}
						Join Session
					{/if}
				</Button>
			</div>
		</form>
	</div>
</section>

<style lang="scss">
	.join-session {
		width: 100%;

		&__container {
			display: flex;
			justify-content: center;
		}

		&__form {
			display: flex;
			flex-direction: column;
			width: 100%;
			max-width: 500px;
			gap: $spacing-xs;
		}

		&__input-group {
			display: flex;
			align-items: stretch;
			gap: $spacing-sm;
		}

		&__error {
			color: $color-error;
			font-size: $font-size-sm;
			text-align: left;
			width: 100%;
		}

		:global(.join-session__input-group .input-wrapper) {
			flex-grow: 1;
		}

		:global(.join-session__input-group .button) {
			flex-shrink: 0;
		}
	}
</style>
