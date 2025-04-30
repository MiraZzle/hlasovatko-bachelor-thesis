<script lang="ts">
	import Hero from '$components/sections/Hero.svelte';
	import JoinSession from '$components/sections/JoinSession.svelte';
	import Features from '$components/sections/Features.svelte';
	import Benefits from '$components/sections/Benefits.svelte';
	import EmailSignup from '$components/sections/EmailSignup.svelte';
	import Footer from '$components/sections/Footer.svelte';
	import Navbar from '$components/sections/Navbar.svelte';

	// Potential page-specific metadata (using $page store from SvelteKit)
	// import { page } from '$app/stores';
	// $: title = 'EngaGenie - Interactive Lectures'; // Example title
</script>

<Navbar />

<main>
	<Hero />
	<section class="join-session__container">
		<JoinSession />
	</section>
	<Features />
	<Benefits />
	<EmailSignup />
</main>

<Footer />

<style lang="scss">
	@import '../styles/variables.scss';

	.join-session__container {
		background-color: $color-surface-alt; // Slightly different background
		padding: $spacing-2xl 0;
	}
</style>
