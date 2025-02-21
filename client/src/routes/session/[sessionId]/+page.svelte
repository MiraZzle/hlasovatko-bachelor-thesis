<script lang="ts">
	import { onMount } from 'svelte';
	import { API_URL } from '$lib/config';
	import type { Session, QuizAnswer } from '$lib/types';

	export let data: { session: Session | null };

	let session: Session | null = data?.session || null;
	let errorMessage: string | null = null;
	let answers: Record<string, number> = {}; // Key = questionId, Value = selected option index
	let currentActivityIndex = 0;

	async function submitAnswers() {
		if (!session) return;

		const formattedAnswers: QuizAnswer[] = [];
		for (const activity of session.activities) {
			if (activity.activityType === 'Quiz') {
				for (const question of activity.questions) {
					if (answers[question.questionId] !== undefined) {
						formattedAnswers.push({
							sessionId: session.sessionId,
							activityId: activity.activityId,
							questionId: question.questionId,
							selectedOption:
								activity.questions.find((q) => q.questionId === question.questionId)?.options[
									answers[question.questionId]
								] || ''
						});
					}
				}
			}
		}

		try {
			const response = await fetch(`${API_URL}/api/answers/submit`, {
				method: 'POST',
				headers: { 'Content-Type': 'application/json' },
				body: JSON.stringify({
					answers: formattedAnswers,
					sessionId: session.sessionId,
					activityId: session.activities[currentActivityIndex].activityId,
					answerType: session.activities[currentActivityIndex].activityType
				})
			});

			console.log(
				JSON.stringify({
					answers: formattedAnswers,
					sessionId: session.sessionId,
					activityId: session.activities[currentActivityIndex].activityId,
					answerType: 'Quiz'
				})
			);

			if (!response.ok) {
				console.error('Failed to submit answers:', response);
				throw new Error('Failed to submit answers');
			}

			alert('Answers submitted successfully!');
		} catch (error) {
			errorMessage = (error as Error).message;
		}
	}
</script>

{#if session}
	<h1>{session.title}</h1>

	{#each session.activities as activity}
		<div>
			<h2>{activity.title}</h2>

			{#each activity.questions as question}
				<p>{question.question}</p>
				{#each question.options as option, oIndex}
					<label>
						<input
							type="radio"
							name={question.questionId}
							on:change={() => (answers[question.questionId] = oIndex)}
						/>
						{option}
					</label>
				{/each}
			{/each}
		</div>
	{/each}

	<button on:click={submitAnswers}>Submit Answers</button>
{:else}
	<p>Loading session...</p>
{/if}

{#if errorMessage}
	<p style="color: red;">{errorMessage}</p>
{/if}
