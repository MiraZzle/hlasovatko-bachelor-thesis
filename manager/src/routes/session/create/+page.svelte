<script lang="ts">
	import { writable } from 'svelte/store';
	import { API_URL } from '$lib/config';
	import { STUDENT_FE_URL } from '$lib/config';

	let sessionId = writable<string | null>(null);
	let jsonInput = writable<string>('');
	let errorMessage = writable<string | null>(null);

	async function createSession() {
		errorMessage.set(null);
		console.log(API_URL);

		let sessionData;
		try {
			sessionData = JSON.parse($jsonInput);
		} catch (e) {
			errorMessage.set('Invalid JSON format. Please check your input.');
			return;
		}

		const res = await fetch(`${API_URL}/api/sessions/create`, {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify(sessionData)
		});

		const data = await res.json();
		if (res.ok) {
			sessionId.set(data.sessionId);
		} else {
			errorMessage.set('Failed to create session.');
		}
	}
</script>

<h1>Create a New Session</h1>

<textarea bind:value={$jsonInput} placeholder="Paste session JSON here..." rows="10" cols="50"
></textarea>
<br />

<button on:click={createSession}>Create Session</button>

{#if $errorMessage}
	<p style="color: red;">{$errorMessage}</p>
{/if}

{#if $sessionId}
	<p>Share this link with students:</p>
	<a href={`${STUDENT_FE_URL}/event/${$sessionId}`} target="_blank">
		https://yourgithubusername.github.io/student?sessionId={$sessionId}
	</a>
{/if}
