<script>
	/**
	 * @file Mobile component that conditionally renders its content based on the screen width.
	 */
	import MediaQuery from '$components/utils/MediaQuery.svelte';
</script>

<MediaQuery query="(max-width: 950px)" let:matches>
	{#if matches}
		<slot />
	{/if}
</MediaQuery>
