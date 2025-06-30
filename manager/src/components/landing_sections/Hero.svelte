<script lang="ts">
	/**
	 * @file Hero section for the landing page.
	 */
	import Button from '$components/elements/typography/Button.svelte';
</script>

<section class="hero-section">
	<div class="page-container hero-section__container">
		<h1 class="hero-section__title">
			Transform Your Lectures into <br /> Interactive Sessions
		</h1>
		<p class="hero-section__subtitle">
			A university-focused interactive platform for quizzes, polls, live Q&A sessions.
		</p>
		<div class="hero-section__actions">
			<Button variant="secondary" href="/guide">Get Started</Button>
			<Button variant="primary" href="/login">Create Session</Button>
		</div>
	</div>
</section>

<style lang="scss">
	.hero-section {
		background-color: $color-surface;
		padding: $spacing-3xl 0;
		text-align: center;

		&__container {
			display: flex;
			flex-direction: column;
			align-items: center;
		}

		&__title {
			font-size: $font-size-5xl;
			font-weight: $font-weight-bold;
			color: $color-text-primary;
			margin-bottom: $spacing-md;
			max-width: 800px;
			line-height: 1.1;

			@media (max-width: $breakpoint-sm) {
				font-size: $font-size-4xl;
			}
		}

		&__subtitle {
			font-size: $font-size-xl;
			color: $color-text-secondary;
			margin-bottom: $spacing-xl;
			max-width: 600px;

			@media (max-width: $breakpoint-sm) {
				font-size: $font-size-lg;
			}
		}

		&__actions {
			display: flex;
			gap: $spacing-md;
		}
	}
</style>
