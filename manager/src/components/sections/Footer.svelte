<script lang="ts">
	const currentYear = new Date().getFullYear();
</script>

<footer class="app-footer">
	<div class="page-container app-footer__container">
		<div class="app-footer__column app-footer__column--brand">
			<a href="/" class="app-footer__logo" aria-label="Homepage">EngaGenie</a>
			<p class="app-footer__copy">&copy; {currentYear} EngaGenie. All rights reserved.</p>
		</div>

		<div class="app-footer__column">
			<h4 class="app-footer__heading">Use Cases</h4>
			<ul class="app-footer__list">
				<li><a href="#">Live Polling</a></li>
				<li><a href="#">Hybrid Learning</a></li>
				<li><a href="#">Student Feedback</a></li>
				<li><a href="#">Remote Workshops</a></li>
				<li><a href="#">Conference Q&A</a></li>
				<li><a href="#">All-hands Meetings</a></li>
			</ul>
		</div>

		<div class="app-footer__column">
			<h4 class="app-footer__heading">Platform</h4>
			<ul class="app-footer__list">
				<li><a href="#">How It Works</a></li>
				<li><a href="#">Success Stories</a></li>
				<li><a href="#">API for Developers</a></li>
				<li><a href="#">Help Center</a></li>
			</ul>
		</div>

		<div class="app-footer__column">
			<h4 class="app-footer__heading">Resources</h4>
			<ul class="app-footer__list">
				<li><a href="#">Blog</a></li>
				<li><a href="#">Best Practices for University Teaching</a></li>
				<li><a href="#">Accessibility Features</a></li>
				<li><a href="#">Terms of Use</a></li>
				<li><a href="#">Privacy Policy</a></li>
				<li><a href="#">Contact Us</a></li>
			</ul>
		</div>
	</div>
</footer>

<style lang="scss">
	@import '../../styles/variables.scss';

	.app-footer {
		background-color: $color-surface; // Can be darker if preferred
		border-top: $border-width-thin solid $color-border-light;
		padding: $spacing-2xl 0 $spacing-xl; // More padding top, less bottom
		font-size: $font-size-sm;
		color: $color-text-secondary;

		&__container {
			display: grid;
			grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
			gap: $spacing-xl $spacing-lg; // Row and column gap

			@media (min-width: $breakpoint-md) {
				grid-template-columns: 2fr 1fr 1fr 1fr; // Give brand column more space
			}
		}

		&__column {
			&--brand {
				// Special styles for the first column if needed
				@media (max-width: $breakpoint-md) {
					grid-column: 1 / -1; // Make brand span full width on small screens
					text-align: center;
					margin-bottom: $spacing-lg;
				}
			}
		}

		&__logo {
			display: inline-block;
			font-size: $font-size-lg;
			font-weight: $font-weight-bold;
			color: $color-text-primary;
			text-decoration: none;
			margin-bottom: $spacing-sm;

			&:hover {
				text-decoration: none;
				color: $color-primary;
			}
		}

		&__copy {
			margin-top: $spacing-xs;
			font-size: $font-size-xs;
			color: $color-text-secondary;
			margin-bottom: 0;
		}

		&__heading {
			font-size: $font-size-md;
			font-weight: $font-weight-semibold;
			color: $color-text-primary;
			margin-bottom: $spacing-md;
		}

		&__list {
			list-style: none;
			padding: 0;
			margin: 0;

			li {
				margin-bottom: $spacing-sm;
			}

			a {
				color: $color-text-secondary;
				text-decoration: none;

				&:hover {
					color: $color-primary;
					text-decoration: underline;
				}
			}
		}
	}
</style>
