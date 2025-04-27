<script lang="ts">
	// Placeholders for icons - replace with actual icon components or SVGs
	const NoRegIcon = '🔗';
	const OpenSourceIcon = '<>';
	const PrivacyIcon = '🔒';
</script>

<section class="benefits-section">
	<div class="page-container benefits-section__container">
		<div class="benefit-card">
			<div class="benefit-card__icon">{NoRegIcon}</div>
			<h3 class="benefit-card__title">No Registration Required</h3>
			<p class="benefit-card__description">
				Students can join instantly using a simple code or link, ensuring a high participation rate.
			</p>
		</div>

		<div class="benefit-card">
			<div class="benefit-card__icon">{OpenSourceIcon}</div>
			<h3 class="benefit-card__title">Open Source & Extensible</h3>
			<p class="benefit-card__description">
				Hlasovátko is open-source and offers a flexible API, integrating with university analytic
				systems.
			</p>
		</div>

		<div class="benefit-card">
			<div class="benefit-card__icon">{PrivacyIcon}</div>
			<h3 class="benefit-card__title">Privacy & Accessibility First</h3>
			<p class="benefit-card__description">
				Supports anonymous participation and is WCAG compliant (or aims to be).
			</p>
		</div>
	</div>
</section>

<style lang="scss">
	@import '../../styles/variables.scss';

	.benefits-section {
		background-color: $color-surface-alt; // Alternate background
		padding: $spacing-3xl 0;
	}

	.benefits-section__container {
		display: grid;
		grid-template-columns: repeat(auto-fit, minmax(280px, 1fr)); // Responsive grid
		gap: $spacing-xl; // Gap between cards

		@media (max-width: $breakpoint-sm) {
			grid-template-columns: 1fr; // Stack on mobile
		}
	}

	.benefit-card {
		background-color: $color-surface; // Card background
		padding: $spacing-xl;
		border-radius: $border-radius-lg;
		box-shadow: $box-shadow-sm;
		text-align: center; // Center content within card

		&__icon {
			font-size: $font-size-3xl; // Make icon larger
			margin-bottom: $spacing-md;
			color: $color-primary; // Use primary color for icon
			// If using SVG icons, style them here (width, height, fill/stroke)
			display: inline-block; // Needed if just text
		}

		&__title {
			font-size: $font-size-xl;
			font-weight: $font-weight-semibold;
			margin-bottom: $spacing-sm;
			color: $color-text-primary;
		}

		&__description {
			font-size: $font-size-md;
			color: $color-text-secondary;
			line-height: $line-height-base;
			margin-bottom: 0; // Remove default paragraph margin
		}
	}
</style>
