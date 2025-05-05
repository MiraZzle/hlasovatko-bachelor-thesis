<script lang="ts">
	import Button from '$components/elements/typography/Button.svelte';
	import Input from '$components/elements/typography/Input.svelte';

	let gameCode = $state('');

	function handleJoinSession(event: SubmitEvent): void {
		event.preventDefault(); // Prevent default form submission
		if (!gameCode.trim()) {
			console.warn('Game code is empty');
			// Add user feedback (e.g., input border error state)
			return;
		}
		console.log('Joining session with code:', gameCode);
		// Dummy action: Navigate to session page or send request
		// Example navigation (requires SvelteKit's goto):
		// import { goto } from '$app/navigation';
		// goto(`/session/${gameCode}`);
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
			justify-content: center; // Center the form
		}

		&__form {
			display: flex;
			gap: $spacing-sm;
			width: 100%;
			max-width: 500px; // Limit form width
			align-items: stretch; // Make input and button same height if needed

			// Target Input component wrapper for flex sizing
			:global(.input-wrapper) {
				flex-grow: 1; // Allow input to take available space
			}

			// Target Button component directly
			:global(.button) {
				flex-shrink: 0; // Prevent button from shrinking
			}
		}
	}
</style>
