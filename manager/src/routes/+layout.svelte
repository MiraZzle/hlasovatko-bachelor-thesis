<script lang="ts">
	import '$styles/global.scss';
	import Toaster from '$components/elements/typography/Toaster.svelte';
</script>

<Toaster />

<div id="page-content">
	<slot />
</div>
