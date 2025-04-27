<script lang="ts">
    import { onMount } from 'svelte';

    export let query: string;

    let mql: MediaQueryList;
    let mqlListener: (e: MediaQueryListEvent) => void;
    let matches = false;

    onMount(() => {
        removeActiveListener();
        addNewListener(query);

        return () => {
            removeActiveListener();
        };
    });

    function addNewListener(query: string) {
        mql = window.matchMedia(query);
        matches = mql.matches;

        mqlListener = (e) => (matches = e.matches);
        mql.addEventListener('change', mqlListener);
    }

    function removeActiveListener() {
        if (mql && mqlListener) {
            mql.removeEventListener('change', mqlListener);
        }
    }
</script>

<slot {matches} />
