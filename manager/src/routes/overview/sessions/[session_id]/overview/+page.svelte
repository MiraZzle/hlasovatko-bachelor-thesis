<script lang="ts">
	import { page } from '$app/stores';
	import Button from '$components/elements/typography/Button.svelte';
	import StatCard from '$components/dashboard/StatCard.svelte';
	import { goto } from '$app/navigation';
	import { getSessionById, getSessionMetrics } from '$lib/sessions/session_utils';
	import { getParticipateSessionLink, getManageSessionLink } from '$lib/router/external_routes';
	let { session_id } = $page.params;

	function getFullSessionDetails() {
		let sessionInfo = getSessionById(session_id);

		if (!sessionInfo) {
			return null;
		}

		return {
			title: sessionInfo.title,
			templateId: sessionInfo.templateID,
			status: sessionInfo.status,
			createdDate: sessionInfo.created,
			joinCode: sessionInfo.joinCode,
			manageLink: getManageSessionLink(session_id),
			participateLink: getParticipateSessionLink(session_id)
		};
	}

	let sessionDetails = $derived(getFullSessionDetails());
	let sessionMetrics = $derived(getSessionMetrics(session_id));

	// Icons
	const IconCopy = () => '📄';
	const IconLink = () => '🔗';

	// Copy join code to clipboard
	function copyJoinCode() {
		if (!sessionDetails) {
			return;
		}
		navigator.clipboard
			.writeText(sessionDetails.joinCode)
			.then(() => alert(`Code ${sessionDetails.joinCode} copied!`))
			.catch((err) => console.error('Failed to copy code:', err));
	}

	// Copy full link to clipboard
	function copyParticipateLink() {
		if (!sessionDetails) {
			return;
		}
		navigator.clipboard
			.writeText(sessionDetails.participateLink)
			.then(() => alert(`Link copied!`))
			.catch((err) => console.error('Failed to copy link:', err));
	}
</script>

<svelte:head>
	<title>Overview: {sessionDetails?.title ?? 'Loading...'} - EngaGenie</title>
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
					<Button href={sessionDetails.participateLink}>
						{IconLink()} Join Link
					</Button>
					<button
						onclick={copyParticipateLink}
						class="info-card__copy-button"
						aria-label="Copy join link"
					>
						{IconCopy()}
					</button>
				</div>
			</div>
		</section>

		<section class="session-overview-page__section">
			<h2 class="session-overview-page__title">Key Metrics</h2>
			<div class="metrics-grid">
				<StatCard title="Participants">
					<span class="metrics-grid__value">{sessionMetrics.participants}</span>
				</StatCard>
				<StatCard title="Activities Run">
					<span class="metrics-grid__value">{sessionMetrics.activitiesRun}</span>
				</StatCard>
				<StatCard title="Answers Received">
					<span class="metrics-grid__value">{sessionMetrics.answersReceived}</span>
				</StatCard>
			</div>
		</section>
	</div>
{:else}
	<div class="session-not-found">
		<h2 class="session-not-found__title">Session Not Found</h2>
		<p class="session-not-found__message">
			The session with ID '{session_id}' could not be found.
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

		&__title {
			font-size: $font-size-xl;
			font-weight: $font-weight-semibold;
			margin-bottom: $spacing-md;
			color: $color-text-primary;
			grid-column: 1 / -1;
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

	.metrics-grid {
		display: grid;
		grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
		gap: $spacing-lg;
		grid-column: 1 / -1;

		&__value {
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			color: $color-text-primary;
			display: block;
			text-align: center;
			margin-top: $spacing-sm;
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
