<script lang="ts">
	/**
	 * @file JoinSession component for allowing users to join a session using a game code.
	 */
	import Button from '$components/elements/typography/Button.svelte';
	import Input from '$components/elements/typography/Input.svelte';
	import { getParticipateSessionLinkWithCode } from '$lib/router/external_routes';
	import { toast } from '$lib/stores/toast_store';

	let gameCode = $state('');

	async function handleJoinSession(event: SubmitEvent): Promise<void> {
		event.preventDefault();
		if (!gameCode.trim()) {
			return;
		}
		try {
			const participateLink = await getParticipateSessionLinkWithCode(gameCode);
			window.location.href = participateLink;
		} catch (err) {
			toast.show(
				`Failed to join session: ${err instanceof Error ? err.message : 'Unknown error'}`,
				'error'
			);
			return;
		}
	}
</script>

<section class="join-session">
	<div class="join-session__container">
		<form class="join-session__form" onsubmit={handleJoinSession}>
			<Input
				name="sessionCode"
				placeholder="Enter Session code"
				ariaLabel="Enter Session code to join Session"
				bind:value={gameCode}
			/>
			<Button type="submit" variant="primary">Join Session</Button>
		</form>
	</div>
</section>

<style lang="scss">
	.join-session {
		&__container {
			display: flex;
			justify-content: center;
		}

		&__form {
			display: flex;
			gap: $spacing-sm;
			width: 100%;
			max-width: 500px;
			align-items: stretch;

			:global(.input-wrapper) {
				flex-grow: 1;
			}

			:global(.button) {
				flex-shrink: 0;
			}
		}
	}
</style>
