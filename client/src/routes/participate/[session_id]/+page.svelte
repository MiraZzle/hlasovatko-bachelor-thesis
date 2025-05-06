<script lang="ts">
	import { page } from '$app/stores';
	import Button from '$components/elements/typography/Button.svelte';
	import { onMount } from 'svelte';

	// Import answer input components
	import MultipleChoiceInput from '$components/activities/MultipleChoiceInput.svelte';
	import OpenEndedInput from '$components/activities/OpenEndedInput.svelte';
	import ScaleRatingInput from '$components/activities/ScaleRatingInput.svelte';
	// Import types (ensure path is correct)
	import type {
		SessionActivity,
		MultipleChoiceDefinition,
		PollDefinition,
		ScaleRatingDefinition,
		OpenEndedDefinition,
		KnownActivityDefinition
	} from '$lib/types';
	import {
		isMultipleChoice,
		isPoll,
		isScaleRating,
		isOpenEnded,
		getKnownDefinition
	} from '$lib/types';

	// --- Get Session ID ---
	let { session_id } = $page.params;

	// --- State ---
	// Mock current activity data - This would be fetched via WebSocket or polling
	let currentActivity = $state<SessionActivity | null>(null);
	let studentAnswer = $state<any>(null); // Type depends on activity
	let isSubmitting = $state(false);
	let submissionStatus = $state<'idle' | 'success' | 'error' | 'waiting'>('waiting'); // 'waiting' = waiting for activity

	// --- Mock Data Fetching/WebSocket Listener ---
	// In a real app, this would connect to a WebSocket or poll for the current activity
	function mockFetchCurrentActivity() {
		console.log(`Fetching current activity for session ${session_id}...`);
		submissionStatus = 'waiting';
		studentAnswer = null; // Reset answer for new activity
		// Simulate different activity types
		const activityTypes: SessionActivity['type'][] = [
			'MultipleChoice',
			'Poll',
			'ScaleRating',
			'OpenEnded'
		];
		const randomIndex = Math.floor(Math.random() * activityTypes.length);
		const randomType = activityTypes[randomIndex];

		let newActivity: SessionActivity;

		switch (randomType) {
			case 'MultipleChoice':
				newActivity = {
					id: `act-${Date.now()}`,
					type: 'MultipleChoice',
					title: 'Which is the largest planet in our solar system?',
					definition: {
						type: 'MultipleChoice',
						options: [
							{ id: 'a', text: 'Earth' },
							{ id: 'b', text: 'Mars' },
							{ id: 'c', text: 'Jupiter' },
							{ id: 'd', text: 'Saturn' }
						],
						allowMultiple: false
					}
				};
				break;
			case 'Poll':
				newActivity = {
					id: `act-${Date.now()}`,
					type: 'Poll',
					title: 'What is your favorite season?',
					definition: {
						type: 'Poll',
						options: [
							{ id: 's', text: 'Spring' },
							{ id: 'su', text: 'Summer' },
							{ id: 'a', text: 'Autumn' },
							{ id: 'w', text: 'Winter' }
						]
					}
				};
				break;
			case 'ScaleRating':
				newActivity = {
					id: `act-${Date.now()}`,
					type: 'ScaleRating',
					title: 'Rate this session so far (1-5)',
					definition: {
						type: 'ScaleRating',
						min: 1,
						max: 5,
						minLabel: 'Poor',
						maxLabel: 'Excellent'
					}
				};
				break;
			case 'OpenEnded':
			default:
				newActivity = {
					id: `act-${Date.now()}`,
					type: 'OpenEnded',
					title: 'What topics would you like to review?',
					definition: { type: 'OpenEnded' }
				};
				break;
		}
		currentActivity = newActivity;
		submissionStatus = 'idle';
	}

	// Simulate receiving a new activity after a delay or on some event
	// In a real app, use onMount to establish WebSocket connection
	// For demo, just load one on mount, and allow manual "next"
	onMount(() => {
		mockFetchCurrentActivity();
	});

	// --- Submission Handler ---
	async function handleSubmitAnswer() {
		if (studentAnswer === null || (Array.isArray(studentAnswer) && studentAnswer.length === 0)) {
			alert('Please select an answer or provide input.');
			return;
		}
		isSubmitting = true;
		submissionStatus = 'idle';
		console.log(`Submitting answer for activity ${currentActivity?.id}:`, studentAnswer);

		// --- TODO: API Call to submit answer ---
		await new Promise((resolve) => setTimeout(resolve, 750)); // Simulate network
		// Assume success for now
		submissionStatus = 'success';
		isSubmitting = false;

		// After submission, wait for the next activity (or show a message)
		// In a real app, WebSocket would push the next activity or a "waiting" state.
		// For demo, we'll just clear current activity and show waiting.
		// currentActivity = null;
		// submissionStatus = 'waiting';
	}

	// Helper to get typed definition for rendering
	let typedDefinition = $derived(currentActivity ? getKnownDefinition(currentActivity) : null);
</script>

<div class="participation-page">
	{#if currentActivity}
		<div class="participation-page__activity-card">
			<span class="participation-page__activity-type">{currentActivity.type}</span>
			<h1 class="participation-page__activity-title">{currentActivity.title}</h1>

			<div class="participation-page__answer-area">
				{#if typedDefinition}
					{#if isMultipleChoice(typedDefinition)}
						<MultipleChoiceInput
							options={typedDefinition.options}
							allowMultiple={typedDefinition.allowMultiple}
							bind:selected={studentAnswer}
							disabled={isSubmitting || submissionStatus === 'success'}
						/>
					{:else if isPoll(typedDefinition)}
						<MultipleChoiceInput
							options={typedDefinition.options}
							allowMultiple={false}
							bind:selected={studentAnswer}
							disabled={isSubmitting || submissionStatus === 'success'}
						/>
					{:else if isScaleRating(typedDefinition)}
						<ScaleRatingInput
							min={typedDefinition.min}
							max={typedDefinition.max}
							minLabel={typedDefinition.minLabel}
							maxLabel={typedDefinition.maxLabel}
							bind:value={studentAnswer}
							disabled={isSubmitting || submissionStatus === 'success'}
						/>
					{:else if isOpenEnded(typedDefinition)}
						<OpenEndedInput
							bind:value={studentAnswer}
							disabled={isSubmitting || submissionStatus === 'success'}
						/>
					{:else}
						<p class="participation-page__message participation-page__message--error">
							Unsupported activity type definition.
						</p>
					{/if}
				{:else if currentActivity.type === 'OpenEnded'}
					<OpenEndedInput
						bind:value={studentAnswer}
						disabled={isSubmitting || submissionStatus === 'success'}
					/>
				{:else}
					<p class="participation-page__message">Activity definition not recognized or missing.</p>
				{/if}
			</div>

			<div class="participation-page__actions">
				{#if submissionStatus === 'idle'}
					<Button
						variant="primary"
						onclick={handleSubmitAnswer}
						disabled={isSubmitting ||
							studentAnswer === null ||
							(Array.isArray(studentAnswer) && studentAnswer.length === 0)}
						fullWidth
					>
						{#if isSubmitting}Submitting...{:else}Submit Answer{/if}
					</Button>
				{:else if submissionStatus === 'success'}
					<div class="participation-page__feedback participation-page__feedback--success">
						Answer Submitted! Waiting for next activity...
					</div>
				{:else if submissionStatus === 'error'}
					<div class="participation-page__feedback participation-page__feedback--error">
						Submission failed. Please try again.
					</div>
				{/if}
			</div>
		</div>
	{:else if submissionStatus === 'waiting'}
		<div class="participation-page__waiting-card">
			<h2 class="participation-page__waiting-title">
				Waiting for the host to start an activity...
			</h2>
			<div class="participation-page__spinner"></div>
		</div>
	{:else}
		<div class="participation-page__waiting-card">
			<h2 class="participation-page__waiting-title">No active activity.</h2>
			<p class="participation-page__waiting-text">Please wait for the session host.</p>
			<Button onclick={mockFetchCurrentActivity} variant="outline">Next Activity (Demo)</Button>
		</div>
	{/if}
</div>

<style lang="scss">
	@import '../../../styles/variables.scss'; // Adjust path

	// Block: participation-page
	.participation-page {
		width: 100%;
		max-width: 600px;
		margin: $spacing-lg auto;

		// Element: Activity Card
		&__activity-card {
			background-color: $color-surface;
			padding: $spacing-xl;
			border-radius: $border-radius-lg;
			box-shadow: $box-shadow-md;
			display: flex;
			flex-direction: column;
			gap: $spacing-lg;
		}

		// Element: Activity Type Badge
		&__activity-type {
			display: inline-block;
			background-color: $color-primary-light;
			color: $color-text-on-primary;
			padding: $spacing-xs $spacing-sm;
			border-radius: $border-radius-sm;
			font-size: $font-size-xs;
			font-weight: $font-weight-semibold;
			text-transform: uppercase;
			align-self: flex-start;
		}

		// Element: Activity Title (Question)
		&__activity-title {
			font-size: $font-size-2xl;
			font-weight: $font-weight-bold;
			color: $color-text-primary;
			margin: 0;
			line-height: 1.3;
		}

		// Element: Answer Area
		&__answer-area {
			// No specific styles currently, but placeholder for future
		}

		// Element: Actions (Submit button container)
		&__actions {
			margin-top: $spacing-md;
		}

		// Element: Feedback Messages
		&__feedback {
			padding: $spacing-md;
			border-radius: $border-radius-md;
			text-align: center;
			font-weight: $font-weight-medium;

			// Modifier: Success
			&--success {
				background-color: rgba($color-success, 0.1);
				color: darken($color-success, 10%);
				border: 1px solid rgba($color-success, 0.3);
			}
			// Modifier: Error
			&--error {
				background-color: rgba($color-error, 0.1);
				color: $color-error;
				border: 1px solid rgba($color-error, 0.3);
			}
		}

		// Element: Waiting Card
		&__waiting-card {
			background-color: $color-surface;
			padding: $spacing-2xl $spacing-xl;
			border-radius: $border-radius-lg;
			box-shadow: $box-shadow-md;
			text-align: center;
		}

		// Element: Waiting Title
		&__waiting-title {
			font-size: $font-size-xl;
			color: $color-text-secondary;
			margin-bottom: $spacing-lg;
		}

		// Element: Waiting Text (for "Please wait...")
		&__waiting-text {
			font-size: $font-size-md;
			color: $color-text-secondary;
			margin-bottom: $spacing-lg;
		}

		// Element: Spinner
		&__spinner {
			margin: $spacing-lg auto;
			border: 4px solid $color-surface-alt;
			border-top: 4px solid $color-primary;
			border-radius: 50%;
			width: 40px;
			height: 40px;
			animation: participation-page__spin 1s linear infinite; // Scoped animation name
		}

		// Element: Message (for unsupported/missing definition)
		&__message {
			font-style: italic;
			color: $color-text-secondary;
			padding: $spacing-md;
			background-color: $color-surface-alt;
			border-radius: $border-radius-sm;
			text-align: center;

			// Modifier: Error
			&--error {
				color: $color-error;
				background-color: rgba($color-error, 0.05);
				border: 1px solid rgba($color-error, 0.1);
			}
		}
	}

	// Keyframes animation (scoped by prefixing with block name or making it global)
	@keyframes participation-page__spin {
		0% {
			transform: rotate(0deg);
		}
		100% {
			transform: rotate(360deg);
		}
	}
</style>
