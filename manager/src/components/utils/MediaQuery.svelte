<script lang="ts">
	/**
	 * @file MediaQuery component that listens to media query changes and provides the match status.
	 * It allows conditional rendering based on the media query.
	 */
	import { onMount } from 'svelte';

	let {
		query = '(max-width: 950px)' // Default query for mobile devices
	}: {
		query?: string;
	} = $props();

	let queryList: MediaQueryList;
	let mqlListener: (e: MediaQueryListEvent) => void;
	let matches = $state(false);

	// Ensure the query is a valid media query string
	onMount(() => {
		destroyQueryListener();
		createQueryListener(query);

		return () => {
			destroyQueryListener();
		};
	});

	function createQueryListener(query: string) {
		queryList = window.matchMedia(query);
		matches = queryList.matches;

		mqlListener = (e) => (matches = e.matches);
		queryList.addEventListener('change', mqlListener);
	}

	function destroyQueryListener() {
		if (queryList && mqlListener) {
			queryList.removeEventListener('change', mqlListener);
		}
	}
</script>

<slot {matches} />
