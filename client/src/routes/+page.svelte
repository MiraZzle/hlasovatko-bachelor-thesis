<script lang="ts">
	import JoinSession from '$components/elements/typography/utils/JoinSession.svelte';
	import { MANAGER_FE_URL } from '$lib/config';
</script>

<svelte:head>
	<title>EngaGenie | Join a Session</title>
	<meta name="description" content="Join an active EngaGenie session using a code." />
</svelte:head>

<div class="join-session-page">
	<div class="join-session-page__logo">
		<a href={MANAGER_FE_URL} class="join-session-page__logo-link" aria-label="Homepage">
			EngaGenie
		</a>
	</div>

	<h1 class="join-session-page__title">Join a Session</h1>
	<p class="join-session-page__instructions">Enter the join code provided by your teacher.</p>

	<div class="join-session-page__component-wrapper">
		<JoinSession />
	</div>

	<p class="join-session-page__alternative-link">
		Are you an teacher? <a href="{MANAGER_FE_URL}/login">Sign in here</a>.
	</p>
</div>

<style lang="scss">
	.join-session-page {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		min-height: 100vh;
		padding: $spacing-xl;
		background-color: $color-background;

		&__logo {
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			color: $color-primary-dark;
			margin-bottom: $spacing-2xl;
			letter-spacing: -0.5px;

			&-link {
				text-decoration: none;

				&:hover {
					color: $color-primary-dark;
				}
			}
		}

		&__title {
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			margin-top: 0;
			margin-bottom: $spacing-sm;
			color: $color-text-primary;
			text-align: center;
		}

		&__instructions {
			font-size: $font-size-lg;
			color: $color-text-secondary;
			margin-bottom: $spacing-xl;
			max-width: 450px;
			text-align: center;
		}

		&__component-wrapper {
			width: 100%;
			max-width: 550px;
		}

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
