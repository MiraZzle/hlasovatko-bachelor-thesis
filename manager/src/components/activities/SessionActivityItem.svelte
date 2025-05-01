<script lang="ts">
	import { fade } from 'svelte/transition';
	import Button from '$components/elements/typography/Button.svelte'; // Verify path

	// Import type definitions and guards (ensure path is correct)
	import type {
		SessionActivity,
		MultipleChoiceDefinition,
		PollDefinition,
		ScaleRatingDefinition,
		OpenEndedDefinition,
		KnownActivityDefinition
	} from '$lib/activity_types';
	import {
		isMultipleChoice,
		isPoll,
		isScaleRating,
		isOpenEnded,
		getKnownDefinition // Use the helper to safely get typed definition
	} from '$lib/activity_types';

	// Import  components
	import MultipleChoice from '$components/activities/MultipleChoice.svelte';
	import Poll from '$components/activities/Poll.svelte';
	import ScaleRating from '$components/activities/ScaleRating.svelte';
	import OpenEnded from '$components/activities/OpenEnded.svelte';
	import RawJson from '$components/activities/RawJson.svelte';

	// --- Component Props ---
	type Props = {
		activity: SessionActivity;
		onStart?: (id: string) => void;
		onStop?: (id: string) => void;
		onViewResults?: (id: string) => void;
	};

	let {
		activity,
		onStart = (id) => console.log('Start:', id),
		onStop = (id) => console.log('Stop:', id),
		onViewResults = (id) => console.log('View Results:', id)
	}: Props = $props();

	// --- Process Definition ---
	// Try to parse definition if it's a string, otherwise use as is.
	// Also attempt to get a strongly-typed known definition.
	let parsedDefinition: KnownActivityDefinition | object | null = $derived(() => {
		let def = activity.definition;
		if (typeof def === 'string') {
			try {
				def = JSON.parse(def);
			} catch (e) {
				console.error(`Failed to parse definition string for activity ${activity.id}`, e);
				return activity.definition; // Return original string on error
			}
		}
		// Ensure it's an object before further checks
		if (typeof def !== 'object' || def === null) {
			return {}; // Return empty object if not valid
		}
		// Add the 'type' property if it's missing but known from activity.type
		if (!('type' in def) && typeof activity.type === 'string') {
			(def as any).type = activity.type;
		}
		return def;
	});

	// Get the specific known definition type, if applicable
	let knownDefinition = $derived(getKnownDefinition({ ...activity, definition: parsedDefinition }));

	// Placeholder Icons
	const IconPlay = () => '▶️';
	const IconStop = () => '⏹️';
	const IconResults = () => '📊';
</script>

<div class="session-activity-item" transition:fade|local={{ duration: 200 }}>
	<div class="session-activity-item__header">
		<span class="session-activity-item__type">{activity.type}</span>
		<h3 class="session-activity-item__title">{activity.title}</h3>
		{#if activity.status}
			<span
				class="session-activity-item__status session-activity-item__status--{activity.status.toLowerCase()}"
			>
				{activity.status}
			</span>
		{/if}
	</div>

	<div class="session-activity-item__body">
		{#if activity.type === 'MultipleChoice' && knownDefinition && isMultipleChoice(knownDefinition)}
			<MultipleChoice definition={knownDefinition} />
		{:else if activity.type === 'Poll' && knownDefinition && isPoll(knownDefinition)}
			<Poll definition={knownDefinition} />
		{:else if activity.type === 'ScaleRating' && knownDefinition && isScaleRating(knownDefinition)}
			<ScaleRating definition={knownDefinition} />
		{:else if activity.type === 'OpenEnded' && knownDefinition && isOpenEnded(knownDefinition)}
			<OpenEnded />
		{:else}
			<RawJson definition={parsedDefinition} />
		{/if}
	</div>

	<div class="session-activity-item__footer">
		{#if activity.status === 'Pending' || activity.status === 'Closed'}
			<Button onclick={() => onStart(activity.id)}>Start</Button>
		{/if}
		{#if activity.status === 'Active'}
			<Button onclick={() => onStop(activity.id)}>Stop</Button>
		{/if}
		{#if activity.status === 'Closed'}
			<Button variant="outline" onclick={() => onViewResults(activity.id)}>Results</Button>
		{/if}
	</div>
</div>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

	// Block: session-activity-item
	.session-activity-item {
		background-color: $color-surface;
		border-radius: $border-radius-md;
		border: 1px solid $color-border-light;
		box-shadow: $box-shadow-sm;
		padding: $spacing-md;
		display: flex;
		flex-direction: column;
		gap: $spacing-sm;

		&__header {
			display: flex;
			align-items: baseline;
			gap: $spacing-sm;
			padding-bottom: $spacing-sm;
			border-bottom: 1px solid $color-border-light;
		}

		&__type {
			background-color: $color-surface-alt;
			color: $color-text-secondary;
			padding: $spacing-xs * 0.5 $spacing-sm;
			border-radius: $border-radius-sm;
			font-size: $font-size-xs;
			font-weight: $font-weight-medium;
			text-transform: uppercase;
			white-space: nowrap;
			flex-shrink: 0;
		}

		&__title {
			font-size: $font-size-md;
			font-weight: $font-weight-medium;
			color: $color-text-primary;
			margin: 0;
			flex-grow: 1;
		}

		&__status {
			display: inline-block;
			padding: $spacing-xs $spacing-sm;
			border-radius: $border-radius-pill;
			font-size: $font-size-xs;
			font-weight: $font-weight-medium;
			text-transform: uppercase;
			white-space: nowrap;
			margin-left: auto;
			flex-shrink: 0;
			&--pending {
				background-color: $color-surface-alt;
				color: $color-text-secondary;
			}
			&--active {
				background-color: rgba($color-success, 0.15);
				color: darken($color-success, 10%);
			}
			&--closed {
				background-color: darken($color-surface-alt, 5%);
				color: $color-text-disabled;
			}
		}

		&__body {
			/* styles if needed */
		}

		&__footer {
			display: flex;
			justify-content: flex-end;
			gap: $spacing-sm;
			padding-top: $spacing-sm;
			border-top: 1px solid $color-border-light;
			margin-top: auto;
		}
	}
</style>
