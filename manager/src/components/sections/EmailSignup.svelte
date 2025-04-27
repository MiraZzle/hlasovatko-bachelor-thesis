<script lang="ts">
	import Button from '$components/elements/typography/Button.svelte';
	import Input from '$components/elements/typography/Input.svelte';

	let email = $state('');

	function handleEmailSubmit(event: SubmitEvent): void {
		event.preventDefault();
		if (!email.trim() || !email.includes('@')) {
			// Basic validation
			console.warn('Invalid email provided');
			// Add user feedback
			return;
		}
		console.log('Submitting email:', email);
		// Dummy action: Send email to backend / mailing list service
		alert(`Thanks for signing up with ${email}!`); // Placeholder feedback
		email = ''; // Clear input
	}
</script>

<section class="email-signup">
	<div class="page-container email-signup__container">
		<h2 class="email-signup__title">Help Us Make Hlasovátko Even Better</h2>
		<p class="email-signup__description">
			We're building this tool for teachers like you. Leave your email and we'll invite you to share
			feedback that shapes the future of Hlasovátko.
		</p>
		<form class="email-signup__form" onsubmit={handleEmailSubmit}>
			<Input
				type="email"
				name="email"
				placeholder="you@example.com"
				ariaLabel="Enter your email address"
				bind:value={email}
				required
			/>
			<Button type="submit" variant="secondary">I Want to Help</Button>
		</form>
	</div>
</section>

<style lang="scss">
	@import '../../styles/variables.scss';

	.email-signup {
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

		&__form {
			display: flex;
			gap: $spacing-sm;
			width: 100%;
			max-width: 500px; // Limit form width
			align-items: stretch;

			:global(.input-wrapper) {
				flex-grow: 1;
			}

			:global(.button) {
				flex-shrink: 0;
			}
		}
	}
</style>
