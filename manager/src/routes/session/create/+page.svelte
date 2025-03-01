<script lang="ts">
	import { writable } from 'svelte/store';
	import { API_URL, STUDENT_FE_URL } from '$lib/config';
	import QRCode from 'qrcode';
	import Button from '$components/elements/typography/Button.svelte';
	import Header from '$components/elements/typography/Header.svelte';
	import TextArea from '$components/elements/typography/utils/TextArea.svelte';

	let sessionId = writable<string | null>(null);
	let jsonInput = writable<string>('');
	let errorMessage = writable<string | null>(null);
	let qrCodeUrl = writable<string | null>(null);

	async function createSession() {
		errorMessage.set(null);
		qrCodeUrl.set(null);

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

<div class="container container--centered">
	<Header>Create a new session</Header>
	<TextArea bind:value={$jsonInput} placeholder="Paste session JSON here..." rows={10} cols={50} />
	<Button action={createSession}>Create Session</Button>

	{#if $errorMessage}
		<p class="error-message">{$errorMessage}</p>
	{/if}

	{#if $sessionId}
		<Header type={2}>Session ID:</Header>
		<p>{$sessionId}</p>
		<Header type={2}>Student Join Link:</Header>
		<a href={`${STUDENT_FE_URL}/session/${$sessionId}`} target="_blank">
			{STUDENT_FE_URL}/session/{$sessionId}
		</a>

		<Header type={2}>QR Code:</Header>
		{#if $qrCodeUrl}
			<img src={$qrCodeUrl} alt="QR Code to join session" />
		{/if}

		<Header type={2}>Results Link:</Header>
		<a href={`/session/results/${$sessionId}`} target="_blank">Live results</a>
	{/if}
</div>

<style lang="scss">
	.container {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		min-height: 100vh;
		max-width: 960px;
		gap: 16px;
		margin: auto;
	}

	.error-message {
		color: red;
	}
</style>
