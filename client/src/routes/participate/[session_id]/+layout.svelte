<div class="participation-layout">
	<slot />
</div>

<style lang="scss">
	.participation-layout {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: flex-start;
		min-height: 100vh;
		background-color: $color-background;
		padding: $spacing-md;

		@media (min-width: $breakpoint-sm) {
			padding: $spacing-lg;
		}
	}
</style>
