<script lang="ts">
	/**
	 * @file Client participation page.
	 * Implements the full flow for both student- and teacher-paced sessions.
	 */
	import { onMount, onDestroy } from 'svelte';
	import { page } from '$app/stores';
	import { getActivityResults, submitAnswer } from '$lib/activities/activity_utils';
	import type { Activity, ActivityResult } from '$lib/activities/types';
	import type { SubmitPayload } from '$lib/activities/definition_types';
	import ActivityInputItem from '$components/activities/input/ActivityInputItem.svelte';
	import SessionAnalyticsItem from '$components/analytics/SessionAnalyticsItem.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import { getSessionById } from '$lib/sessions/session_utils';
	import type { Session } from '$lib/sessions/types';
	import { getActivitiesFromSession, getSessionState } from '$lib/sessions/session_utils';
	import type { ParticipantSessionState } from '$lib/sessions/session_utils';
	import type { StaticActivityType } from '$lib/activities/types';

	const session_id = $page.params.session_id;

	let sessionInfo: Session | null = $state(null);
	let allActivities: Activity[] = $state([]);
	let currentActivity: Activity | null = $state(null);
	let currentResults: ActivityResult | null = $state(null);

	let studentPacedIndex = $state(0);
	let sessionState: ParticipantSessionState | null = $state(null);

	let submittedAnswers = $state<Record<string, boolean>>({});
	let isLoading = $state(true);
	let errorMessage: string | null = $state(null);

	let statePollInterval: any;
	let resultsPollInterval: any;

	let isStudentPaced = $state(false);
	let hasSubmitted = $state(false);
	let showResults = $state(true);

	onMount(async () => {
		sessionInfo = await getSessionById(session_id);

		if (!sessionInfo) {
			errorMessage = 'Session not found.';
			isLoading = false;
			return;
		}

		isStudentPaced = sessionInfo!.mode === 'student-paced';

		allActivities = await getActivitiesFromSession(session_id);
		isLoading = false;

		if (!isStudentPaced) {
			pollState();
			statePollInterval = setInterval(pollState, 3000);
		}

		if (sessionState) {
			showResults = sessionState.showResults;
		}
	});

	onDestroy(() => {
		clearInterval(statePollInterval);
		clearInterval(resultsPollInterval);
	});

	$effect(() => {
		if (currentActivity) {
			hasSubmitted = submittedAnswers[currentActivity.id] ?? false;
		} else {
			hasSubmitted = false;
		}
	});

	$effect(() => {
		if (isLoading || !sessionInfo) return;

		let activeId: string | undefined | null;
		if (isStudentPaced) {
			activeId = allActivities[studentPacedIndex]?.id;
		} else {
			activeId = sessionState?.currentActivityId;
		}

		currentActivity = allActivities.find((a) => a.id === activeId) ?? null;

		if (currentActivity) {
			submittedAnswers[currentActivity.id] = submittedAnswers[currentActivity.id] || false;
		}
	});

	$effect(() => {
		clearInterval(resultsPollInterval);
		currentResults = null;

		if (showResults && currentActivity && sessionInfo) {
			if (isStudentPaced && !hasSubmitted) return;

			pollResults();
			resultsPollInterval = setInterval(pollResults, 5000);
		}
	});

	async function pollState() {
		sessionState = await getSessionState(session_id);
	}

	async function pollResults() {
		if (currentActivity) {
			currentResults = await getActivityResults(session_id, currentActivity.id);
		}
	}

	async function handleActivitySubmit(payload: SubmitPayload) {
		const fullPayload: SubmitPayload = {
			...payload,
			activityType: currentActivity!.type as StaticActivityType
		};

		const success = await submitAnswer(session_id, fullPayload);
		if (success) {
			submittedAnswers = { ...submittedAnswers, [payload.activityId]: true };
		} else {
			alert('Error submitting answer.');
		}
	}

	function goToNext() {
		if (studentPacedIndex < allActivities.length - 1) studentPacedIndex++;
	}

	function goToPrev() {
		if (studentPacedIndex > 0) studentPacedIndex--;
	}
</script>

<svelte:head>
	<title>Participate | {sessionInfo?.title ?? 'Session'}</title>
</svelte:head>

<div class="participate-page">
	{#if isLoading}
		<p>Joining session...</p>
	{:else if errorMessage}
		<div class="error-message">{errorMessage}</div>
	{:else if sessionInfo}
		<header class="participate-page__header">
			<h1 class="participate-page__title">{sessionInfo.title}</h1>
			{#if isStudentPaced}
				<p>Activity {studentPacedIndex + 1} of {allActivities.length}</p>
			{/if}
		</header>

		<main class="participate-page__main">
			{#if currentActivity}
				<ActivityInputItem
					activity={currentActivity}
					disabled={hasSubmitted}
					onSubmit={handleActivitySubmit}
				/>
			{:else}
				<p>Waiting for the host to start the next activity...</p>
			{/if}
		</main>

		{#if hasSubmitted && showResults}
			<div class="results-display">
				{#if currentResults}
					<SessionAnalyticsItem activityResult={currentResults} />
				{:else}
					<p>Loading results...</p>
				{/if}
			</div>
		{/if}

		{#if isStudentPaced}
			<footer class="participate-page__footer">
				<Button onclick={goToPrev} disabled={studentPacedIndex === 0}>Previous</Button>
				<Button onclick={goToNext} disabled={studentPacedIndex >= allActivities.length - 1}
					>Next</Button
				>
			</footer>
		{/if}
	{/if}
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
