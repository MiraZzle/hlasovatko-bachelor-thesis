<script lang="ts">
	import Button from '$components/elements/typography/Button.svelte';
	import Input from '$components/elements/typography/Input.svelte';
	import { goto } from '$app/navigation';
	import { tick } from 'svelte';
	import { logout, changePassword, getPartialApiKey, regenerateApiKey } from '$lib/auth/auth';
	import { onMount } from 'svelte';
	import { getInitials } from '$lib/functions/utils';
	import type { User } from '$lib/auth/types';

	type CurrentUser = User & {
		initials: string;
		apiKey: string;
		partialApiKey: string;
	};

	let currentUser = $state<CurrentUser>({
		id: '',
		name: '',
		email: '',
		token: '',
		initials: '',
		apiKey: '',
		partialApiKey: ''
	});

	// State variables for password change
	let currentPassword = $state('');
	let newPassword = $state('');
	let confirmPassword = $state('');
	let isUpdatingPassword = $state(false);
	let passwordUpdateError = $state<string | null>(null);
	let passwordUpdateSuccess = $state<string | null>(null);
	let isApiKeyVisible = $state(false);
	let isRegeneratingKey = $state(false);
	let apiKeyError = $state<string | null>(null);
	let copySuccessMessage = $state<string | null>(null);

	/*
	 * Get the current user from localStorage and initialize the component.
	 * If user data is not found, redirect to login.
	 */
	onMount(async () => {
		const raw = localStorage.getItem('user');
		if (!raw) {
			console.warn('User not found in localStorage');
			goto('/login');
			return;
		}

		try {
			const user: User = JSON.parse(raw);
			const partial = await getPartialApiKey();

			currentUser = {
				...user,
				apiKey: '',
				partialApiKey: partial,
				initials: getInitials(user.name)
			};
		} catch (err) {
			console.error('Failed to initialize user:', err);
			apiKeyError = 'Unable to load user info.';
		}
	});

	function resetPasswordFields() {
		currentPassword = '';
		newPassword = '';
		confirmPassword = '';
	}

	/*
	 * Handles the password change form submission.
	 * Validates input, calls the changePassword function, and updates UI state accordingly.
	 */
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

		isUpdatingPassword = true;

		try {
			let success = await changePassword(currentPassword, newPassword);
			if (!success) {
				passwordUpdateError = 'Current password is incorrect.';
				isUpdatingPassword = false;
				return;
			} else {
				passwordUpdateSuccess = 'Password updated successfully!';
			}
		} catch (error) {
			console.error('Password change failed:', error);
			passwordUpdateError = 'Failed to update password. Please try again.';
			isUpdatingPassword = false;
			return;
		}

		resetPasswordFields();
		isUpdatingPassword = false;
	}

	function handleLogout() {
		logout();
		goto('/');
	}

	/**
	 * Copies the API key to the clipboard.
	 * Displays a success message forsome  time.
	 * Alerts if the API key is not available or if clipboard API is not supported.
	 */
	async function copyApiKey(): Promise<void> {
		if (!currentUser.apiKey) {
			alert('Full API key is not available. Please regenerate it first.');
			return;
		}

		copySuccessMessage = null;
		if (!navigator.clipboard) {
			alert('Clipboard API not available in this browser.');
			return;
		}
		try {
			await navigator.clipboard.writeText(currentUser.apiKey);
			copySuccessMessage = 'API Key copied!';
			await tick();
			setTimeout(() => {
				copySuccessMessage = null;
			}, 2500);
		} catch (err) {
			console.error('Failed to copy API Key:', err);
			alert('Failed to copy API Key to clipboard.');
		}
	}

	async function regenerateApiKeyHandler(): Promise<void> {
		if (!confirm('Are you sure you want to regenerate your API key?')) return;

		isRegeneratingKey = true;
		apiKeyError = null;
		copySuccessMessage = null;

		try {
			const newKey = await regenerateApiKey();
			currentUser.apiKey = newKey;
			currentUser.partialApiKey = await getPartialApiKey();
			isApiKeyVisible = true;

			alert('API Key regenerated successfully! Copy it now – it will not be shown again.');
		} catch (e) {
			apiKeyError = 'API key regeneration failed. Please try again.';
		} finally {
			isRegeneratingKey = false;
		}
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
				<code class="api-key-display__value">
					{currentUser.apiKey || currentUser.partialApiKey}
				</code>
			{:else}
				<code class="api-key-display__value api-key-display__value--obscured">
					{currentUser.partialApiKey || 'No API Key Available'}
				</code>
			{/if}
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
			<Button
				variant="danger-outline"
				onclick={regenerateApiKeyHandler}
				disabled={isRegeneratingKey}
			>
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
