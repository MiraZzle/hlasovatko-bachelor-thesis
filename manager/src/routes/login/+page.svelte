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
		if (email === 'test@example.com' && password === '1234') {
			console.log('Login successful!');
			alert('Login successful! (Redirect placeholder)');
			await goto('/overview/templates');
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
		min-height: 100vh;
		width: 100%;
		display: flex;
	}

	// Block: login-page
	.login-page {
		&__grid {
			flex-grow: 1;
			display: grid;
			grid-template-columns: 1fr;

			@media (min-width: $breakpoint-md) {
				grid-template-columns: 1fr 1fr;
			}
		}

		&__join-column {
			// background-color: $color-background; // Uncomment if gray background desired
			padding: $spacing-3xl $spacing-lg;
			display: flex;
			flex-direction: column;
			justify-content: center;
			align-items: center;
			order: 1;

			@media (max-width: $breakpoint-md) {
				padding: $spacing-2xl $spacing-lg;
			}
		}

		&__form-column {
			background-color: $color-surface;
			padding: $spacing-3xl $spacing-lg;
			display: flex;
			flex-direction: column;
			justify-content: center;
			align-items: center;
			order: 2;
			min-height: 400px;

			@media (max-width: $breakpoint-md) {
				padding: $spacing-2xl $spacing-lg;
			}
		}

		&__content-wrapper {
			width: 100%;
			max-width: 500px;
		}

		&__section-title {
			text-align: left;
			font-size: $font-size-xl;
			font-weight: $font-weight-semibold;
			margin-bottom: $spacing-lg;
			color: $color-text-primary;
			width: 100%;
		}

		&__form-title {
			text-align: left;
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			margin-bottom: $spacing-xl;
			color: $color-text-primary;
			width: 100%;
		}

		// Necessary overrides for child component styles within this context
		:global(.join-session) {
			width: 100%;
			max-width: none;
			padding: 0;
		}
		:global(.join-session__form) {
			max-width: none;
		}
	}

	// Block: login-card
	.login-card {
		background-color: $color-surface;
		padding: $spacing-xl;
		border-radius: $border-radius-lg;
		box-shadow: $box-shadow-md;
		width: 100%;
		max-width: none;
		margin: 0;

		@media (min-width: $breakpoint-md) {
			padding: $spacing-2xl;
		}

		&__form {
			display: flex;
			flex-direction: column;
			gap: $spacing-lg;
		}

		&__field {
			// Currently no styles needed for this wrapper element
		}

		&__error-message {
			background-color: rgba($color-error, 0.1);
			color: $color-error;
			border: 1px solid rgba($color-error, 0.3);
			border-radius: $border-radius-md;
			padding: $spacing-sm $spacing-md;
			font-size: $font-size-sm;
			text-align: center;
		}

		&__signup-link {
			margin-top: $spacing-xl;
			text-align: center;
			font-size: $font-size-sm;
			color: $color-text-secondary;
		}
	}

	// Utility/Element: link-button
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
		&:focus-visible {
			// Added focus style
			outline: 2px solid $color-primary-light;
			outline-offset: 2px;
			border-radius: $border-radius-sm;
		}
	}
</style>
