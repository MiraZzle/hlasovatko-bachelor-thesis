<script lang="ts">
	// No specific definition needed for props currently
</script>

<div class="activity-display activity-display--open-ended">
	<p class="activity-display__placeholder-text">(Participants will provide a text response)</p>
</div>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

	.activity-display {
		margin-top: $spacing-sm;
		&__placeholder-text {
			font-style: italic;
			color: $color-text-secondary;
			font-size: $font-size-sm;
			background-color: $color-surface-alt;
			padding: $spacing-md;
			border-radius: $border-radius-md;
			margin: 0;
		}
	}
</style>
