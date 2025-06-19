<script lang="ts">
	import { fade } from 'svelte/transition';

	import type {
		MultipleChoiceDefinition,
		PollDefinition,
		ScaleRatingDefinition
	} from '$lib/activities/definition_types';

	import MultipleChoice from '$components/activities/MultipleChoice.svelte';
	import Poll from '$components/activities/Poll.svelte';
	import ScaleRating from '$components/activities/ScaleRating.svelte';
	import OpenEnded from '$components/activities/OpenEnded.svelte';
	import RawJson from '$components/activities/RawJson.svelte';
	import type { Activity } from '$lib/activities/types';

	let {
		activity
	}: {
		activity: Activity;
	} = $props();
</script>

<div class="session-activity-item" transition:fade|local>
	<div class="session-activity-item__header">
		<span class="session-activity-item__type">{activity.type.replace('_', ' ')}</span>
		<h3 class="session-activity-item__title">{activity.title}</h3>
	</div>

	<div class="session-activity-item__body">
		{#if activity.type === 'multiple_choice'}
			<MultipleChoice definition={activity.definition as MultipleChoiceDefinition} />
		{:else if activity.type === 'poll'}
			<Poll definition={activity.definition as PollDefinition} />
		{:else if activity.type === 'scale_rating'}
			<ScaleRating definition={activity.definition as ScaleRatingDefinition} />
		{:else if activity.type === 'open_ended'}
			<OpenEnded />
		{:else}
			<RawJson definition={activity.definition} />
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
