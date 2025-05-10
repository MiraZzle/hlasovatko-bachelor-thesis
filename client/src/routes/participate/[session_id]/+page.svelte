<script lang="ts">
	import { page } from '$app/stores';
	import Button from '$components/elements/typography/Button.svelte';
	import { onMount } from 'svelte';

	// Import answer input components
	import MultipleChoiceInput from '$components/activities/MultipleChoiceInput.svelte';
	import OpenEndedInput from '$components/activities/OpenEndedInput.svelte';
	import ScaleRatingInput from '$components/activities/ScaleRatingInput.svelte';

	// --- Import Result Display Components ---
	import ChoiceResultsDisplay from '$components/analytics/ChoiceResultsDisplay.svelte'; // Verify path
	import ScaleRatingResultsDisplay from '$components/analytics/ScaleRatingResultsDisplay.svelte'; // Verify path
	import OpenEndedResultsDisplay from '$components/analytics/OpenEndedResultsDisplay.svelte'; // Verify path
	// RawJsonDisplay might not be needed for student view of results

	// Import types
	import type {
		SessionActivity,
		MultipleChoiceDefinition,
		PollDefinition,
		ScaleRatingDefinition,
		OpenEndedDefinition,
		// Import result types
		ChoiceActivityResult,
		ScaleRatingActivityResult,
		OpenEndedActivityResult
	} from '$lib/activity_types'; // Adjust path
	import {
		isMultipleChoice,
		isPoll,
		isScaleRating,
		isOpenEnded,
		getKnownDefinition,
		// Import result type guards
		isChoiceResult,
		isScaleRatingResult,
		isOpenEndedResult
	} from '$lib/activity_types'; // Adjust path

	// --- Get Session ID ---
	let { session_id } = $page.params;

	// --- State ---
	let currentActivity = $state<SessionActivity | null>(null);
	let studentAnswer = $state<any>(null);
	let isSubmitting = $state(false);
	let submissionStatus = $state<'idle' | 'success' | 'error' | 'waiting'>('waiting');

	// --- NEW: State for showing results to student ---
	// In a real app, this would be updated via WebSocket from the teacher's actions
	let showResultsToStudent = $state(false);
	// For demo purposes, a button to toggle this will be added
	function toggleStudentResultsView() {
		showResultsToStudent = !showResultsToStudent;
		if (showResultsToStudent && currentActivity && !currentActivity.results) {
			// If showing results and no results yet, simulate fetching them
			// This is purely for demo; in reality, results come when teacher releases them.
			mockFetchResultsForCurrentActivity();
		}
	}

	// --- Mock Data Fetching ---
	function mockFetchCurrentActivity() {
		console.log(`Fetching current activity for session ${session_id}...`);
		submissionStatus = 'waiting';
		studentAnswer = null;
		showResultsToStudent = false; // Reset for new activity
		currentActivity = null; // Clear previous activity first

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
					title: 'Which is the largest planet?',
					definition: {
						type: 'MultipleChoice',
						options: [
							{ id: 'a', text: 'Earth' },
							{ id: 'b', text: 'Jupiter' }
						],
						allowMultiple: false,
						correctOptionId: 'b'
					},
					results: null // Results will be populated later
				};
				break;
			case 'Poll':
				newActivity = {
					id: `act-${Date.now()}`,
					type: 'Poll',
					title: 'Favorite season?',
					definition: {
						type: 'Poll',
						options: [
							{ id: 's', text: 'Spring' },
							{ id: 'su', text: 'Summer' }
						]
					},
					results: null
				};
				break;
			case 'ScaleRating':
				newActivity = {
					id: `act-${Date.now()}`,
					type: 'ScaleRating',
					title: 'Rate this (1-3)',
					definition: { type: 'ScaleRating', min: 1, max: 3, minLabel: 'Low', maxLabel: 'High' },
					results: null
				};
				break;
			default: // OpenEnded
				newActivity = {
					id: `act-${Date.now()}`,
					type: 'OpenEnded',
					title: 'Your feedback?',
					definition: { type: 'OpenEnded' },
					results: null
				};
				break;
		}
		currentActivity = newActivity;
		submissionStatus = 'idle';
	}

	// --- Mock Fetching/Receiving Results for the current activity ---
	function mockFetchResultsForCurrentActivity() {
		if (!currentActivity) return;
		console.log('Simulating results becoming available for:', currentActivity.title);
		let newResults: any = null;
		switch (currentActivity.type) {
			case 'MultipleChoice':
			case 'Poll':
				newResults = (currentActivity.definition as MultipleChoiceDefinition).options.map(
					(opt) => ({
						...opt,
						count: Math.floor(Math.random() * 20)
					})
				);
				break;
			case 'ScaleRating':
				const def = currentActivity.definition as ScaleRatingDefinition;
				newResults = [];
				for (let i = def.min; i <= def.max; i++) {
					newResults.push({ rating: i, count: Math.floor(Math.random() * 10) });
				}
				break;
			case 'OpenEnded':
				newResults = ['Great!', 'Interesting.', 'Could be clearer.', 'Loved it!'];
				break;
		}
		// Update the currentActivity with results (immutable update)
		currentActivity = {
			...currentActivity,
			results: newResults,
			responseCount: Math.floor(Math.random() * 20) + 5,
			participantCount: 25
		};
	}

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
		console.log(`Submitting answer for activity ${currentActivity?.id}:`, studentAnswer);
		await new Promise((resolve) => setTimeout(resolve, 750));
		submissionStatus = 'success';
		isSubmitting = false;
		// In a real app, results might come via WebSocket after submission or when teacher releases them.
		// For demo, let's simulate results being available after submission IF teacher has enabled them.
		// Or, teacher explicitly clicks "Show Results" which updates showResultsToStudent.
	}

	let typedDefinition = $derived(currentActivity ? getKnownDefinition(currentActivity) : null);
</script>

<div class="participation-page">
	{#if currentActivity}
		<div class="participation-page__activity-card">
			<span class="participation-page__activity-type">{currentActivity.type}</span>
			<h1 class="participation-page__activity-title">{currentActivity.title}</h1>

			{#if submissionStatus !== 'success'}
				<div class="participation-page__answer-area">
					{#if typedDefinition}
						{#if isMultipleChoice(typedDefinition)}
							<MultipleChoiceInput
								options={typedDefinition.options}
								allowMultiple={typedDefinition.allowMultiple}
								bind:selected={studentAnswer}
								disabled={isSubmitting}
							/>
						{:else if isPoll(typedDefinition)}
							<MultipleChoiceInput
								options={typedDefinition.options}
								allowMultiple={false}
								bind:selected={studentAnswer}
								disabled={isSubmitting}
							/>
						{:else if isScaleRating(typedDefinition)}
							<ScaleRatingInput
								min={typedDefinition.min}
								max={typedDefinition.max}
								minLabel={typedDefinition.minLabel}
								maxLabel={typedDefinition.maxLabel}
								bind:value={studentAnswer}
								disabled={isSubmitting}
							/>
						{:else if isOpenEnded(typedDefinition)}
							<OpenEndedInput bind:value={studentAnswer} disabled={isSubmitting} />
						{:else}
							<p class="participation-page__message participation-page__message--error">
								Unsupported activity type definition.
							</p>
						{/if}
					{:else if currentActivity.type === 'OpenEnded'}
						<OpenEndedInput bind:value={studentAnswer} disabled={isSubmitting} />
					{:else}
						<p class="participation-page__message">
							Activity definition not recognized or missing.
						</p>
					{/if}
				</div>
			{/if}

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
						Answer Submitted!
					</div>

					{#if showResultsToStudent && currentActivity.results}
						<div class="participation-page__results-display">
							<h3 class="participation-page__results-title">Activity Results</h3>
							{#if (currentActivity.type === 'MultipleChoice' || currentActivity.type === 'Poll') && isChoiceResult(currentActivity.results)}
								<ChoiceResultsDisplay
									results={currentActivity.results}
									definition={isMultipleChoice(typedDefinition) ? typedDefinition : null}
								/>
							{:else if currentActivity.type === 'ScaleRating' && isScaleRatingResult(currentActivity.results)}
								<ScaleRatingResultsDisplay
									results={currentActivity.results}
									definition={isScaleRating(typedDefinition) ? typedDefinition : null}
								/>
							{:else if currentActivity.type === 'OpenEnded' && isOpenEndedResult(currentActivity.results)}
								<OpenEndedResultsDisplay results={currentActivity.results} />
							{:else}
								<p class="participation-page__message">Results are not available in this format.</p>
							{/if}
						</div>
					{:else if showResultsToStudent && !currentActivity.results}
						<div class="participation-page__feedback">Waiting for results data...</div>
					{:else if !showResultsToStudent}
						<div class="participation-page__feedback">
							Results are currently hidden by the host.
						</div>
					{/if}
					<p class="participation-page__waiting-text">Waiting for the next activity...</p>
				{:else if submissionStatus === 'error'}
					<div class="participation-page__feedback participation-page__feedback--error">
						Submission failed. Please try again.
						<Button onclick={() => (submissionStatus = 'idle')} variant="outline" size="sm"
							>Try Again</Button
						>
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
		</div>
	{/if}

	<div class="demo-controls">
		<Button onclick={mockFetchCurrentActivity} variant="secondary" size="sm"
			>Next Activity (Demo)</Button
		>
		{#if currentActivity && submissionStatus === 'success'}
			<Button onclick={toggleStudentResultsView} variant="secondary" size="sm">
				{showResultsToStudent ? 'Hide Student Results (Demo)' : 'Show Student Results (Demo)'}
			</Button>
			{#if !currentActivity.results && showResultsToStudent}
				<Button onclick={mockFetchResultsForCurrentActivity} variant="secondary" size="sm"
					>Fetch Results (Demo)</Button
				>
			{/if}
		{/if}
	</div>
</div>

<style lang="scss">
	@import '../../../styles/variables.scss'; // Adjust path

	// Block: participation-page
	.participation-page {
		width: 100%;
		max-width: 600px;
		margin: $spacing-lg auto;
		&__activity-card {
			background-color: $color-surface;
			padding: $spacing-xl;
			border-radius: $border-radius-lg;
			box-shadow: $box-shadow-md;
			display: flex;
			flex-direction: column;
			gap: $spacing-lg;
		}
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
		&__activity-title {
			font-size: $font-size-2xl;
			font-weight: $font-weight-bold;
			color: $color-text-primary;
			margin: 0;
			line-height: 1.3;
		}
		&__answer-area {
			/* styles */
		}
		&__actions {
			margin-top: $spacing-md;
			display: flex;
			flex-direction: column;
			gap: $spacing-md;
		}
		&__feedback {
			padding: $spacing-md;
			border-radius: $border-radius-md;
			text-align: center;
			font-weight: $font-weight-medium;
			&--success {
				background-color: rgba($color-success, 0.1);
				color: darken($color-success, 10%);
				border: 1px solid rgba($color-success, 0.3);
			}
			&--error {
				background-color: rgba($color-error, 0.1);
				color: $color-error;
				border: 1px solid rgba($color-error, 0.3);
			}
		}
		&__results-display {
			margin-top: $spacing-lg;
			padding-top: $spacing-lg;
			border-top: 1px solid $color-border-light;
		}
		&__results-title {
			font-size: $font-size-lg;
			font-weight: $font-weight-semibold;
			margin-bottom: $spacing-md;
			color: $color-text-primary;
		}
		&__waiting-card {
			background-color: $color-surface;
			padding: $spacing-2xl $spacing-xl;
			border-radius: $border-radius-lg;
			box-shadow: $box-shadow-md;
			text-align: center;
		}
		&__waiting-title {
			font-size: $font-size-xl;
			color: $color-text-secondary;
			margin-bottom: $spacing-lg;
		}
		&__waiting-text {
			font-size: $font-size-md;
			color: $color-text-secondary;
			margin-bottom: $spacing-lg;
		}
		&__spinner {
			margin: $spacing-lg auto;
			border: 4px solid $color-surface-alt;
			border-top: 4px solid $color-primary;
			border-radius: 50%;
			width: 40px;
			height: 40px;
			animation: participation-page__spin 1s linear infinite;
		}
		&__message {
			font-style: italic;
			color: $color-text-secondary;
			padding: $spacing-md;
			background-color: $color-surface-alt;
			border-radius: $border-radius-sm;
			text-align: center;
			&--error {
				color: $color-error;
				background-color: rgba($color-error, 0.05);
				border: 1px solid rgba($color-error, 0.1);
			}
		}
	}

	@keyframes participation-page__spin {
		0% {
			transform: rotate(0deg);
		}
		100% {
			transform: rotate(360deg);
		}
	}

	// Demo controls styling (remove in production)
	.demo-controls {
		margin-top: $spacing-xl;
		padding: $spacing-md;
		background-color: $color-surface-alt;
		border-radius: $border-radius-md;
		display: flex;
		flex-direction: column;
		gap: $spacing-sm;
		align-items: flex-start;
		border: 1px dashed $color-border;
	}
</style>
