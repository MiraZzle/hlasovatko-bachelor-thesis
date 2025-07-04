<script lang="ts">
	import { onMount } from 'svelte';
	import QRCode from 'qrcode';
	import { getParticipateSessionLink } from '$lib/router/external_routes';
	import { page } from '$app/stores';

	const sessionId = $page.url.searchParams.get('id') || '';
	const joinCode = $page.url.searchParams.get('code');
	const participateUrl = getParticipateSessionLink(sessionId);

	let qrCanvas: HTMLCanvasElement;

	/**
	 * Generates the QR code and renders it to the canvas.
	 */
	onMount(() => {
		if (qrCanvas && participateUrl) {
			QRCode.toCanvas(
				qrCanvas,
				participateUrl,
				{
					width: 320,
					margin: 2,
					color: {
						dark: '#000000FF',
						light: '#FFFFFFFF'
					}
				},
				(error) => {
					if (error) {
						console.error('QR Code generation failed:', error);
					}
				}
			);
		}
	});
</script>

<div class="share-page">
	{#if joinCode}
		<div class="share-page__container">
			<h1 class="share-page__title">Join Session</h1>
			<p class="share-page__description">Scan the QR code or use the code below to join.</p>

			<div class="share-page__content">
				<div class="share-page__qr-container">
					<canvas bind:this={qrCanvas}></canvas>
				</div>

				<div class="share-page__code-container">
					<p class="share-page__join-code">
						{joinCode}
					</p>
				</div>
			</div>
		</div>
	{:else}
		<div class="share-page__container">
			<h1 class="share-page__title">Error</h1>
			<p class="share-page__description">
				A join code was not found in the URL. Please make sure the link is correct.
			</p>
		</div>
	{/if}
</div>

<style lang="scss">
	.share-page {
		display: flex;
		justify-content: center;
		align-items: center;
		min-height: 100vh;
		background-color: $color-primary-light;
		padding: $spacing-xl;
		box-sizing: border-box;

		&__container {
			background: $color-surface;
			padding: $spacing-3xl;
			max-width: 1200px;
			width: 100%;
			text-align: center;
			border-radius: $border-radius-lg;
		}

		&__title {
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			margin-bottom: $spacing-md;
			color: $color-text-primary;
		}

		&__description {
			font-size: $font-size-lg;
			color: $color-text-secondary;
			margin-bottom: $spacing-2xl;
		}

		&__content {
			display: flex;
			flex-direction: row;
			gap: $spacing-2xl;
			align-items: center;
			justify-content: center;
			flex-wrap: wrap;
		}

		&__qr-container {
			display: inline-block;
			background-color: white;
			padding: $spacing-md;
			border-radius: $border-radius-lg;
			border: 1px solid $color-border-light;

			canvas {
				display: block;
				max-width: 100%;
				height: auto;
				border-radius: $border-radius-md;
			}
		}

		&__code-container {
			display: flex;
			align-items: center;
			justify-content: center;
		}

		&__join-code {
			display: inline-block;
			font-size: $font-size-5xl;
			font-weight: $font-weight-bold;
			letter-spacing: 6px;
			background-color: $color-background;
			padding: $spacing-lg $spacing-2xl;
			border-radius: $border-radius-lg;
			border: 1px solid $color-border-light;
			text-decoration: none;
		}
	}
</style>
