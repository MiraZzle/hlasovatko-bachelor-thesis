<script lang="ts">
	/**
	 * @file Benefits section for the landing page.
	 * Displays key advantages of using EngaGenie.
	 */
	import BenefitCard from '$components/elements/landing_utils/BenefitCard.svelte';

	const NoRegIcon = '🔓';
	const OpenSourceIcon = '🧩';
	const PrivacyIcon = '🛡️';
</script>

<section class="benefits-section">
	<div class="page-container benefits-section__container">
		<BenefitCard
			title="No Registration Required"
			description="Students can join instantly using a simple code or link - no accounts needed."
			icon={NoRegIcon}
		/>
		<BenefitCard
			title="Open Source & Extensible"
			description="EngaGenie is 100% open-source and offers a flexible API and custom activity types for developers."
			icon={OpenSourceIcon}
		/>
		<BenefitCard
			title="Privacy & Accessibility First"
			description="Privacy-respecting by design. No tracking, no accounts, and anonymous participation."
			icon={PrivacyIcon}
		/>
	</div>
</section>

<style lang="scss">
	.benefits-section {
		background-color: $color-surface-alt;
		padding: $spacing-3xl 0;
	}

	.benefits-section__container {
		display: grid;
		grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
		gap: $spacing-xl;

		@media (max-width: $breakpoint-sm) {
			grid-template-columns: 1fr;
		}
	}
</style>
