<script lang="ts">
	import Select from '$components/elements/typography/Select.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import StatCard from '$components/dashboard/StatCard.svelte';

	// Placeholder Icons (Replace with actual Svelte components or SVG imports)
	const IconBarChart = () => '<svg>📊</svg>'; // Placeholder
	const IconDocument = () => '<svg>📄</svg>'; // Placeholder
	const IconClock = () => '<svg>🕒</svg>'; // Placeholder
	const IconExport = () => '<svg>📤</svg>'; // Placeholder

	type TimeFrame = '7d' | '30d' | '90d' | 'all';

	// --- State ---
	let selectedTimeFrame = $state<TimeFrame>('7d');
	const timeFrameOptions: { value: TimeFrame; label: string }[] = [
		{ value: '7d', label: '7 days' },
		{ value: '30d', label: '30 days' },
		{ value: '90d', label: '90 days' },
		{ value: 'all', label: 'All time' }
	];

	// Dummy stats data (replace with actual fetched data based on selectedTimeFrame)
	let globalMetrics = $derived(() => ({
		sessions: 10,
		students: 34,
		answers: 139
	}));

	let activityBreakdown = $derived(() => [
		{ label: 'Polls', value: 62 },
		{ label: 'Word clouds', value: 24 },
		{ label: 'Ratings', value: 10 },
		{ label: 'Other', value: 4 }
	]);

	// --- Handlers ---
	function handleTimeFrameChange(event: Event & { currentTarget: HTMLSelectElement }) {
		// selectedTimeFrame is already updated via bind:value
		console.log('Time frame changed to:', selectedTimeFrame);
		// Re-fetch data based on selectedTimeFrame
	}

	function handleExport() {
		console.log('Exporting data for time frame:', selectedTimeFrame);
		// Trigger export logic
	}
</script>

<svelte:head>
	<title>Analytics Overview - EngaGenie</title>
</svelte:head>

<div class="analytics-page">
	<header class="analytics-page__header">
		<h1 class="analytics-page__title">Global Analytics Overview</h1>
		<p class="analytics-page__description">Insights into engagement across all your sessions.</p>
	</header>

	<div class="analytics-page__controls">
		<Select
			label="Time Frame"
			options={timeFrameOptions}
			bind:value={selectedTimeFrame}
			onchange={handleTimeFrameChange}
			ariaLabel="Select time frame for analytics"
		/>
		<Button variant="outline" onclick={handleExport}>Export</Button>
	</div>

	<div class="analytics-page__stats-grid">
		<StatCard title="Session Timeline">
			<div class="timeline-placeholder">Session timeline chart will be displayed here.</div>
		</StatCard>

		<StatCard title="Session Timeline">
			<div class="timeline-placeholder">Session timeline chart will be displayed here.</div>
		</StatCard>

		<StatCard title="Session Timeline">
			<div class="timeline-placeholder">Session timeline chart will be displayed here.</div>
		</StatCard>
	</div>

	<section class="analytics-page__raw-data">
		<h2 class="raw-data__title">Need raw data?</h2>
		<p class="raw-data__text">
			Use the public API. <a href="/docs/api" target="_blank" rel="noopener noreferrer"
				>[Read the Docs]</a
			>
		</p>
	</section>
</div>

<style lang="scss">
	@import '../../../styles/variables.scss';

	// Block: analytics-page
	.analytics-page {
		// No specific styles needed for the root, layout handled by parent slot padding

		// Element: Page Header
		&__header {
			margin-bottom: $spacing-xl;
		}

		// Element: Page Title
		&__title {
			font-size: $font-size-3xl; // Match other page titles if necessary
			font-weight: $font-weight-bold;
			margin-bottom: $spacing-xs;
		}

		// Element: Page Description
		&__description {
			font-size: $font-size-md;
			color: $color-text-secondary;
			max-width: 600px; // Limit description width
		}

		// Element: Controls Container
		&__controls {
			display: flex;
			justify-content: space-between;
			align-items: flex-end; // Align items to bottom if labels make heights differ
			margin-bottom: $spacing-xl;
			gap: $spacing-md;
			flex-wrap: wrap; // Allow wrapping on smaller screens
		}

		// Element: Stats Grid
		&__stats-grid {
			display: grid;
			// Responsive grid: 1 column mobile, 2 medium, 3 large? Or just 1 -> 3?
			grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
			gap: $spacing-lg;
			margin-bottom: $spacing-2xl; // Space below cards

			// Explicit columns example:
			// grid-template-columns: 1fr;
			// @media (min-width: $breakpoint-sm) { grid-template-columns: repeat(2, 1fr); }
			// @media (min-width: $breakpoint-lg) { grid-template-columns: repeat(3, 1fr); }
		}

		// Element: Raw Data Section
		&__raw-data {
			background-color: $color-surface; // Example: section background
			border-radius: $border-radius-md;
			padding: $spacing-lg;
			border: $border-width-thin solid $color-border-light;

			.raw-data__title {
				// Use BEM for elements within this section block
				font-size: $font-size-lg;
				font-weight: $font-weight-semibold;
				margin-bottom: $spacing-sm;
			}
			.raw-data__text {
				font-size: $font-size-md;
				color: $color-text-secondary;
				margin-bottom: 0; // Reset paragraph margin if needed

				a {
					font-weight: $font-weight-medium;
					&:hover {
						text-decoration: underline;
					}
				}
			}
		}
	}

	// Placeholder style for StatCard content
	.timeline-placeholder {
		display: flex;
		align-items: center;
		justify-content: center;
		height: 100%;
		min-height: 100px; // Ensure some height
		color: $color-text-disabled;
		font-style: italic;
	}
</style>
