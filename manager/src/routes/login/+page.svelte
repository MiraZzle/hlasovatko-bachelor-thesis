<script lang="ts">
	import { goto } from '$app/navigation';
	import Button from '$components/elements/typography/Button.svelte';
	import Input from '$components/elements/typography/Input.svelte';
	import JoinSession from '$components/sections/JoinSession.svelte';

	// --- State for Login Form ---
	let email = $state('');
	let password = $state('');
	let isLoading = $state(false);
	let error = $state<string | null>(null);

	// --- Dummy Login Handler ---
	async function handleLogin(event: SubmitEvent): Promise<void> {
		event.preventDefault();
		isLoading = true;
		error = null;
		console.log('Attempting login with:', { email, password });
		await new Promise((resolve) => setTimeout(resolve, 1000));
		if (email === 'test@example.com' && password === 'password') {
			console.log('Login successful!');
			alert('Login successful! (Redirect placeholder)');
			// await goto('/dashboard');
		} else {
			console.error('Login failed!');
			error = 'Invalid email or password. Please try again.';
		}
		isLoading = false;
	}

	// --- Optional: Add link handler ---
	function goToSignUp(): void {
		goto('/signup');
	}
</script>

<svelte:head>
	<title>Sign In - EngaGenie</title>
	<meta name="description" content="Sign in to your EngaGenie account or join an activity." />
</svelte:head>

<div class="login-page-wrapper">
	<div class="login-page__grid">
		<section class="login-page__join-column" aria-labelledby="join-heading">
			<div class="login-page__content-wrapper">
				<h2 id="join-heading" class="login-page__section-title">Join an Activity Instantly</h2>
				<JoinSession />
			</div>
		</section>

		<section class="login-page__form-column" aria-labelledby="signin-heading">
			<div class="login-page__content-wrapper">
				<h1 id="signin-heading" class="login-page__form-title">Sign in!</h1>

				<div class="login-card">
					<form class="login-card__form" onsubmit={handleLogin}>
						{#if error}
							<div class="login-card__error-message" role="alert">
								{error}
							</div>
						{/if}
						<div class="login-card__field">
							<Input
								label="Email *"
								type="email"
								name="email"
								id="login-email"
								placeholder="you@example.com"
								bind:value={email}
								required
								disabled={isLoading}
							/>
						</div>
						<div class="login-card__field">
							<Input
								label="Password *"
								type="password"
								name="password"
								id="login-password"
								placeholder="Enter your password"
								bind:value={password}
								required
								disabled={isLoading}
							/>
						</div>
						<Button type="submit" variant="primary" fullWidth disabled={isLoading}>
							{#if isLoading}
								Signing In...
							{:else}
								Sign In
							{/if}
						</Button>
					</form>
					<p class="login-card__signup-link">
						Don't have an account?
						<button type="button" class="link-button" onclick={goToSignUp}> Create Account </button>
					</p>
				</div>
			</div>
		</section>
	</div>
</div>

<style lang="scss">
	@import '../../styles/variables.scss';

	.login-page-wrapper {
		// Occupies full viewport height, acts as parent for the grid
		min-height: 100vh;
		width: 100%;
		display: flex; // Use flex just to make the grid child take full height easily
	}

	.login-page__grid {
		flex-grow: 1; // Make grid take up all space from wrapper
		display: grid;
		grid-template-columns: 1fr; // Mobile first: single column layout

		// Desktop: Two-column layout
		@media (min-width: $breakpoint-md) {
			grid-template-columns: 1fr 1fr; // Two equal columns
		}
	}

	.login-page__join-column {
		// Left column styling
		// background-color: $color-background; // The desired gray background
		padding: $spacing-3xl $spacing-lg; // Padding inside the column
		display: flex; // Center content vertically and horizontally within the column
		flex-direction: column;
		justify-content: center;
		align-items: center;
		order: 1; // Ensure correct order on mobile

		@media (max-width: $breakpoint-md) {
			padding: $spacing-2xl $spacing-lg; // Adjust padding on mobile if needed
		}
	}

	.login-page__form-column {
		// Right column styling
		background-color: $color-surface; // Default surface color (likely white)
		padding: $spacing-3xl $spacing-lg; // Padding inside the column
		display: flex; // Center content vertically and horizontally
		flex-direction: column;
		justify-content: center;
		align-items: center;
		order: 2; // Ensure correct order on mobile
		min-height: 400px; // Ensure minimum height, especially on mobile

		@media (max-width: $breakpoint-md) {
			padding: $spacing-2xl $spacing-lg;
		}
	}

	// Wrapper for content within each column to control max-width
	.login-page__content-wrapper {
		width: 100%;
		max-width: 500px; // <<< ADJUSTED: Increased max-width for content blocks
	}

	// Title for "Join Activity"
	.login-page__section-title {
		text-align: left;
		font-size: $font-size-xl;
		font-weight: $font-weight-semibold;
		margin-bottom: $spacing-lg;
		color: $color-text-primary; // Check contrast against $color-background
		width: 100%; // Take full width of the wrapper
	}

	// Title for "Sign In!" - Now outside the card
	.login-page__form-title {
		text-align: left;
		font-size: $font-size-3xl;
		font-weight: $font-weight-bold;
		margin-bottom: $spacing-xl; // Space below title, above card
		color: $color-text-primary;
		width: 100%; // Take full width of the wrapper
	}

	// Adjust JoinSession component appearance within this context
	:global(.join-session) {
		width: 100%; // Make JoinSession take full width of its wrapper
		max-width: none; // Remove any internal max-width it might have
		padding: 0; // Remove internal padding if desired
		background-color: transparent; // Ensure background doesn't conflict
	}
	:global(.join-session__form) {
		max-width: none; // Ensure form inside can expand
	}

	// Card container for the login form
	.login-card {
		background-color: $color-surface;
		padding: $spacing-xl;
		border-radius: $border-radius-lg;
		box-shadow: $box-shadow-md;
		width: 100%; // Takes full width of its content-wrapper
		max-width: none; // No specific max-width here, controlled by wrapper
		margin: 0;

		@media (min-width: $breakpoint-md) {
			padding: $spacing-2xl; // Larger padding on desktop
		}
	}

	.login-card__form {
		display: flex;
		flex-direction: column;
		gap: $spacing-lg;
	}

	.login-card__error-message {
		background-color: rgba($color-error, 0.1);
		color: $color-error;
		border: 1px solid rgba($color-error, 0.3);
		border-radius: $border-radius-md;
		padding: $spacing-sm $spacing-md;
		font-size: $font-size-sm;
		text-align: center;
	}

	.login-card__signup-link {
		margin-top: $spacing-xl;
		text-align: center;
		font-size: $font-size-sm;
		color: $color-text-secondary;
	}

	.link-button {
		background: none;
		border: none;
		padding: 0;
		margin: 0;
		color: $color-link;
		text-decoration: underline;
		cursor: pointer;
		font-size: inherit;
		font-family: inherit;

		&:hover {
			color: $color-link-hover;
		}
	}
</style>
