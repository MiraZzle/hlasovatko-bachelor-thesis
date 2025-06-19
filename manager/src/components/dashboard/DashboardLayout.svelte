<script lang="ts">
	/**
	 * @file Dashboard layout component.
	 * Provides a main content area with a top bar and slot for content.
	 */
</script>

<div class="dashboard-layout__main">
	<slot name="topbar" />
	<main class="dashboard-layout__content">
		<slot name="content" />
	</main>
</div>

<style lang="scss">
	.dashboard-layout__main {
		flex-grow: 1;
		display: flex;
		flex-direction: column;
		height: 100vh;
		overflow-y: hidden;
	}

	.dashboard-layout__content {
		flex-grow: 1;
		padding: $spacing-xl;
		overflow-y: auto;
	}
</style>
