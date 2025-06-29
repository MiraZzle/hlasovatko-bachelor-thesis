<script lang="ts">
	/**
	 * @file Activity Bank page
	 * This page allows users to view, filter, and create activities.
	 */
	import Button from '$components/elements/typography/Button.svelte';
	import Select from '$components/elements/typography/Select.svelte';
	import ActivityCard from '$components/dashboard/ActivityCard.svelte';
	import AddActivityModal from '$components/elements/modals/AddActivityModal.svelte';
	import type { NewActivityData, Activity } from '$lib/activities/types';
	import {
		getDefinedActivityTypes,
		addActivityToBank,
		getActivityBank
	} from '$lib/activities/activity_utils';
	import type { SelectOption } from '$lib/shared_types';
	import ActivityDetailModal from '$components/elements/modals/ActivityDetailModal.svelte';
	import { onMount } from 'svelte';

	let activities = $state<Activity[]>([]);
	let isCreateActivityModalOpen = $state(false);
	let selectedCategory = $state('all');
	let selectedActivityType = $state('all');
	let isDetailModalOpen = $state(false);
	let selectedActivityForDetail = $state<Activity | null>(null);
	let fileInputRef: HTMLInputElement | null = null;

	onMount(async () => {
		activities = await getActivityBank();
	});

	/**
	 * Gets all unique categories from activities
	 * Returns an array of SelectOption objects for categories
	 * @returns Array of categories
	 */
	function getCategoryOptions(): SelectOption[] {
		const categories = new Set<string>();
		activities.forEach((activity) => {
			activity.tags?.forEach((cat) => categories.add(cat));
		});
		const options = Array.from(categories).map((cat) => ({ value: cat, label: cat }));
		return [{ value: 'all', label: 'All Categories' }, ...options];
	}

	/**
	 * Gets all defined activity types
	 * Returns an array of SelectOption objects for activity types
	 * @param forFiltering - If true, includes an "All Activity Types" option
	 * @returns Array of activiy types
	 */
	function getActivityTypeOptions(forFiltering: boolean = false): SelectOption[] {
		const types = getDefinedActivityTypes();
		const options = types.map((type) => ({ value: type.name.toLowerCase(), label: type.name }));
		if (forFiltering) {
			return [{ value: 'all', label: 'All Activity Types' }, ...options];
		}
		return options;
	}

	// Options variables for specific components
	let categoryOptions = $derived(getCategoryOptions());
	let activityTypeOptions = $derived(getActivityTypeOptions(true));
	let availableActivityTypes = $derived(getActivityTypeOptions());

	function openCreateActivityModal(): void {
		isCreateActivityModalOpen = true;
	}
	function closeCreateActivityModal(): void {
		isCreateActivityModalOpen = false;
	}
	async function handleAddActivitySubmit(data: NewActivityData): Promise<void> {
		let newActivityDef = await addActivityToBank(data);
		if (!newActivityDef) {
			console.error('Failed to add new activity to bank.');
			return;
		}
		activities.push(newActivityDef);
		activities = activities;
	}

	async function getActivities() {
		return await getActivityBank();
	}

	function closeDetailModal(): void {
		isDetailModalOpen = false;
	}

	function triggerImport(): void {
		if (fileInputRef) {
			fileInputRef.value = '';
		}
		fileInputRef?.click();
	}

	/*
	 * Handles file selection for importing activities from JSON
	 * Validates the selected file type and logs the file details
	 */
	async function handleFileSelected(event: Event): Promise<void> {
		const target = event.target as HTMLInputElement;
		const files = target.files;

		if (files && files.length > 0) {
			const file = files[0];
			console.log('Selected file:', file.name, file.type, file.size);

			if (!file.name.toLowerCase().endsWith('.json') && file.type !== 'application/json') {
				alert('Please select a valid JSON file (.json).');
				return;
			}
		} else {
			console.log('No file selected.');
		}
	}

	/**
	 * Handles card click to open activity detail modal
	 * Sets the selected activity for detail view
	 * @param id - ID of the activity to be opened
	 */
	function handleCardClick(id: string): void {
		const activity = activities.find((a) => a.id === id);
		if (activity) {
			selectedActivityForDetail = activity;
			isDetailModalOpen = true;
		} else {
			console.error(`Activity with id ${id} not found.`);
		}
	}

	/**
	 * Filters activities based on selected category and activity type
	 * @returns Filtered array of activities
	 */
	function getFilteredActivities(): Activity[] {
		return activities.filter((activity) => {
			const categoryMatch = selectedCategory === 'all' || activity.tags?.includes(selectedCategory);
			const typeMatch =
				selectedActivityType === 'all' || activity.type.toLowerCase() === selectedActivityType;
			return categoryMatch && typeMatch;
		});
	}

	let filteredActivities = $derived(getFilteredActivities());
</script>

<svelte:head>
	<title>EngaGenie | Activity Bank</title>
</svelte:head>

<div class="activity-bank-page">
	<h1 class="activity-bank-page__title">Activity bank</h1>

	<div class="activity-bank-page__controls">
		<div class="activity-bank-page__actions">
			<Button variant="primary" onclick={openCreateActivityModal}>Create Activity</Button>
			<Button variant="outline" onclick={triggerImport}>Import from JSON</Button>
			<input
				type="file"
				accept=".json,application/json"
				bind:this={fileInputRef}
				onchange={handleFileSelected}
				style="display: none;"
				aria-hidden="true"
				tabindex="-1"
			/>
		</div>
		<div class="activity-bank-page__filters">
			<Select
				label="Category"
				options={categoryOptions}
				bind:value={selectedCategory}
				ariaLabel="Filter by category"
			/>
			<Select
				label="Activity type"
				options={activityTypeOptions}
				bind:value={selectedActivityType}
				ariaLabel="Filter by activity type"
			/>
		</div>
	</div>

	<div class="activity-bank-page__grid">
		{#each filteredActivities as activity}
			<ActivityCard {activity} onclick={() => handleCardClick(activity.id)} />
		{:else}
			<p class="activity-bank-page__no-results">No activities found matching your filters.</p>
		{/each}
	</div>

	<AddActivityModal
		bind:open={isCreateActivityModalOpen}
		activityTypes={availableActivityTypes}
		onAdd={handleAddActivitySubmit}
		onclose={closeCreateActivityModal}
	/>

	<ActivityDetailModal
		bind:open={isDetailModalOpen}
		activity={selectedActivityForDetail}
		onclose={closeDetailModal}
	/>
</div>

<style lang="scss">
	.activity-bank-page {
		&__title {
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			margin-bottom: $spacing-lg;
		}

		&__controls {
			display: flex;
			justify-content: space-between;
			flex-direction: column;
			flex-wrap: wrap;
			gap: $spacing-lg;
			margin-bottom: $spacing-xl;
		}

		&__actions {
			display: flex;
			gap: $spacing-md;
		}

		&__filters {
			display: flex;
			gap: $spacing-md;
			flex-wrap: wrap;
		}

		&__grid {
			display: grid;
			grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
			gap: $spacing-lg;
		}

		&__no-results {
			color: $color-text-secondary;
			font-style: italic;
			grid-column: 1 / -1;
			text-align: center;
			padding: $spacing-xl;
		}
	}
</style>
