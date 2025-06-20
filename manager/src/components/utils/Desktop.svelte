<script>
	/**
	 * @file Desktop component that conditionally renders its content based on the screen width.
	 */
	import MediaQuery from '$components/utils/MediaQuery.svelte';
</script>

<MediaQuery query="(min-width: 951px)" let:matches>
	{#if matches}
		<slot />
	{/if}
</MediaQuery>
