<script lang="ts">
	/**
	 * @file Global Analytics Overview page
	 * This page provides an overview of global analytics across all sessions.
	 */
	import Button from '$components/elements/typography/Button.svelte';
	import StatCard from '$components/dashboard/StatCard.svelte';
	import ExportAnalyticsModal from '$components/elements/modals/ExportAnalyticsModal.svelte';
	import { getStatistics, exportStatistics } from '$lib/analytics/analytics_utils';
	import { onMount } from 'svelte';
	import type { Statistics } from '$lib/analytics/types';
	import type { ExportFormat } from '$lib/analytics/types';
	import { API_URL } from '$lib/config';

	let isExportModalOpen = $state(false);
	let statistics: Statistics | null = $state(null);

	onMount(async () => {
		try {
			statistics = await getStatistics();
		} catch (error) {
			console.error('Failed to fetch statistics:', error);
			alert('Failed to load statistics. Please try again later.');
		}
	});

	function openExportModal(): void {
		isExportModalOpen = true;
	}

	function closeExportModal(): void {
		isExportModalOpen = false;
	}

	async function handleExportSubmit(format: string): Promise<void> {
		try {
			await exportStatistics(format as ExportFormat);
			alert('Export successful!');
		} catch (error) {
			alert(
				`Failed to export statistics: ${error instanceof Error ? error.message : 'Unknown error'}`
			);
		} finally {
			closeExportModal();
		}
	}
</script>

<svelte:head>
	<title>EngaGenie | Analytics</title>
</svelte:head>

<div class="analytics-page">
	<header class="analytics-page__header">
		<h1 class="analytics-page__title">Global Analytics Overview</h1>
		<p class="analytics-page__description">Insights into engagement across all your sessions.</p>
	</header>

	<div class="analytics-page__controls">
		<Button variant="outline" onclick={openExportModal}>Export</Button>
	</div>

	{#if !statistics}
		<p>Loading statistics...</p>
	{:else}
		<div class="analytics-page__stats-grid">
			<StatCard title="Total sessions">
				<p class="analytics-page__stat-value">{statistics!.totalSessions}</p>
			</StatCard>

			<StatCard title="Total activities">
				<p class="analytics-page__stat-value">{statistics!.totalActivities}</p>
			</StatCard>

			<StatCard title="Most popular activity type">
				<p class="analytics-page__stat-value">{statistics!.mostCommonActivityType}</p>
			</StatCard>
		</div>
	{/if}

	<section class="analytics-page__raw-data">
		<h2 class="raw-data__title">Need more control?</h2>
		<p class="raw-data__text">
			Use the public API. <a
				href="{API_URL}/swagger/index.html"
				target="_blank"
				rel="noopener noreferrer">[Read the Docs]</a
			>
		</p>
	</section>

	<ExportAnalyticsModal
		bind:open={isExportModalOpen}
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
</style>
