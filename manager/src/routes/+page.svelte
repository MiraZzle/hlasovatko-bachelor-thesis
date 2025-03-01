<script>
	import { goto } from '$app/navigation';
	import Header from '$components/elements/typography/Header.svelte';
	import Button from '$components/elements/typography/Button.svelte';
</script>

<div class="action-container">
	<Header>Hlasovatko</Header>
	<Button action={() => goto('/session/create')}>Create a session</Button>
</div>

<style lang="scss">
	.action-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		min-height: 100vh;
		max-width: 960px;
		gap: 16px;
		margin: auto;
	}
</style>
