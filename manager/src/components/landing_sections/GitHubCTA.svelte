<script lang="ts">
	/**
	 * @file GitHub Call to Action section for the landing page.
	 */
	import Button from '$components/elements/typography/Button.svelte';
	import { GITHUB_URL } from '$lib/config';
</script>

<section class="github-cta">
	<div class="page-container github-cta__container">
		<h2 class="github-cta__title">Thinking of Using EngaGenie at Your Institution?</h2>
		<p class="github-cta__description">
			EngaGenie is open-source and designed for flexibility. Explore the code, contribute, or deploy
			your own instance.
		</p>
		<Button variant="secondary" onclick={() => window.open(GITHUB_URL, '_blank')}>
			View on GitHub
		</Button>
	</div>
</section>

<style lang="scss">
	.github-cta {
		background-color: $color-surface;
		padding: $spacing-3xl 0;
		text-align: center;

		&__container {
			display: flex;
			flex-direction: column;
			align-items: center;
		}

		&__title {
			font-size: $font-size-3xl;
			margin-bottom: $spacing-sm;
		}

		&__description {
			font-size: $font-size-lg;
			color: $color-text-secondary;
			max-width: 650px;
			margin-bottom: $spacing-xl;
		}
	}
</style>
