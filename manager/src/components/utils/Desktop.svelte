<script>
    import MediaQuery from '$components/utils/MediaQuery.svelte';
</script>

<MediaQuery query="(min-width: 951px)" let:matches>
    {#if matches}
        <slot />
    {/if}
</MediaQuery>
