<script lang="ts">
	/**
	 * @file Client participation page.
	 * Implements the full flow for interactive sessions.
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
	import { toast } from '$lib/stores/toast_store';
	import { MANAGER_FE_URL } from '$lib/config';

	const sessionId = $page.params.session_id;

	// state management
	let sessionInfo: Session | null = $state(null);
	let allActivities: Activity[] = $state([]);
	let currentActivity: Activity | null = $state(null);
	let currentResults: ActivityResult | null = $state(null);

	let studentPacedIndex = $state(0);
	let sessionState: ParticipantSessionState | null = $state(null);
	let submittedAnswers = $state<Record<string, boolean>>({});
	let isLoading = $state(true);
	let errorMessage: string | null = $state(null);

	let statePollTimeoutId: any;
	let resultsPollTimeoutId: any;

	let isStudentPaced = $state(false);
	let hasSubmitted = $state(false);
	let showResults = $state(true);
	let allActivitiesDone = $derived(isStudentPaced && studentPacedIndex >= allActivities.length);

	// polling intervals
	const STATE_POLLING_INTERVAL = 3000;
	const RESULTS_POLLING_INTERVAL = 5000;
	const JITTER_AMOUNT = 1000; // Max random jitter in ms

	onMount(async () => {
		try {
			sessionInfo = await getSessionById(sessionId);

			if (!sessionInfo) {
				errorMessage = 'Session not found';
				isLoading = false;
				return;
			}

			isStudentPaced = sessionInfo!.mode === 'student-paced';

			if (sessionInfo.status.toLowerCase() !== 'active' && isStudentPaced) {
				errorMessage = 'This session is not currently active.';
				isLoading = false;
				return;
			}

			try {
				allActivities = await getActivitiesFromSession(sessionId);
			} catch (error) {
				errorMessage = 'Could not load activities for this session.';
				isLoading = false;
				return;
			}

			if (isStudentPaced) {
				sessionState = await getSessionState(sessionId);
			} else {
				pollStateWithJitter();
			}

			if (sessionState) {
				showResults = sessionState.showResults;
			}
		} catch (error) {
			console.error('Failed to initialize session:', error);
			errorMessage = 'Could not load the session.';
		} finally {
			isLoading = false;
		}
	});

	onDestroy(() => {
		clearTimeout(statePollTimeoutId);
		clearTimeout(resultsPollTimeoutId);
	});

	$effect(() => {
		if (sessionState) {
			showResults = sessionState.showResults;
		}
	});

	$effect(() => {
		if (currentActivity) {
			hasSubmitted = submittedAnswers[currentActivity.id] ?? false;
		} else {
			hasSubmitted = false;
		}
	});

	$effect(() => {
		if (isLoading || !sessionInfo || allActivities.length === 0) {
			return;
		}
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
		clearTimeout(resultsPollTimeoutId);
		currentResults = null;

		if (showResults && currentActivity && sessionInfo) {
			if (isStudentPaced && !hasSubmitted) return;
			pollResultsWithJitter();
		}
	});

	/**
	 * Navigates the user back to the manager page.
	 */
	function navigateHome() {
		window.location.href = MANAGER_FE_URL;
	}
	/**
	 * Fetches the session state
	 */
	async function pollStateWithJitter(): Promise<void> {
		sessionState = await getSessionState(sessionId);

		const jitter = Math.random() * JITTER_AMOUNT;
		statePollTimeoutId = setTimeout(pollStateWithJitter, STATE_POLLING_INTERVAL + jitter);
	}

	/**
	 * Fetches activity results
	 */
	async function pollResultsWithJitter(): Promise<void> {
		if (currentActivity) {
			currentResults = await getActivityResults(sessionId, currentActivity.id);
		}

		const jitter = Math.random() * JITTER_AMOUNT;
		resultsPollTimeoutId = setTimeout(pollResultsWithJitter, RESULTS_POLLING_INTERVAL + jitter);
	}

	async function handleActivitySubmit(payload: SubmitPayload): Promise<void> {
		const fullPayload: SubmitPayload = {
			...payload,
			activityType: currentActivity!.type as StaticActivityType
		};

		const success = await submitAnswer(sessionId, fullPayload);
		if (success) {
			submittedAnswers = { ...submittedAnswers, [payload.activityId]: true };
			toast.show('Answer submitted successfully.', 'success');
		} else {
			toast.show('Error submitting answer.', 'error');
		}
	}

	function goToNext() {
		if (studentPacedIndex < allActivities.length) studentPacedIndex++;
	}

	function goToPrevious() {
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
			{#if isStudentPaced && !allActivitiesDone}
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
			{:else if allActivitiesDone}
				<p class="participate-page__message">You have completed all activities! 🎉</p>
			{:else}
				<p class="participate-page__message">No activity is currently running.</p>
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

		{#if isStudentPaced && !allActivitiesDone}
			<footer class="participate-page__footer">
				<Button onclick={goToPrevious} disabled={studentPacedIndex <= 0} variant={'outline'}
					>Previous</Button
				>
				<Button
					onclick={goToNext}
					disabled={studentPacedIndex >= allActivities.length || !hasSubmitted}
					variant={'outline'}>Next</Button
				>
			</footer>
		{:else if allActivitiesDone}
			<footer class="participate-page__footer--centered">
				<Button onclick={navigateHome} variant={'primary'} fullWidth>Go Home</Button>
			</footer>
		{/if}
	{/if}
</div>

<style lang="scss">
	.participate-page {
		max-width: $container-max-width-sm;
		width: 100%;
		margin: 2rem auto;
		padding: 2rem;
		font-family: sans-serif;
		display: flex;
		flex-direction: column;
		gap: 2rem;

		&__message {
			text-align: center;
			font-size: 1.2rem;
		}

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

		&__footer {
			margin-top: 1rem;
			display: flex;
			flex-direction: row;
			justify-content: space-between;
			gap: 1.5rem;
		}
	}
</style>
