<script lang="ts">
	import { page } from '$app/stores';
	import Button from '$components/elements/typography/Button.svelte'; // Verify path
	// Reuse StatCard or create a more specific InfoCard if needed
	import StatCard from '$components/dashboard/StatCard.svelte'; // Verify path
	import { goto } from '$app/navigation';

	// --- Get Session ID ---
	let { session_id } = $page.params;

	// --- Dummy Data (Replace with actual data fetched based on session_id) ---
	// This data would typically come from a load function or an API call
	let sessionDetails = $state({
		title: `Session ${session_id} Overview`,
		templateTitle: `Quiz Template (#t${session_id.substring(1)})`,
		status: 'Active',
		createdDate: new Date(Date.now() - 2 * 60 * 60 * 1000).toISOString(), // Example: 2 hours ago
		joinCode: 'XYZ123', // Example join code
		joinLink: `/join/${session_id}` // Example link
	});

	let sessionMetrics = $state({
		participants: 28,
		activitiesRun: 3,
		averageScore: 78, // Example metric
		questionsAsked: 15 // Example metric
	});

	let recentActivities = $state([
		{ id: 'act1', type: 'Poll', question: 'Topic A or B next?', time: '10 minutes ago' },
		{ id: 'act2', type: 'Quiz', question: 'Concept Check 1', time: '15 minutes ago' },
		{ id: 'act3', type: 'Rating', question: 'Pace of lecture?', time: '25 minutes ago' }
	]);

	// --- Handlers ---
	function copyJoinCode() {
		navigator.clipboard
			.writeText(sessionDetails.joinCode)
			.then(() => alert(`Code ${sessionDetails.joinCode} copied!`))
			.catch((err) => console.error('Failed to copy code:', err));
	}
	function copyJoinLink() {
		const fullLink = `${$page.url.origin}${sessionDetails.joinLink}`;
		navigator.clipboard
			.writeText(fullLink)
			.then(() => alert(`Link copied!`))
			.catch((err) => console.error('Failed to copy link:', err));
	}
	function viewFullAnalytics() {
		goto(`/sessions/${session_id}/analytics`);
	}
	function manageActivities() {
		goto(`/sessions/${session_id}/activities`);
	}

	// Placeholder Icons
	const IconUsers = () => '👥';
	const IconList = () => '📋';
	const IconCheck = () => '✅';
	const IconQuestion = () => '❓';
	const IconCopy = () => '📄';
	const IconLink = () => '🔗';
</script>

<svelte:head>
	<title>Overview: {sessionDetails.title} - EngaGenie</title>
</svelte:head>

<div class="session-overview-page">
	<section class="session-overview-page__info-section">
		<div class="info-card info-card--details">
			<h2 class="info-card__title">Session Details</h2>
			<dl class="info-card__list">
				<dt>Template:</dt>
				<dd>{sessionDetails.templateTitle}</dd>
				<dt>Status:</dt>
				<dd>{sessionDetails.status}</dd>
				<dt>Created:</dt>
				<dd>{new Date(sessionDetails.createdDate).toLocaleString()}</dd>
			</dl>
		</div>
		<div class="info-card info-card--join">
			<h2 class="info-card__title">Join Info</h2>
			<div class="info-card__join-code">
				<span>Join Code:</span>
				<strong>{sessionDetails.joinCode}</strong>
				<button onclick={copyJoinCode} class="info-card__copy-button" aria-label="Copy join code">
					{IconCopy()}
				</button>
			</div>
			<div class="info-card__join-link">
				<Button href={sessionDetails.joinLink}>
					{IconLink()} Join Link
				</Button>
				<button onclick={copyJoinLink} class="info-card__copy-button" aria-label="Copy join link">
					{IconCopy()}
				</button>
			</div>
		</div>
	</section>

	<section class="session-overview-page__metrics-section">
		<h2 class="section-title">Key Metrics</h2>
		<div class="metrics-grid">
			<StatCard title="Participants">
				<span class="metric-value">{sessionMetrics.participants}</span>
			</StatCard>
			<StatCard title="Activities Run">
				<span class="metric-value">{sessionMetrics.activitiesRun}</span>
			</StatCard>
			<StatCard title="Average Score">
				<span class="metric-value">{sessionMetrics.averageScore}%</span>
			</StatCard>
			<StatCard title="Questions Asked">
				<span class="metric-value">{sessionMetrics.questionsAsked}</span>
			</StatCard>
		</div>
		<div class="section-footer">
			<Button variant="outline" onclick={viewFullAnalytics}>View Full Analytics</Button>
		</div>
	</section>

	<section class="session-overview-page__recent-activity-section">
		<h2 class="section-title">Recent Activity</h2>
		<ul class="recent-activity-list">
			{#each recentActivities as activity (activity.id)}
				<li class="recent-activity-item">
					<span class="recent-activity-item__type">{activity.type}</span>
					<span class="recent-activity-item__question">{activity.question}</span>
					<span class="recent-activity-item__time">{activity.time}</span>
				</li>
			{:else}
				<li class="recent-activity-item--empty">No activities run yet.</li>
			{/each}
		</ul>
		<div class="section-footer">
			<Button variant="outline" onclick={manageActivities}>Manage Activities</Button>
		</div>
	</section>
</div>

<style lang="scss">
	@import '../../../../../styles/variables.scss'; // Adjust path depth

	// Block: session-overview-page
	.session-overview-page {
		display: flex;
		flex-direction: column;
		gap: $spacing-xl; // Space between sections
	}

	// --- Section: Info ---
	.session-overview-page__info-section {
		display: grid;
		grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
		gap: $spacing-lg;
	}

	// Block: info-card (Used within info section)
	.info-card {
		background-color: $color-surface;
		border-radius: $border-radius-lg;
		padding: $spacing-lg;
		border: 1px solid $color-border-light;
		box-shadow: $box-shadow-sm;

		// Element: Title
		&__title {
			font-size: $font-size-lg;
			font-weight: $font-weight-semibold;
			margin-top: 0;
			margin-bottom: $spacing-md;
			color: $color-text-primary;
		}

		// Element: Definition List for details
		&__list {
			font-size: $font-size-sm;
			dt {
				color: $color-text-secondary;
				float: left;
				clear: left;
				width: 100px; // Adjust width as needed
				margin-right: $spacing-sm;
				margin-bottom: $spacing-xs;
				font-weight: $font-weight-medium;
			}
			dd {
				color: $color-text-primary;
				margin-left: 110px; // Corresponds to dt width + margin
				margin-bottom: $spacing-xs;
			}
		}

		// Element: Join Code Area
		&__join-code {
			display: flex;
			align-items: center;
			gap: $spacing-md;
			font-size: $font-size-md;
			margin-bottom: $spacing-sm;

			span {
				color: $color-text-secondary;
			}
			strong {
				font-size: $font-size-xl;
				font-weight: $font-weight-bold;
				color: $color-primary;
				background-color: $color-surface-alt;
				padding: $spacing-xs $spacing-sm;
				border-radius: $border-radius-md;
			}
		}
		// Element: Join Link Area
		&__join-link {
			display: flex;
			align-items: center;
			gap: $spacing-sm;
		}

		// Element: Copy Button (common style)
		&__copy-button {
			background: none;
			border: none;
			padding: $spacing-xs;
			margin: -$spacing-xs;
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
			svg {
				width: 16px;
				height: 16px;
			}
		}
	}

	// --- Section: Metrics ---
	.session-overview-page__metrics-section {
		.section-title {
			/* Defined below */
		}
		.metrics-grid {
			display: grid;
			grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
			gap: $spacing-lg;
		}
		.metric-value {
			// Style for the large number in StatCard
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			color: $color-text-primary;
			display: block; // Ensure it takes space
			text-align: center; // Center the metric value
			margin-top: $spacing-sm;
		}
		.section-footer {
			/* Defined below */
		}
	}

	// --- Section: Recent Activity ---
	.session-overview-page__recent-activity-section {
		.section-title {
			/* Defined below */
		}
		.recent-activity-list {
			list-style: none;
			padding: 0;
			margin: 0;
			background-color: $color-surface;
			border-radius: $border-radius-md;
			border: 1px solid $color-border-light;
			box-shadow: $box-shadow-sm;
		}
		.section-footer {
			/* Defined below */
		}
	}

	// Block: recent-activity-item (within list)
	.recent-activity-item {
		display: flex;
		align-items: center;
		gap: $spacing-md;
		padding: $spacing-sm $spacing-md;
		border-bottom: 1px solid $color-border-light;
		font-size: $font-size-sm;

		&:last-child {
			border-bottom: none;
		}

		// Element: Type badge
		&__type {
			background-color: $color-surface-alt;
			color: $color-text-secondary;
			padding: $spacing-xs $spacing-sm;
			border-radius: $border-radius-sm;
			font-weight: $font-weight-medium;
			white-space: nowrap;
		}
		// Element: Question text
		&__question {
			flex-grow: 1; // Take available space
			color: $color-text-primary;
			white-space: nowrap;
			overflow: hidden;
			text-overflow: ellipsis;
		}
		// Element: Time ago text
		&__time {
			color: $color-text-secondary;
			font-size: $font-size-xs;
			white-space: nowrap;
			flex-shrink: 0;
		}

		// Modifier: Empty state
		&--empty {
			padding: $spacing-lg;
			text-align: center;
			color: $color-text-secondary;
			font-style: italic;
			border-bottom: none;
		}
	}

	// --- Common Section Elements ---
	.section-title {
		font-size: $font-size-xl;
		font-weight: $font-weight-semibold;
		margin-bottom: $spacing-md;
		color: $color-text-primary;
	}
	.section-footer {
		margin-top: $spacing-lg;
		text-align: right; // Align buttons right
	}
</style>
