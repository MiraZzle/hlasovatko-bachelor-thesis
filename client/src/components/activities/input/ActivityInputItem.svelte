<script lang="ts">
	/**
	 * @file A unified component that dynamically renders the correct input
	 * interface for a given activity and handles the submission of the user's answer.
	 */
	import type { Activity } from '$lib/activities/types';
	import type { MultipleChoiceDefinition } from '$lib/activities/definition_types';
	import type { ScaleRatingDefinition } from '$lib/activities/definition_types';
	import MultipleChoiceInput from './MultipleChoiceInput.svelte';
	import OpenEndedInput from './OpenEndedInput.svelte';
	import ScaleRatingInput from './ScaleRatingInput.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import type { SubmitPayload } from '$lib/activities/definition_types';

	let {
		activity,
		isSubmitting = false,
		disabled = false,
		onSubmit
	}: {
		activity: Activity;
		isSubmitting?: boolean;
		disabled?: boolean;
		onSubmit?: (payload: SubmitPayload) => void;
	} = $props();

	// Typecast the activity definition to the specific types
	const mcDef = $derived(activity.definition as MultipleChoiceDefinition);
	const srDef = $derived(activity.definition as ScaleRatingDefinition);

	function getInitialValue(act: Activity) {
		switch (act.type) {
			case 'multiple_choice':
			case 'poll':
				const mcDef = act.definition as MultipleChoiceDefinition;
				return mcDef?.allowMultipleAnswers ? [] : null;
			case 'open_ended':
				return '';
			case 'scale_rating':
			default:
				return null;
		}
	}

	// User answer value
	let value: string | string[] | number | null = $state(getInitialValue(activity));

	/**
	 * Effect that resets the value based on the activity type.
	 * This is necessary to ensure that the input is cleared when the activity changes.
	 */
	$effect(() => {
		const _ = activity.id;
		value = getInitialValue(activity);
	});

	// Checks if the current value is valid for submission
	const isSubmittable = $derived(() => {
		if (value === null) return false;
		if (typeof value === 'string') return value.trim().length > 0;
		if (Array.isArray(value)) return value.length > 0;
		if (typeof value === 'number') return true;
		return false;
	});

	// Handles the submission of the answer
	function handleSubmit(): void {
		if (!isSubmittable || disabled || isSubmitting) return;

		onSubmit?.({
			activityId: activity.id,
			value: value,
			activityType: activity.type
		});
	}
</script>

<div class="activity-input">
	<div class="activity-input__header">
		<h3 class="activity-input__title">{activity.title}</h3>
		<span class="activity-input__type">{activity.type.replace('_', ' ')}</span>
	</div>

	<div class="activity-input__body">
		{#if activity.type === 'multiple_choice' || activity.type === 'poll'}
			<MultipleChoiceInput
				options={mcDef.options}
				allowMultiple={mcDef.allowMultipleAnswers}
				bind:selected={value as string | string[] | null}
				{disabled}
			/>
		{:else if activity.type === 'scale_rating'}
			<ScaleRatingInput
				min={srDef.min}
				max={srDef.max}
				minLabel={srDef.minLabel}
				maxLabel={srDef.maxLabel}
				bind:value={value as number | null}
				{disabled}
			/>
		{:else if activity.type === 'open_ended'}
			<OpenEndedInput bind:value={value as string} {disabled} />
		{:else}
			<p class="activity-input__unsupported">
				This activity type is not yet supported for participation.
			</p>
		{/if}
	</div>

	<div class="activity-input__footer">
		<Button
			variant="primary"
			fullWidth
			onclick={handleSubmit}
			disabled={!isSubmittable || disabled || isSubmitting}
		>
			{#if isSubmitting}
				Submitting...
			{:else}
				Submit Answer
			{/if}
		</Button>
	</div>
</div>

<style lang="scss">
	.activity-input {
		font-family: $font-family-primary;
		background-color: $color-surface;
		border-radius: $border-radius-lg;
		padding: $spacing-xl;
		border: 1px solid $color-border-light;
		box-shadow: $box-shadow-md;
		display: flex;
		flex-direction: column;
		gap: $spacing-lg;

		&__header {
			text-align: center;
		}

		&__title {
			font-size: $font-size-lg;
			font-weight: $font-weight-semibold;
			color: $color-text-primary;
			margin: 0 0 $spacing-sm 0;
		}

		&__type {
			display: inline-block;
			background-color: #eef0f2;
			color: $color-text-secondary;
			padding: $spacing-sm * 0.5 $spacing-md;
			border-radius: $border-radius-sm;
			font-size: $font-size-xs;
			font-weight: 500;
			text-transform: uppercase;
		}

		&__footer {
			margin-top: $spacing-md;

			:global(.btn) {
				width: 100%;
			}
		}

		&__unsupported {
			text-align: center;
			color: $color-text-secondary;
			font-style: italic;
			padding: $spacing-xl 0;
		}
	}
</style>
