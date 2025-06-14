<script lang="ts">
	import Select from '$components/elements/typography/Select.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import StatCard from '$components/dashboard/StatCard.svelte';
	import ExportAnalyticsModal from '$components/elements/modals/ExportAnalyticsModal.svelte';
	import {
		getTotalNumberOfSessions,
		getMostPopularActivityType,
		getTotalResponses
	} from '$lib/analytics/analytics_utils';
	import type { TimeFrame } from '$lib/analytics/types';

	let selectedTimeFrame = $state<TimeFrame>('7d');
	const timeFrameOptions: { value: TimeFrame; label: string }[] = [
		{ value: '7d', label: '7 days' },
		{ value: '30d', label: '30 days' },
		{ value: '90d', label: '90 days' },
		{ value: 'all', label: 'All time' }
	];

	let isExportModalOpen = $state(false);

	function getGlobalMetrics() {
		return {
			sessions: getTotalNumberOfSessions(selectedTimeFrame),
			activity: getMostPopularActivityType(selectedTimeFrame),
			answers: getTotalResponses(selectedTimeFrame)
		};
	}

	let globalMetrics = $derived(getGlobalMetrics());

	function handleTimeFrameChange(event: Event & { currentTarget: HTMLSelectElement }) {
		console.log('Time frame changed to:', selectedTimeFrame);
	}

	function openExportModal(): void {
		isExportModalOpen = true;
	}
	function closeExportModal(): void {
		isExportModalOpen = false;
	}
	function handleExportSubmit(format: string): void {
		console.log(
			`Exporting statistics for time frame '${selectedTimeFrame}' in format '${format}'...`
		);
		alert(`Exporting ${format}... (Placeholder)`);
	}

	function getCurrentTimeFrameLabel(): string {
		const option = timeFrameOptions.find((opt) => opt.value === selectedTimeFrame);
		return option ? `the last ${option.label.toLowerCase()}` : 'the selected period';
	}

	let currentTimeFrameLabel = $derived(getCurrentTimeFrameLabel());
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
		<Button variant="outline" onclick={openExportModal}>Export</Button>
	</div>

	<div class="analytics-page__stats-grid">
		<StatCard title="Total Sessions">
			<p class="analytics-page__stat-value">{globalMetrics.sessions}</p>
		</StatCard>

		<StatCard title="Total Responses">
			<p class="analytics-page__stat-value">{globalMetrics.answers}</p>
		</StatCard>

		<StatCard title="Most Popular Activity Type">
			<p class="analytics-page__stat-value">{globalMetrics.activity}</p>
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

	<ExportAnalyticsModal
		bind:open={isExportModalOpen}
		timeFrameLabel={currentTimeFrameLabel}
		onclose={closeExportModal}
		onExport={handleExportSubmit}
	/>
</div>

<style lang="scss">
	.analytics-page {
		&__header {
			margin-bottom: $spacing-xl;
		}

		&__title {
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			margin-bottom: $spacing-xs;
		}

		&__description {
			font-size: $font-size-md;
			color: $color-text-secondary;
			max-width: 600px;
		}

		&__controls {
			display: flex;
			justify-content: space-between;
			align-items: flex-end;
			margin-bottom: $spacing-xl;
			gap: $spacing-md;
			flex-wrap: wrap;
		}

		&__stats-grid {
			display: grid;
			grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
			gap: $spacing-lg;
			margin-bottom: $spacing-2xl;
		}

		&__stat-value {
			font-size: $font-size-2xl;
			font-weight: $font-weight-bold;
			color: $color-text-primary;
			text-align: center;
			margin: $spacing-md 0;
		}

		&__raw-data {
			background-color: $color-surface;
			border-radius: $border-radius-md;
			padding: $spacing-lg;
			border: $border-width-thin solid $color-border-light;

			.raw-data__title {
				font-size: $font-size-lg;
				font-weight: $font-weight-semibold;
				margin-bottom: $spacing-sm;
			}
			.raw-data__text {
				font-size: $font-size-md;
				color: $color-text-secondary;
				margin-bottom: 0;

				a {
					font-weight: $font-weight-medium;
					&:hover {
						text-decoration: underline;
					}
				}
			}
		}
	}

	.timeline-placeholder {
		display: flex;
		align-items: center;
		justify-content: center;
		height: 100%;
		min-height: 100px;
		color: $color-text-disabled;
		font-style: italic;
	}
</style>
