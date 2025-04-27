<script lang="ts">
	import Button from '$components/elements/typography/Button.svelte';

	function handleSignUp(): void {
		console.log('Sign Up clicked');
		// Dummy action: Maybe open a modal or navigate
	}

	function handleLogin(): void {
		console.log('Log In clicked');
		// Dummy action: Maybe open a modal or navigate
	}
</script>

<header class="app-header">
	<div class="app-header__inner page-container">
		<a href="/" class="app-header__logo" aria-label="Homepage"> EngaGenie </a>

		<nav class="app-header__nav">
			<ul>
				<li><a href="/activities">Activities</a></li>
				<li><a href="/discovery">Discovery</a></li>
				<li><a href="/developers">For Developers</a></li>
				<li><a href="/resources">Resources</a></li>
			</ul>
		</nav>

		<div class="app-header__actions">
			<Button variant="outline" onclick={handleSignUp}>Sign Up</Button>
			<Button variant="primary" onclick={handleLogin}>Log In</Button>
		</div>

		<button class="app-header__mobile-toggle" aria-label="Toggle menu">
			<span>MENU</span>
		</button>
	</div>
</header>

<style lang="scss">
	@import '../../styles/variables.scss';

	.app-header {
		padding: $spacing-md 0;
		background-color: $color-surface; // White background
		border-bottom: $border-width-thin solid $color-border-light;
		position: sticky; // Optional: make header sticky
		top: 0;
		z-index: 10; // Ensure it's above other content

		&__inner {
			display: flex;
			align-items: center;
			justify-content: space-between;
			gap: $spacing-lg;
		}

		&__logo {
			font-size: $font-size-xl;
			font-weight: $font-weight-bold;
			color: $color-text-primary;
			text-decoration: none;
			flex-shrink: 0; // Prevent logo from shrinking

			&:hover {
				text-decoration: none; // Remove underline on hover
				color: $color-primary; // Example hover effect
			}
		}

		&__nav {
			// Hide nav on smaller screens, show mobile toggle instead
			display: none;

			@media (min-width: $breakpoint-lg) {
				display: block; // Show nav on larger screens
				flex-grow: 1; // Allow nav to take space if needed
				margin: 0 $spacing-xl; // Space around nav
			}

			ul {
				display: flex;
				list-style: none;
				padding: 0;
				margin: 0;
				gap: $spacing-xl; // Space between nav items
			}

			li {
				margin: 0;
			}

			a {
				font-size: $font-size-md;
				font-weight: $font-weight-medium;
				color: $color-text-secondary;
				text-decoration: none;
				transition: color $transition-duration-fast;

				&:hover {
					color: $color-primary;
					text-decoration: none;
				}
			}
		}

		&__actions {
			display: none; // Hide actions on smaller screens initially

			@media (min-width: $breakpoint-lg) {
				display: flex; // Show actions on larger screens
				align-items: center;
				gap: $spacing-sm; // Space between buttons
				flex-shrink: 0;
			}
		}

		&__mobile-toggle {
			display: block; // Show mobile toggle on smaller screens
			background: none;
			border: none;
			cursor: pointer;
			padding: $spacing-xs;
			// Add styles for burger icon (SVG or CSS)
			color: $color-primary;
			font-weight: $font-weight-medium;

			@media (min-width: $breakpoint-lg) {
				display: none; // Hide on larger screens
			}
		}
	}
</style>
