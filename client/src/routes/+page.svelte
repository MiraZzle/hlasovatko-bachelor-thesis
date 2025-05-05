<script lang="ts">
	import { goto } from '$app/navigation';
	import Button from '$components/elements/typography/Button.svelte'; // Verify path
	import Input from '$components/elements/typography/Input.svelte'; // Verify path

	// --- State ---
	let sessionCode = $state('');
	let isLoading = $state(false);
	let error = $state<string | null>(null);

	// --- Handlers ---
	async function handleJoinSession(event: SubmitEvent): Promise<void> {
		event.preventDefault();
		const code = sessionCode.trim().toUpperCase(); // Normalize code

		if (!code) {
			error = 'Please enter a session code.';
			return;
		}

		isLoading = true;
		error = null;
		console.log(`Attempting to join session with code: ${code}`);

		// --- TODO: Implement API call to validate session code ---
		// Example simulation:
		await new Promise((resolve) => setTimeout(resolve, 750)); // Simulate network delay

		// Replace with actual validation logic
		if (code.length === 6 && /^[A-Z0-9]+$/.test(code)) {
			// Dummy validation: 6 alphanumeric chars
			console.log('Code valid (simulation), navigating...');
			// Navigate to the specific session participation page (adjust route as needed)
			// This route likely needs to be outside the (app) group if it doesn't use the dashboard layout
			await goto(`/participate/${code}`); // Example route
		} else {
			console.error('Invalid session code (simulation)');
			error = `Invalid session code "${code}". Please check and try again.`;
			isLoading = false;
		}
		// isLoading will remain true on successful navigation
	}
</script>

<svelte:head>
	<title>Join Session - EngaGenie</title>
	<meta name="description" content="Join an active EngaGenie session using a code." />
</svelte:head>

<div class="join-session-page">
	<div class="join-session-page__container">
		<div class="join-session-page__logo">EngaGenie</div>

		<h1 class="join-session-page__title">Join a Session</h1>
		<p class="join-session-page__instructions">Enter the code provided by your instructor.</p>

		<form class="join-session-page__form" onsubmit={handleJoinSession}>
			{#if error}
				<p class="join-session-page__error" role="alert">{error}</p>
			{/if}

			<Input
				name="sessionCode"
				id="session-code-input"
				placeholder="Enter code (e.g., ABC123)"
				bind:value={sessionCode}
				required
				disabled={isLoading}
				ariaLabel="Session Code"
				oninput={() => {
					error = null;
					sessionCode = sessionCode.toUpperCase();
				}}
			/>

			<Button type="submit" variant="primary" fullWidth disabled={isLoading || !sessionCode.trim()}>
				{#if isLoading}
					Joining...
				{:else}
					Join Session
				{/if}
			</Button>
		</form>
	</div>
</div>

<style lang="scss">
	@import '../styles/variables.scss'; // Adjust path if needed

	.join-session-page {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		min-height: 90vh;
		padding: $spacing-lg;
		background-color: $color-background;

		// Element: Container
		&__container {
			width: 100%;
			max-width: 400px;
			background-color: $color-surface;
			padding: $spacing-xl;
			border-radius: $border-radius-lg;
			box-shadow: $box-shadow-md;
			text-align: center;
		}

		// Element: Logo (Optional)
		&__logo {
			font-size: $font-size-2xl;
			font-weight: $font-weight-bold;
			color: $color-primary;
			margin-bottom: $spacing-lg;
		}

		// Element: Title
		&__title {
			font-size: $font-size-2xl;
			font-weight: $font-weight-semibold;
			margin-top: 0;
			margin-bottom: $spacing-xs;
			color: $color-text-primary;
		}

		// Element: Instructions
		&__instructions {
			font-size: $font-size-md;
			color: $color-text-secondary;
			margin-bottom: $spacing-xl;
		}

		// Element: Form
		&__form {
			display: flex;
			flex-direction: column;
			gap: $spacing-md;
		}

		// Element: Error Message
		&__error {
			color: $color-error;
			font-size: $font-size-sm;
			margin-bottom: $spacing-sm;
			text-align: left;
			background-color: rgba($color-error, 0.05);
			padding: $spacing-sm;
			border-radius: $border-radius-sm;
		}

		// --- Styling Child Components within this context ---
		// Use :global() carefully or ensure components accept className props
		:global(.input-wrapper__input) {
			// Example: If you wanted to style the input specifically here
			// text-align: center;
			// text-transform: uppercase;
		}
	}

	// Styling the Input/Button components if needed within this context
	// Example: Make input text larger/centered?
	// :global(.join-session-page__form .input-wrapper__input) {
	//     text-align: center;
	//     font-size: $font-size-lg;
	//     text-transform: uppercase; // Match the uppercase transform
	// }
</style>
