<script lang="ts">
	import Button from '$components/elements/typography/Button.svelte';

	let isMenuOpen = $state(false);

	function toggleMenu(): void {
		isMenuOpen = !isMenuOpen;
	}

	function handleNavLinkClick(): void {
		if (isMenuOpen) {
			isMenuOpen = false;
		}
	}

	function handleLogin(): void {
		console.log('Log In clicked');
		window.location.href = '/login';
	}
</script>

<header class="navbar" class:navbar--menu-open={isMenuOpen}>
	<div class="navbar__inner page-container">
		<a href="/" class="navbar__logo" aria-label="Homepage" onclick={handleNavLinkClick}>
			EngaGenie
		</a>

		<button
			class="navbar__toggle"
			onclick={toggleMenu}
			aria-label="Toggle menu"
			aria-expanded={isMenuOpen}
		>
			{#if isMenuOpen}
				❌
			{:else}
				☰
			{/if}
		</button>

		<div class="navbar__menu">
			<nav class="navbar__nav">
				<ul class="navbar__list">
					<li class="navbar__item">
						<a href="#features" class="navbar__link" onclick={handleNavLinkClick}>Activities</a>
					</li>
					<li class="navbar__item">
						<a href="/discovery" class="navbar__link" onclick={handleNavLinkClick}>Usecases</a>
					</li>
					<li class="navbar__item">
						<a
							href="https://github.com/MiraZzle/hlasovatko-bachelor-thesis"
							target="_blank"
							rel="noopener noreferrer"
							class="navbar__link"
							onclick={handleNavLinkClick}>For Developers</a
						>
					</li>
				</ul>
			</nav>

			<div class="navbar__actions">
				<Button variant="primary" onclick={handleLogin}>Log In</Button>
			</div>
		</div>
	</div>
</header>

<style lang="scss">
	.navbar {
		padding: $spacing-md 0;
		background-color: $color-surface;
		border-bottom: $border-width-thin solid $color-border-light;
		position: sticky;
		top: 0;
		z-index: 10;

		&__inner {
			display: flex;
			align-items: center;
			justify-content: space-between;
			gap: $spacing-xl;
		}

		&__logo {
			font-size: $font-size-xl;
			font-weight: $font-weight-bold;
			color: $color-text-primary;
			text-decoration: none;
			flex-shrink: 0;
			z-index: 11;

			&:hover {
				color: $color-primary;
			}
		}

		&__toggle {
			display: block;
			background: none;
			border: none;
			cursor: pointer;
			padding: $spacing-xs;
			color: $color-primary;
			font-weight: $font-weight-medium;
			font-size: $font-size-md;
			z-index: 11;

			@media (min-width: $breakpoint-lg) {
				display: none;
			}
		}

		&__menu {
			position: absolute;
			top: 100%;
			left: 0;
			right: 0;
			background-color: $color-surface;
			border-bottom: 1px solid $color-border-light;
			padding: $spacing-xl;
			display: flex;
			flex-direction: column;
			align-items: center;
			gap: $spacing-xl;

			transform: translateY(-100%);
			opacity: 0;
			pointer-events: none;
			transition:
				transform 0.3s ease-in-out,
				opacity 0.3s ease-in-out;

			@media (min-width: $breakpoint-lg) {
				position: static;
				flex-direction: row;
				align-items: center;
				justify-content: space-between;
				flex-grow: 1;
				padding: 0;
				border: none;
				background-color: transparent;
				transform: none;
				opacity: 1;
				pointer-events: auto;
			}
		}

		&--menu-open {
			.navbar__menu {
				transform: translateY(0);
				opacity: 1;
				pointer-events: auto;
			}
		}

		&__nav {
			width: 100%;

			@media (min-width: $breakpoint-lg) {
				width: auto;
			}
		}

		&__list {
			display: flex;
			flex-direction: column;
			align-items: center;
			list-style: none;
			padding: 0;
			margin: 0;
			gap: $spacing-lg;

			@media (min-width: $breakpoint-lg) {
				flex-direction: row;
				gap: $spacing-xl;
			}
		}

		&__link {
			font-size: $font-size-lg;
			font-weight: $font-weight-medium;
			color: $color-text-secondary;
			text-decoration: none;
			transition: color $transition-duration-fast;

			&:hover {
				color: $color-primary;
			}

			@media (min-width: $breakpoint-lg) {
				font-size: $font-size-md;
			}
		}

		&__actions {
			display: flex;
			flex-direction: column;
			align-items: stretch;
			width: 100%;
			max-width: 250px;
			gap: $spacing-sm;

			@media (min-width: $breakpoint-lg) {
				flex-direction: row;
				align-items: center;
				width: auto;
				max-width: none;
			}
		}
	}
</style>
