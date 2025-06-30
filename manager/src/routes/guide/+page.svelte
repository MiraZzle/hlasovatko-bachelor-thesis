<script lang="ts">
	/**
	 * @file Guide Page - basic info on usage
	 */
	import Navbar from '$components/landing_sections/Navbar.svelte';
	export let data;
</script>

<svelte:head>
	<title>EngaGenie | Guide</title>
</svelte:head>

<div class="page-wrapper">
	<Navbar />
	<main class="about-page">
		<h1 class="about-page__title">About EngaGenie</h1>
		<p class="about-page__text">
			EngaGenie is an open-source platform designed to facilitate interactive learning experiences
		</p>

		<section class="about-page__section">
			<h2 class="about-page__subtitle">Supported Activity Types</h2>
			<p class="about-page__text">The platform supports the following activity types:</p>
			<ul class="about-page__list">
				<li class="about-page__list-item">Multiple Choice</li>
				<li class="about-page__list-item">Poll</li>
				<li class="about-page__list-item">Scale Rating</li>
				<li class="about-page__list-item">Open-Ended</li>
			</ul>
		</section>

		<section class="about-page__section">
			<h2 class="about-page__subtitle">How to add an activity to the Bank</h2>
			<p class="about-page__text">
				The "Activity bank" is user personal collection of reusable activities. To add a new one:
			</p>
			<ol class="about-page__steps">
				<li class="about-page__step">
					Navigate to the <a href="/overview/activity-bank"> Activity Bank </a> after logging in.
				</li>
				<li class="about-page__step">
					Click the "Add Activity" button to open the creation modal.
				</li>
				<li class="about-page__step">
					Fill in the required fields: give your activity a title, select its type, and provide the
					specific JSON definition for it.
				</li>
				<li class="about-page__step">
					Click Save to add the activity to your bank for future use in templates.
				</li>
			</ol>
		</section>

		<section class="about-page__section">
			<h2 class="about-page__subtitle">Activity JSON Schemas</h2>
			<p class="about-page__text">
				Each activity type has a specific JSON schema that defines itss structure. The schemas are
				used to validate the activity definitions when creating new activities.
			</p>
			{#each data.activitySchemas as { type, schema }}
				<h3 class="about-page__sub-subtitle">{type}</h3>
				<div class="about-page__schema-container">
					<pre><code>{schema}</code></pre>
				</div>
			{/each}
		</section>
	</main>
</div>

<style lang="scss">
	.page-wrapper {
		background-color: $color-surface;
		min-height: 100vh;
	}

	.about-page {
		width: 100%;
		max-width: 900px;
		margin: 0 auto;
		padding: 2rem 2rem 4rem;

		&__title {
			font-size: 48px;
			font-weight: 700;
			margin-bottom: 1rem;
			color: $color-text-primary;
		}

		&__subtitle {
			font-size: 24px;
			font-weight: 600;
			margin-top: 2.5rem;
			margin-bottom: 1.5rem;
			color: $color-text-primary;
			border-bottom: 1px solid $color-border;
			padding-bottom: 0.5rem;
		}

		&__sub-subtitle {
			font-size: 20px;
			font-weight: 600;
			margin-top: 2rem;
			margin-bottom: 1rem;
			color: $color-text-secondary;
		}

		&__text {
			font-size: 1rem;
			line-height: 1.6;
			color: $color-text-secondary;
			margin-bottom: 1rem;
		}

		&__list {
			list-style: none;
			padding-left: 0;
			margin-bottom: 1rem;
		}

		&__list-item {
			position: relative;
			padding-left: 1.5rem;
			margin-bottom: 0.75rem;
			color: $color-text-secondary;
			font-size: 1rem;

			&::before {
				content: '✓';
				position: absolute;
				left: 0;
				color: $color-primary;
				font-weight: 700;
			}
		}

		&__steps {
			list-style: none;
			counter-reset: steps-counter;
			padding-left: 0;
			margin-top: 1.5rem;
		}

		&__step {
			counter-increment: steps-counter;
			margin-bottom: 1rem;
			display: flex;
			align-items: flex-start;
			font-size: 1rem;
			line-height: 1.6;
			color: $color-text-secondary;

			&::before {
				content: counter(steps-counter);
				margin-right: 1rem;
				font-size: 0.9rem;
				font-weight: 700;
				background-color: $color-primary-light;
				color: $color-primary;
				width: 28px;
				height: 28px;
				border-radius: 50%;
				display: inline-flex;
				align-items: center;
				justify-content: center;
				flex-shrink: 0;
			}

			a {
				color: $color-primary;
				text-decoration: none;
				font-weight: 600;
				&:hover {
					text-decoration: underline;
				}
			}
		}

		&__schema-container {
			background-color: #282c34;
			color: #abb2bf;
			font-family: monospace;
			font-size: 0.9rem;
			line-height: 1.5;
			padding: 1.5rem;
			border-radius: 12px;
			overflow-x: auto;
			border: 1px solid #3a4048;
			box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);

			pre {
				margin: 0;
				white-space: pre-wrap;
				word-wrap: break-word;
			}

			code {
				color: #c5c8c6;
			}
		}
	}
</style>
