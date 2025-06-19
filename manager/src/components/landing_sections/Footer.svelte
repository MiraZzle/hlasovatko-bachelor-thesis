<script lang="ts">
	/**
	 * @file Footer component for the landing page.
	 */
	import { GITHUB_URL } from '$lib/config';

	const currentYear = new Date().getFullYear();
</script>

<footer class="app-footer">
	<div class="app-footer__container page-container">
		<div class="app-footer__brand">
			<a href="/" class="app-footer__logo" aria-label="Homepage">EngaGenie</a>
			<p class="app-footer__copy">&copy; {currentYear} EngaGenie. All rights reserved.</p>
		</div>

		<div class="app-footer__links">
			<ul class="app-footer__list">
				<li class="app-footer__item">
					<a href={GITHUB_URL} class="app-footer__link" target="_blank" rel="noopener">GitHub</a>
				</li>
				<li class="app-footer__item">
					<a href="mailto:you@example.com" class="app-footer__link">Contact</a>
				</li>
				<li class="app-footer__item">
					<a href="/terms" class="app-footer__link">Privacy Policy</a>
				</li>
			</ul>
		</div>
	</div>
</footer>

<style lang="scss">
	.app-footer {
		background-color: $color-surface;
		border-top: $border-width-thin solid $color-border-light;
		padding: $spacing-xl 0;
		font-size: $font-size-sm;
		color: $color-text-secondary;

		&__container {
			display: flex;
			justify-content: space-between;
			flex-wrap: wrap;
			gap: $spacing-lg;
			align-items: center;
		}

		&__brand {
			flex: 1 1 200px;
		}

		&__logo {
			display: inline-block;
			font-size: $font-size-lg;
			font-weight: $font-weight-bold;
			color: $color-text-primary;
			text-decoration: none;

			&:hover {
				color: $color-primary;
			}
		}

		&__copy {
			margin-top: $spacing-xs;
			font-size: $font-size-xs;
			color: $color-text-secondary;
		}

		&__links {
			flex: 1 1 200px;
			text-align: right;

			@media (max-width: $breakpoint-sm) {
				text-align: left;
			}
		}

		&__list {
			list-style: none;
			margin: 0;
			padding: 0;
		}

		&__item {
			margin-bottom: $spacing-xs;
		}

		&__link {
			color: $color-text-secondary;
			text-decoration: none;
			transition: color $transition-duration-fast;

			&:hover {
				color: $color-primary;
				text-decoration: underline;
			}
		}
	}
</style>
