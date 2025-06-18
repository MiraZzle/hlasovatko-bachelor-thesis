<script lang="ts">
	import { fade } from 'svelte/transition';
	import Button from '$components/elements/typography/Button.svelte';

	import type { SessionActivity, KnownActivityDefinition } from '$lib/activity_types';
	import { isMultipleChoice, isPoll, isScaleRating, isOpenEnded } from '$lib/activity_types';

	import MultipleChoice from '$components/activities/MultipleChoice.svelte';
	import Poll from '$components/activities/Poll.svelte';
	import ScaleRating from '$components/activities/ScaleRating.svelte';
	import OpenEnded from '$components/activities/OpenEnded.svelte';
	import RawJson from '$components/activities/RawJson.svelte';
	import type { Activity } from '$lib/activities/types';

	let {
		activity,
		isOnlyView = false
	}: {
		activity: Activity;
		isOnlyView?: boolean;
	} = $props();

	/*
	 * Parses the activity definition and validates its structure.
	 * If the type in the definition does not match the activity type,
	 * it injects/corrects the type into the definition.
	 *
	 * @returns Parsed definition or an error object if parsing fails.
	 */
	function getParsedDefinition(): JSON | { error: string; original: unknown } | object {
		let def = activity.definition;

		// If definition is a string, try to parse it as JSON
		if (typeof def === 'string') {
			try {
				def = JSON.parse(def);
			} catch (e) {
				console.error(`Failed to parse definition string for activity ${activity.id}`, e);
				return { error: 'Invalid JSON string', original: activity.definition };
			}
		}

		// Check if the parsed definition is an object
		if (typeof def !== 'object' || def === null) {
			console.warn(`Activity ${activity.id} definition is not a valid object after parsing.`);
			return { error: 'Invalid definition structure', original: activity.definition };
		}

		// Ensure the definition has a type and matches the activity type
		if (!('type' in def) || def.type !== activity.type) {
			console.warn(
				`Injecting/correcting type '${activity.type}' into definition for activity ${activity.id}`
			);

			// Inject the type into definition if its missing or incorrect
			def = { ...def, type: activity.type };
			console.log('New definition:', def);
		}
		return def;
	}

	let parsedDefinition: KnownActivityDefinition | { error: string; original: unknown } | object =
		$derived(getParsedDefinition());
</script>

<div class="session-activity-item" transition:fade|local={{ duration: 200 }}>
	<div class="session-activity-item__header">
		<span class="session-activity-item__type">{activity.type}</span>
		<h3 class="session-activity-item__title">{activity.title}</h3>
	</div>

	<div class="session-activity-item__body">
		{#if isMultipleChoice(parsedDefinition)}
			<MultipleChoice definition={parsedDefinition} />
		{:else if isPoll(parsedDefinition)}
			<Poll definition={parsedDefinition} />
		{:else if isScaleRating(parsedDefinition)}
			<ScaleRating definition={parsedDefinition} />
		{:else if isOpenEnded(parsedDefinition)}
			<OpenEnded />
		{:else}
			<RawJson definition={parsedDefinition} />
		{/if}
	</div>
</div>

<style lang="scss">
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
