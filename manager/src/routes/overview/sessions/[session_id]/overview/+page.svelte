<script lang="ts">
	/**
	 * @file Session Overview Page
	 * This page provides an overview of a specific session, including details, join info, and key metrics.
	 */
	import { page } from '$app/stores';
	import { onMount } from 'svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import StatCard from '$components/dashboard/StatCard.svelte';
	import { getSessionById } from '$lib/sessions/session_utils';
	import { getParticipateSessionLink, getManageSessionLink } from '$lib/router/external_routes';
	import { toast } from '$lib/stores/toast_store';

	type SessionDetails = {
		title: string;
		templateId: string;
		status: string;
		createdDate: string;
		joinCode?: string;
		manageLink: string;
		participateLink: string;
		activationDate?: string;
	};

	let sessionId = $page.params.session_id;

	let sessionDetails: SessionDetails | null = $state(null);
	let isLoading = $state(true);
	let error: string | null = $state(null);
	let notification = $state('');
	let manageUrl = $state(getManageSessionLink(sessionId));

	// Fetch data when the component is first mounted
	onMount(() => {
		async function loadSessionData() {
			isLoading = true;
			try {
				const sessionInfo = await getSessionById(sessionId);

				if (!sessionInfo) {
					throw new Error(`The session with ID '${sessionId}' could not be found.`);
				}

				// Map data to the shape our component needs
				sessionDetails = {
					title: sessionInfo.title,
					templateId: sessionInfo.templateID,
					status: sessionInfo.status,
					createdDate: sessionInfo.created,
					joinCode: sessionInfo.joinCode,
					manageLink: getManageSessionLink(sessionId),
					participateLink: getParticipateSessionLink(sessionId, sessionInfo.joinCode || ''),
					activationDate: sessionInfo.activationDate
				};
			} catch (err: any) {
				console.error('Failed to load session details:', err);
				error = err.message || 'An unknown error occurred.';
			} finally {
				isLoading = false;
			}
		}

		loadSessionData();
	});

	// Icons
	const IconCopy = () => '📄';
	const IconLink = () => '🔗';

	// Copy join code to clipboard
	function copyJoinCode() {
		if (!sessionDetails?.joinCode) return;

		navigator.clipboard
			.writeText(sessionDetails.joinCode)
			.then(() => toast.show(`Join code copied!`, `info`))
			.catch((err) => {
				console.error('Failed to copy code:', err);
				toast.show(`Failed to copy code.`, `error`);
			});
	}

	// Copy full link to clipboard
	function copyParticipateLink() {
		if (!sessionDetails?.participateLink) return;

		navigator.clipboard
			.writeText(sessionDetails.participateLink)
			.then(() => toast.show(`Participate link copied!`, `info`))
			.catch((err) => {
				console.error('Failed to copy link:', err);
				toast.show(`Failed to copy link.`, `error`);
			});
	}

	// Open share page in a new tab
	function openSharePage(): void {
		if (!sessionDetails?.joinCode) {
			toast.show(`Join code is not available.`, `error`);
			return;
		}

		const url = `/share?id=${sessionId}&code=${sessionDetails.joinCode}`;
		window.open(url, '_blank', 'noopener,noreferrer');
		toast.show(`Join info opened!`, `info`);
	}
</script>

<svelte:head>
	<title>EngaGenie | Session {sessionId} - Overview</title>
</svelte:head>

{#if sessionDetails}
	<div class="session-overview-page">
		<section class="session-overview-page__section">
			<div class="info-card info-card--details">
				<h2 class="info-card__title">Session Details</h2>
				<dl class="info-card__list">
					<dt class="info-card__term">Template:</dt>
					<dd class="info-card__definition">{sessionDetails.templateId}</dd>
					<dt class="info-card__term">Status:</dt>
					<dd class="info-card__definition">{sessionDetails.status}</dd>
					<dt class="info-card__term">Created:</dt>
					<dd class="info-card__definition">
						{new Date(sessionDetails.createdDate).toLocaleString()}
					</dd>
					{#if sessionDetails.status.toLowerCase() === 'planned' && sessionDetails.activationDate}
						<dt class="info-card__term">Planned for:</dt>
						<dd class="info-card__definition">
							{new Date(sessionDetails.activationDate).toLocaleString()}
						</dd>
					{/if}
				</dl>
			</div>
			<div class="info-card info-card--join">
				<h2 class="info-card__title">Join Info</h2>
				<div class="info-card__join-group">
					<span class="info-card__label">Join Code:</span>
					<strong class="info-card__code">{sessionDetails.joinCode}</strong>
					<button onclick={copyJoinCode} class="info-card__copy-button" aria-label="Copy join code">
						{IconCopy()}
					</button>
				</div>
				<div class="info-card__join-group">
					<Button href={manageUrl} variant="primary" fullWidth>Manage Session</Button>
				</div>
				<div class="info-card__join-group">
					<Button onclick={openSharePage} variant="secondary" fullWidth>Open Join Info</Button>
				</div>
				<div class="info-card__join-group">
					<Button onclick={copyParticipateLink} variant="outline" fullWidth>Copy Join Link</Button>
				</div>
			</div>
		</section>
	</div>
{:else}
	<div class="session-not-found">
		<h2 class="session-not-found__title">Session Not Found</h2>
		<p class="session-not-found__message">
			The session with ID '{sessionId}' could not be found.
		</p>
		<Button href="/dashboard">Go to Dashboard</Button>
	</div>
{/if}

<style lang="scss">
	.session-overview-page {
		display: flex;
		flex-direction: column;
		gap: $spacing-xl;

		&__section {
			display: grid;
			grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
			gap: $spacing-lg;
		}
	}

	.info-card {
		background-color: $color-surface;
		border-radius: $border-radius-lg;
		padding: $spacing-lg;
		border: 1px solid $color-border-light;
		box-shadow: $box-shadow-sm;

		&__title {
			font-size: $font-size-lg;
			font-weight: $font-weight-semibold;
			margin-top: 0;
			margin-bottom: $spacing-md;
			color: $color-text-primary;
		}

		&__list {
			font-size: $font-size-sm;
		}

		&__term {
			color: $color-text-secondary;
			float: left;
			clear: left;
			width: 100px;
			margin-right: $spacing-sm;
			margin-bottom: $spacing-xs;
			font-weight: $font-weight-medium;
		}

		&__definition {
			color: $color-text-primary;
			margin-left: 110px;
			margin-bottom: $spacing-xs;
		}

		&__join-group {
			display: flex;
			align-items: center;
			gap: $spacing-md;
			font-size: $font-size-md;
			&:not(:last-child) {
				margin-bottom: $spacing-sm;
			}
		}

		&__label {
			color: $color-text-secondary;
		}

		&__code {
			font-size: $font-size-xl;
			font-weight: $font-weight-bold;
			color: $color-primary;
			background-color: $color-surface-alt;
			padding: $spacing-xs $spacing-sm;
			border-radius: $border-radius-md;
		}

		&__copy-button {
			background: none;
			border: none;
			padding: $spacing-xs;
			margin-left: $spacing-xs;
			cursor: pointer;
			color: $color-text-secondary;
			border-radius: $border-radius-circle;
			display: inline-flex;
			align-items: center;
			justify-content: center;
			&:hover {
				background-color: $color-surface-alt;
				color: $color-primary;
			}
		}
	}

	.session-not-found {
		text-align: center;
		padding: $spacing-xl;

		&__message {
			margin-bottom: $spacing-lg;
		}
	}
</style>
