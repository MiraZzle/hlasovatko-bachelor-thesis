<script lang="ts">
	import { onMount } from 'svelte';
	import { API_URL } from '$lib/config';
	import type { Session, Question } from '$lib/types';
	import type { PageProps } from './$types';

	export let data: { session: Session | null };

	let session: Session | null = data?.session || null;
	let errorMessage: string | null = null;
	let answers: Record<number, number> = {}; // Key = question index, Value = selected option index

	async function submitAnswers() {
		if (!session) return;

		try {
			const response = await fetch(`${API_URL}/api/sessions/${session.sessionId}/submit`, {
				method: 'POST',
				headers: { 'Content-Type': 'application/json' },
				body: JSON.stringify({ answers })
			});

			if (!response.ok) {
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

			{#each activity.questions as question, qIndex}
				<p>{question.question}</p>
				{#each question.options as option, oIndex}
					<label>
						<input type="radio" name={`q${qIndex}`} on:change={() => (answers[qIndex] = oIndex)} />
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
