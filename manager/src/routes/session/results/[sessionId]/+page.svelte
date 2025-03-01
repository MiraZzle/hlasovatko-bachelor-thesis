<script lang="ts">
	import { onDestroy } from 'svelte';
	import { API_URL } from '$lib/config';
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();

	type SessionAnswer = {
		questionId: string;
		selectedOption: string;
	};

	let sessionId: string = data.sessionId;
	let answers = $state<SessionAnswer[]>([]);
	let errorMessage = $state<string | null>(null);
	let loading = $state(true);
	let intervalId: any;

	async function fetchAnswers() {
		loading = true;
		console.log('fetching answers');
		try {
			const response = await fetch(`${API_URL}/api/answers/session/${sessionId}`);
			if (!response.ok) {
				throw new Error('Failed to fetch answers');
			}
			answers = await response.json();
		} catch (error) {
			errorMessage = (error as Error).message;
		} finally {
			loading = false;
		}
	}

	fetchAnswers();
	intervalId = setInterval(fetchAnswers, 5000);

	onDestroy(() => {
		clearInterval(intervalId);
	});
</script>

<div class="answer-container"></div>

{#if loading}
	<p>Loading answers...</p>
{:else if errorMessage}
	<p style="color: red;">{errorMessage}</p>
{:else}
	<h2>Submitted Answers</h2>
	<ul>
		{#each answers as answer}
			<li>
				<strong>Question ID:</strong>
				{answer.questionId} <br />
				<strong>Selected Option:</strong>
				{answer.selectedOption}
			</li>
		{/each}
	</ul>
{/if}
