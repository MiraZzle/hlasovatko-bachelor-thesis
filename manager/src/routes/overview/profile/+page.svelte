<script lang="ts">
	import Button from '$components/elements/typography/Button.svelte'; // Verify path
	import Input from '$components/elements/typography/Input.svelte'; // Verify path
	import { goto } from '$app/navigation';
	import { tick } from 'svelte'; // Import tick for clipboard success message

	// --- Dummy User Data (Replace with actual fetched/store data) ---
	let currentUser = $state({
		name: 'Matěj Foukal', // Example Name
		email: 'matej.foukal@example.com', // Example Email
		initials: 'MF', // Used by parent layout's topbar
		// --- Dummy API Key ---
		apiKey: 'sk_live_abcdefghijklmnopqrstuvwxyz1234567890' // Example key
	});

	// --- State for Forms ---
	let updateName = $state(currentUser.name);
	let currentPassword = $state('');
	let newPassword = $state('');
	let confirmPassword = $state('');
	let isUpdatingProfile = $state(false);
	let isUpdatingPassword = $state(false);
	let profileUpdateError = $state<string | null>(null);
	let passwordUpdateError = $state<string | null>(null);
	let passwordUpdateSuccess = $state<string | null>(null);

	// --- State for API Key ---
	let isApiKeyVisible = $state(false);
	let isRegeneratingKey = $state(false);
	let apiKeyError = $state<string | null>(null);
	let copySuccessMessage = $state<string | null>(null);

	// --- Handlers ---
	async function handleUpdateProfile(event: SubmitEvent) {
		event.preventDefault();
		if (updateName.trim() === currentUser.name) return;

		isUpdatingProfile = true;
		profileUpdateError = null;
		console.log('Updating profile name to:', updateName.trim());

		// --- TODO: API Call to update profile ---
		await new Promise((resolve) => setTimeout(resolve, 800)); // Simulate network
		currentUser.name = updateName.trim(); // Simulate success
		isUpdatingProfile = false;
	}

	async function handleChangePassword(event: SubmitEvent) {
		event.preventDefault();
		passwordUpdateError = null;
		passwordUpdateSuccess = null;

		if (!currentPassword || !newPassword || !confirmPassword) {
			passwordUpdateError = 'Please fill in all password fields.';
			return;
		}
		if (newPassword !== confirmPassword) {
			passwordUpdateError = 'New passwords do not match.';
			return;
		}
		if (newPassword.length < 8) {
			passwordUpdateError = 'New password must be at least 8 characters long.';
			return;
		}

		isUpdatingPassword = true;
		console.log('Attempting password change...');

		// --- TODO: API Call to change password ---
		await new Promise((resolve) => setTimeout(resolve, 1000)); // Simulate network

		passwordUpdateSuccess = 'Password updated successfully!'; // Simulate success
		currentPassword = '';
		newPassword = '';
		confirmPassword = '';
		isUpdatingPassword = false;
	}

	function handleLogout() {
		console.log('Logging out...');
		// --- TODO: Implement actual logout logic ---
		alert('Logged out (Placeholder)');
		goto('/login');
	}

	// --- API Key Handlers ---
	function toggleApiKeyVisibility(): void {
		isApiKeyVisible = !isApiKeyVisible;
		copySuccessMessage = null; // Clear copy message
	}

	async function copyApiKey(): Promise<void> {
		copySuccessMessage = null;
		if (!navigator.clipboard) {
			alert('Clipboard API not available in this browser.');
			return;
		}
		try {
			await navigator.clipboard.writeText(currentUser.apiKey);
			copySuccessMessage = 'API Key copied!';
			await tick(); // Ensure DOM updates
			setTimeout(() => {
				copySuccessMessage = null;
			}, 2500); // Hide after 2.5s
		} catch (err) {
			console.error('Failed to copy API Key:', err);
			alert('Failed to copy API Key to clipboard.');
		}
	}

	async function regenerateApiKey(): Promise<void> {
		if (
			!confirm(
				'Are you sure you want to regenerate your API key? Your current key will stop working immediately.'
			)
		) {
			return;
		}

		isRegeneratingKey = true;
		apiKeyError = null;
		copySuccessMessage = null; // Clear copy message
		console.log('Regenerating API key...');

		// --- TODO: API Call to regenerate key ---
		await new Promise((resolve) => setTimeout(resolve, 1000)); // Simulate network

		// Example success: Update the key
		const newKey = `sk_live_${Math.random().toString(36).substring(2)}${Date.now().toString(36)}`;
		currentUser.apiKey = newKey;
		isApiKeyVisible = false; // Hide the new key
		alert('API Key regenerated successfully! Please copy the new key.');
		// Example error:
		// apiKeyError = 'Failed to regenerate API key. Please try again.';

		isRegeneratingKey = false;
	}
</script>

<svelte:head>
	<title>My Profile - EngaGenie</title>
</svelte:head>

<div class="profile-page">
	<h1 class="profile-page__title">My Profile</h1>

	<section class="profile-page__section profile-card">
		<h2 class="profile-card__title">Account Information</h2>
		<div class="profile-card__info-grid">
			<div class="profile-card__info-item">
				<span class="profile-card__label">Name</span>
				<span class="profile-card__value">{currentUser.name}</span>
			</div>
			<div class="profile-card__info-item">
				<span class="profile-card__label">Email</span>
				<span class="profile-card__value">{currentUser.email}</span>
			</div>
		</div>
	</section>

	<section class="profile-page__section profile-card">
		<h2 class="profile-card__title">Update Profile</h2>
		<form class="profile-card__form" onsubmit={handleUpdateProfile}>
			{#if profileUpdateError}
				<p class="profile-card__message profile-card__message--error">{profileUpdateError}</p>
			{/if}
			<Input
				label="Full Name"
				id="profile-name"
				bind:value={updateName}
				required
				disabled={isUpdatingProfile}
			/>
			<div class="profile-card__actions">
				<Button
					type="submit"
					variant="primary"
					disabled={isUpdatingProfile || updateName.trim() === currentUser.name}
				>
					{#if isUpdatingProfile}
						Saving...
					{:else}
						Save Name
					{/if}
				</Button>
			</div>
		</form>
	</section>

	<section class="profile-page__section profile-card">
		<h2 class="profile-card__title">Change Password</h2>
		<form class="profile-card__form" onsubmit={handleChangePassword}>
			{#if passwordUpdateError}
				<p class="profile-card__message profile-card__message--error">{passwordUpdateError}</p>
			{/if}
			{#if passwordUpdateSuccess}
				<p class="profile-card__message profile-card__message--success">{passwordUpdateSuccess}</p>
			{/if}
			<Input
				label="Current Password"
				id="current-password"
				type="password"
				bind:value={currentPassword}
				required
				disabled={isUpdatingPassword}
			/>
			<Input
				label="New Password"
				id="new-password"
				type="password"
				bind:value={newPassword}
				required
				disabled={isUpdatingPassword}
			/>
			<Input
				label="Confirm New Password"
				id="confirm-password"
				type="password"
				bind:value={confirmPassword}
				required
				disabled={isUpdatingPassword}
			/>
			<div class="profile-card__actions">
				<Button type="submit" variant="primary" disabled={isUpdatingPassword}>
					{#if isUpdatingPassword}
						Updating...
					{:else}
						Update Password
					{/if}
				</Button>
			</div>
		</form>
	</section>

	<section class="profile-page__section profile-card">
		<h2 class="profile-card__title">API Key</h2>
		<p class="profile-card__description">
			Use this key to interact with the EngaGenie API. Keep it secure!
		</p>
		{#if apiKeyError}
			<p class="profile-card__message profile-card__message--error">{apiKeyError}</p>
		{/if}

		<div class="api-key-display">
			<span class="api-key-display__label">Your Key:</span>
			{#if isApiKeyVisible}
				<code class="api-key-display__value">{currentUser.apiKey}</code>
			{:else}
				<code class="api-key-display__value api-key-display__value--obscured"
					>sk_live_****************************************</code
				>
			{/if}
			<Button variant="outline" size="sm" onclick={toggleApiKeyVisibility}>
				{isApiKeyVisible ? 'Hide' : 'Show'}
			</Button>
			<Button variant="secondary" size="sm" onclick={copyApiKey} disabled={!currentUser.apiKey}>
				Copy
			</Button>
		</div>
		{#if copySuccessMessage}
			<p class="profile-card__message profile-card__message--success api-key-display__copy-success">
				{copySuccessMessage}
			</p>
		{/if}

		<div class="profile-card__actions api-key-actions">
			<Button variant="danger-outline" onclick={regenerateApiKey} disabled={isRegeneratingKey}>
				{#if isRegeneratingKey}
					Regenerating...
				{:else}
					Regenerate Key
				{/if}
			</Button>
		</div>
	</section>

	<section class="profile-page__section profile-page__section--logout">
		<Button variant="danger" onclick={handleLogout}>Log Out</Button>
	</section>
</div>

<style lang="scss">
	@import '../../../styles/variables.scss'; // Adjust path

	// Block: profile-page
	.profile-page {
		display: flex;
		flex-direction: column;
		gap: $spacing-xl;

		&__title {
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			margin-bottom: 0;
		}

		&__section {
			&--logout {
				background: none;
				border: none;
				box-shadow: none;
				padding: 0;
				margin-top: $spacing-lg;
				display: flex;
				justify-content: flex-start;
				gap: $spacing-md;
			}
		}
	}

	// Block: profile-card
	.profile-card {
		background-color: $color-surface;
		border-radius: $border-radius-lg;
		padding: $spacing-xl;
		border: 1px solid $color-border-light;
		box-shadow: $box-shadow-sm;

		&__title {
			font-size: $font-size-lg;
			font-weight: $font-weight-semibold;
			margin: 0 0 $spacing-lg 0;
			padding-bottom: $spacing-sm;
			border-bottom: 1px solid $color-border-light;
		}

		&__description {
			font-size: $font-size-sm;
			color: $color-text-secondary;
			margin-top: -$spacing-sm;
			margin-bottom: $spacing-lg;
		}

		&__info-grid {
			display: grid;
			grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
			gap: $spacing-md $spacing-lg;
		}

		&__info-item {
			display: flex;
			flex-direction: column;
			gap: $spacing-xs * 0.5;
		}

		&__label {
			font-size: $font-size-xs;
			color: $color-text-secondary;
			text-transform: uppercase;
			font-weight: $font-weight-medium;
		}

		&__value {
			font-size: $font-size-md;
			color: $color-text-primary;
		}

		&__form {
			display: flex;
			flex-direction: column;
			gap: $spacing-lg;
		}

		&__actions {
			display: flex;
			justify-content: flex-end;
			margin-top: $spacing-sm;
		}

		&__message {
			padding: $spacing-sm $spacing-md;
			border-radius: $border-radius-md;
			font-size: $font-size-sm;
			margin-bottom: $spacing-sm;

			&--error {
				background-color: rgba($color-error, 0.1);
				color: $color-error;
				border: 1px solid rgba($color-error, 0.2);
			}
			&--success {
				background-color: rgba($color-success, 0.1);
				color: darken($color-success, 10%);
				border: 1px solid rgba($color-success, 0.2);
			}
		}
	}

	// Styles for API Key Section
	.api-key-display {
		display: flex;
		align-items: center;
		gap: $spacing-sm;
		background-color: $color-surface-alt;
		padding: $spacing-sm $spacing-md;
		border-radius: $border-radius-md;
		flex-wrap: wrap;

		&__label {
			font-size: $font-size-sm;
			font-weight: $font-weight-medium;
			color: $color-text-secondary;
			flex-shrink: 0;
		}

		&__value {
			font-family: monospace;
			font-size: $font-size-sm;
			color: $color-text-primary;
			background-color: darken($color-surface-alt, 3%);
			padding: $spacing-xs $spacing-sm;
			border-radius: $border-radius-sm;
			word-break: break-all;
			flex-grow: 1;

			&--obscured {
				color: $color-text-secondary;
				font-style: italic;
			}
		}

		&__toggle,
		&__copy {
			flex-shrink: 0;
		}

		&__copy-success {
			margin-top: $spacing-sm;
			margin-bottom: 0;
			width: 100%;
			text-align: right;
		}
	}

	.api-key-actions {
		justify-content: flex-start;
		border-top: none;
		padding-top: 0;
		margin-top: $spacing-md;
	}
</style>
