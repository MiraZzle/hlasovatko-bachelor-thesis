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
