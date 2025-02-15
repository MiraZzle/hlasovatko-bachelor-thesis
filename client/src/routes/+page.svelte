<script lang="ts">
	import { goto } from '$app/navigation';

	let sessionId: string = '';

	function joinSession() {
		if (sessionId.trim()) {
			goto(`/event/${sessionId}`);
		}
	}
</script>

<main>
	<h1>Join a Session</h1>
	<input
		type="text"
		placeholder="Enter Session ID"
		bind:value={sessionId}
		on:keydown={(e) => e.key === 'Enter' && joinSession()}
	/>
	<button on:click={joinSession}>Join</button>
</main>

<style>
	main {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		height: 100vh;
	}

	input {
		padding: 10px;
		font-size: 16px;
		margin-bottom: 10px;
		width: 250px;
	}

	button {
		padding: 10px 20px;
		font-size: 16px;
		cursor: pointer;
	}
</style>
