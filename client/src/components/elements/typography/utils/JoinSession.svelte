<script lang="ts">
	// --- Imports ---
	import { goto } from '$app/navigation';
	import Button from '$components/elements/typography/Button.svelte';
	import Input from '$components/elements/typography/Input.svelte';

	// --- State ---
	let gameCode = $state('');
	let isLoading = $state(false);
	let error = $state<string | null>(null);

	// --- Handlers ---
	async function handleJoinSession(event: SubmitEvent): Promise<void> {
		event.preventDefault();
		const code = gameCode.trim().toUpperCase();

		if (!code) {
			error = 'Please enter a session code.';
			return;
		}
		isLoading = true;
		error = null;
		console.log(`Attempting to join session with code: ${code}`);
		await new Promise((resolve) => setTimeout(resolve, 750)); // Simulate network delay

		if (code.length === 6 && /^[A-Z0-9]+$/.test(code)) {
			// Dummy validation
			console.log('Code valid (simulation), navigating...');
			await goto(`/participate/${code}`);
		} else {
			console.error('Invalid session code (simulation)');
			error = `Invalid session code "${code}". Please check and try again.`;
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
	@import '../../../../styles/variables.scss'; // Adjust path if needed

	// Block: join-session
	.join-session {
		width: 100%;

		// Element: Container
		&__container {
			display: flex;
			justify-content: center;
		}

		// Element: Form
		&__form {
			display: flex;
			flex-direction: column;
			width: 100%;
			max-width: 500px;
			gap: $spacing-xs;
		}

		// Element: Input Group (Input + Button)
		&__input-group {
			display: flex;
			align-items: stretch; // Make input/button same height
			gap: $spacing-sm;
		}

		// Element: Error Message
		&__error {
			color: $color-error;
			font-size: $font-size-sm;
			text-align: left;
			width: 100%;
		}

		// --- Styling Child Components ---
		// Target Input component wrapper
		:global(.join-session__input-group .input-wrapper) {
			flex-grow: 1;
		}

		// Target Button component
		:global(.join-session__input-group .button) {
			flex-shrink: 0;
			// --- REMOVED explicit padding ---
			// padding-top: $spacing-md;
			// padding-bottom: $spacing-md;
			// Let the button's default padding and align-items: stretch handle height
		}
	}
</style>
