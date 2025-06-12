<script lang="ts">
	import Button from '$components/elements/typography/Button.svelte';
	import Input from '$components/elements/typography/Input.svelte';

	let gameCode = $state('');

	function handleJoinSession(event: SubmitEvent): void {
		event.preventDefault();
		if (!gameCode.trim()) {
			console.warn('Game code is empty');
			return;
		}
		console.log('Joining session with code:', gameCode);
		// TODO: redirect to manager with game code
		// goto(`${MANAGE_URL}/manager/sessions/${gameCode}`);
	}
</script>

<section class="join-session">
	<div class="join-session__container">
		<form class="join-session__form" onsubmit={handleJoinSession}>
			<Input
				name="gameCode"
				placeholder="Enter game code"
				ariaLabel="Enter game code to join session"
				bind:value={gameCode}
			/>
			<Button type="submit" variant="primary">Join Session</Button>
		</form>
	</div>
</section>

<style lang="scss">
	@import '../../styles/variables.scss';

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
