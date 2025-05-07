<script lang="ts">
	// Import the reusable component
	import JoinSession from '$components/elements/typography/utils/JoinSession.svelte'; // Verify path

	// No specific state or handlers needed here anymore,
	// as they are encapsulated within JoinSession.svelte
</script>

<svelte:head>
	<title>Join Session - EngaGenie</title>
	<meta name="description" content="Join an active EngaGenie session using a code." />
</svelte:head>

<div class="join-session-page">
	<div class="join-session-page__logo">EngaGenie</div>

	<h1 class="join-session-page__title">Join a Session</h1>
	<p class="join-session-page__instructions">
		Enter the 6-character code provided by your instructor.
	</p>

	<div class="join-session-page__component-wrapper">
		<JoinSession />
	</div>

	<p class="join-session-page__alternative-link">
		Are you an instructor? <a href="/login">Log in here</a>.
	</p>
</div>

<style lang="scss">
	@import '../styles/variables.scss'; // Adjust path if needed

	// Block: join-session-page
	.join-session-page {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center; // Center content vertically
		min-height: 100vh; // Use full viewport height
		padding: $spacing-xl;
		background-color: $color-background; // Use standard background, removed gradient

		// Element: Logo
		&__logo {
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			color: $color-primary-dark;
			margin-bottom: $spacing-2xl;
			letter-spacing: -0.5px;
		}

		// Element: Title
		&__title {
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			margin-top: 0;
			margin-bottom: $spacing-sm;
			color: $color-text-primary;
			text-align: center;
		}

		// Element: Instructions
		&__instructions {
			font-size: $font-size-lg;
			color: $color-text-secondary;
			margin-bottom: $spacing-xl;
			max-width: 450px;
			text-align: center;
		}

		// Element: Wrapper for the component to control its width
		&__component-wrapper {
			width: 100%;
			max-width: 550px; // Max width for the JoinSession component area
		}

		// --- REMOVED: Styles for __form, __error ---
		// --- REMOVED: :global styles for input/button ---

		// Element: Alternative Link
		&__alternative-link {
			margin-top: $spacing-2xl;
			font-size: $font-size-sm;
			color: $color-text-secondary;
			a {
				color: $color-primary;
				font-weight: $font-weight-medium;
				text-decoration: underline;
				&:hover {
					color: $color-primary-dark;
				}
			}
		}
	}
</style>
