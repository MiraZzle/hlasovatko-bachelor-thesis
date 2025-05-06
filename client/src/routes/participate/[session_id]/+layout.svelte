<script lang="ts">
	// Import global styles if they are not already handled by app.html or a root layout
	// import '../../../styles/global.scss'; // Adjust path if your global.scss is elsewhere

	// This layout could also fetch initial session validity or branding in a load function
</script>

<div class="participation-layout">
	<slot />
</div>

<style lang="scss">
	@import '../../../styles/variables.scss'; // Adjust path depth

	.participation-layout {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: flex-start; // Align content to the top
		min-height: 100vh;
		background-color: $color-background; // Or a specific theme for participation
		padding: $spacing-md; // Basic padding for the overall page

		@media (min-width: $breakpoint-sm) {
			padding: $spacing-lg;
		}
	}
</style>
