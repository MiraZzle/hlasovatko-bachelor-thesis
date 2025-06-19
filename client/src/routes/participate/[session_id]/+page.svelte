<script lang="ts">
	/**
	 * @file A test page to demonstrate the usage of the ActivityInputItem component.
	 * It loads a list of activities and allows a user to cycle through them,
	 * submitting an answer for each one.
	 */
	import { onMount } from 'svelte';
	import { getActivitiesFromSession } from '$lib/activities/activity_utils';
	import type { Activity } from '$lib/activities/types';
	import ActivityInputItem from '$components/activities/input/ActivityInputItem.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import SessionAnalyticsItem from '$components/analytics/SessionAnalyticsItem.svelte';
	import type { ActivityResult } from '$lib/activities/types';
	import { getActivityResultsForSession } from '$lib/analytics/analytics_utils';
	import type { SubmitPayload } from '$lib/activities/definition_types';
	import { submitActivityAnswer } from '$lib/activities/activity_utils';
	import type { Session } from '$lib/sessions/types';
	import { getSessionById } from '$lib/sessions/session_utils';
	import { getSessionSettings } from '$lib/sessions/session_utils';
	import { page } from '$app/stores';

	const session_id = $page.params.session_id;
	let allActivities = $state<Activity[]>([]);
	let allActivityResults = $state<ActivityResult[]>([]);
	let currentIndex = $state(0);
	let sessionInfo = $state<Session | null>(getSessionById(session_id));
	let sessionSettings = $state(getSessionSettings(session_id));

	// Map to store submited answers
	let submittedAnswers = $state<Record<string, any>>({});

	// Flag to check if session allows participants to see answers - defaults to true
	let participantAnswersAllowed = $state(sessionSettings.allowParticipantAnswers ?? true);

	// State to control the visibility of the results
	let revealAnswers = $state(false);

	// Fetch all necessary data on mount
	onMount(() => {
		allActivities = getActivitiesFromSession('mock_session_id');
		allActivityResults = getActivityResultsForSession('mock_session_id');
	});

	const currentActivity = $derived(allActivities[currentIndex]);
	const hasSubmitted = $derived(currentActivity?.id in submittedAnswers);

	// Dynamically find the current activity result based on the current activity - we get all answers at once
	const currentActivityResult = $derived(
		allActivityResults.find((result) => result.activityRef.id === currentActivity?.id) ?? null
	);

	/**
	 * Advances to the next activity in the list and resets the state.
	 */
	function goToNextActivity(): void {
		if (currentIndex < allActivities.length - 1) {
			currentIndex++;
			revealAnswers = false; // Hide results for the new activity
		} else {
			alert("You've reached the end of the activities!");
		}
	}

	/**
	 * Handles the submission of an activity answer.
	 * @param payload - The payload containing the activity ID and submit value
	 */
	async function handleActivitySubmit(payload: SubmitPayload): Promise<void> {
		await submitActivityAnswer(payload);

		submittedAnswers[payload.activityId] = payload.value;
		revealAnswers = true; // Show results after submission
	}
</script>

<svelte:head>
	<title>EngaGenie | Participate</title>
</svelte:head>

<div class="participate-page">
	<header class="participate-page__header">
		<h1 class="participate-page__title">Activity Participation</h1>
	</header>

	<main class="participate-page__main">
		{#if currentActivity}
			<ActivityInputItem
				activity={currentActivity}
				onSubmit={handleActivitySubmit}
				disabled={hasSubmitted}
			/>
		{:else}
			<p>Loading activities...</p>
		{/if}
	</main>

	{#if hasSubmitted}
		<div class="participate-page__results-control">
			{#if revealAnswers && currentActivityResult && participantAnswersAllowed}
				<div class="participate-page__results-display">
					<SessionAnalyticsItem activityResult={currentActivityResult} />
				</div>
			{/if}
		</div>
	{/if}

	<footer class="participate-page__footer">
		<div class="participate-page__navigation">
			<span>Activity {currentIndex + 1} of {allActivities.length}</span>
			<Button onclick={goToNextActivity} disabled={currentIndex >= allActivities.length - 1}>
				Next Activity →
			</Button>
		</div>

		<!-- Debug -->
		<!--
		{#if Object.keys(submittedAnswers).length > 0}
			<div class="participate-page__results-viewer">
				<h3>Your Submitted Answers</h3>
				<pre>{JSON.stringify(submittedAnswers, null, 2)}</pre>
			</div>
		{/if}
		-->
	</footer>
</div>

<style lang="scss">
	.participate-page {
		max-width: 768px;
		margin: 2rem auto;
		padding: 2rem;
		font-family: sans-serif;
		display: flex;
		flex-direction: column;
		gap: 2rem;

		&__header {
			text-align: center;
			border-bottom: 1px solid #e0e0e0;
			padding-bottom: 1rem;

			h1 {
				margin: 0;
				font-size: 2rem;
			}
			p {
				margin: 0.5rem 0 0;
				color: #666;
			}
		}

		&__results-control {
			display: flex;
			flex-direction: column;
			align-items: center;
			gap: 1.5rem;
			padding: 1.5rem;
			border: 1px dashed #ccc;
			border-radius: 8px;
			background-color: #fafafa;
		}

		&__results-display {
			width: 100%;
		}

		&__footer {
			margin-top: 1rem;
			display: flex;
			flex-direction: column;
			gap: 1.5rem;
		}

		&__navigation {
			display: flex;
			justify-content: space-between;
			align-items: center;
			padding: 1rem;
			background-color: #f8f9fa;
			border-radius: 8px;
		}

		&__results-viewer {
			background-color: #2d2d2d;
			color: #f1f1f1;
			border-radius: 8px;
			padding: 1rem;

			h3 {
				margin: 0 0 1rem;
				border-bottom: 1px solid #444;
				padding-bottom: 0.5rem;
			}

			pre {
				white-space: pre-wrap;
				word-wrap: break-word;
			}
		}
	}
</style>
