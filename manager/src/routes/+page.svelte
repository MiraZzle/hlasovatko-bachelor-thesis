<script>
	import { goto } from '$app/navigation';
</script>

<h1>Hlasovatko</h1>
<button on:click={() => goto('/session/create')}>Create a session</button>
