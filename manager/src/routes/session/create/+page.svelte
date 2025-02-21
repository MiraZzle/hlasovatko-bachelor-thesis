<script lang="ts">
	import { writable } from 'svelte/store';
	import { API_URL, STUDENT_FE_URL } from '$lib/config';
	import QRCode from 'qrcode';

	let sessionId = writable<string | null>(null);
	let jsonInput = writable<string>('');
	let errorMessage = writable<string | null>(null);
	let qrCodeUrl = writable<string | null>(null);

	async function createSession() {
		errorMessage.set(null);
		qrCodeUrl.set(null); // Reset QR code

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
			generateQRCode(`${STUDENT_FE_URL}/session/${data.sessionId}`);
		} else {
			errorMessage.set('Failed to create session.');
		}
	}

	async function generateQRCode(url: string) {
		try {
			const qrDataUrl = await QRCode.toDataURL(url);
			qrCodeUrl.set(qrDataUrl);
		} catch (err) {
			console.error('Error generating QR code:', err);
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
	<h2>Student Join Link:</h2>
	<a href={`${STUDENT_FE_URL}/session/${$sessionId}`} target="_blank">
		${STUDENT_FE_URL}/session/{$sessionId}
	</a>

	<h3>QR Code:</h3>
	{#if $qrCodeUrl}
		<img src={$qrCodeUrl} alt="QR Code to join session" />
	{/if}
{/if}
