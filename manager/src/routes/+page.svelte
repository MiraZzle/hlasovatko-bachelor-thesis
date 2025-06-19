<script lang="ts">
	import Hero from '$components/landing_sections/Hero.svelte';
	import JoinSession from '$components/landing_sections/JoinSession.svelte';
	import Features from '$components/landing_sections/Features.svelte';
	import Benefits from '$components/landing_sections/Benefits.svelte';
	import Footer from '$components/landing_sections/Footer.svelte';
	import Navbar from '$components/landing_sections/Navbar.svelte';
	import GitHubCta from '$components/landing_sections/GitHubCTA.svelte';
</script>

<svelte:head>
	<title>EngaGenie | Home</title>
</svelte:head>

<Navbar />
<main>
	<Hero />
	<section class="join-session__container">
		<JoinSession />
	</section>
	<Features />
	<Benefits />
	<GitHubCta />
</main>
<Footer />

<style lang="scss">
	.join-session__container {
		background-color: $color-surface-alt;
		padding: $spacing-2xl $spacing-lg;
	}
</style>
