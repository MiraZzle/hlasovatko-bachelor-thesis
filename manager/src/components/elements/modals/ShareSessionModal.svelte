<script lang="ts">
	/**
	 * @file Modal dialog component for sharing a session.
	 * Allows users to share a session via QR code or code input.
	 */
	import ModalDialog from '$components/elements/modals/ModalDialog.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import { onMount } from 'svelte';
	import QRCode from 'qrcode';

	let {
		open = $bindable(false),
		participateUrl = $bindable(''),
		manageUrl = $bindable(''),
		code = $bindable(''),
		onclose = () => {
			open = false;
		}
	}: {
		open?: boolean;
		participateUrl: string;
		manageUrl: string;
		code: string;
		onclose?: () => void;
	} = $props();

	let qrCanvas: HTMLCanvasElement;
	let copyButtonText = $state('Copy');

	// Ensure the URL is valid when the component mounts
	const participateUrlAsObject = $derived(() => {
		try {
			const urlObject = new URL(participateUrl);
			urlObject.searchParams.set('code', code);
			return urlObject.href;
		} catch (e) {
			console.error('Invalid base URL provided to ShareSessionModal:', participateUrl);
			return '';
		}
	});

	// Generate QR code when the modal opens or when the URL/code changes
	$effect(() => {
		if (open && qrCanvas && participateUrlAsObject) {
			QRCode.toCanvas(
				qrCanvas,
				participateUrlAsObject(),
				{
					width: 240,
					margin: 1,
					color: {
						dark: '#000000FF',
						light: '#FFFFFFFF'
					}
				},
				(error) => {
					if (error) console.error('QR Code generation failed:', error);
				}
			);
		}
	});

	const titleId = 'share-session-title';
	const descriptionId = 'share-session-desc';
</script>

<ModalDialog bind:open {onclose} width="sm" {titleId} {descriptionId}>
	<h2 id={titleId} class="share-modal__title">Share Session</h2>
	<p id={descriptionId} class="share-modal__description">
		Participants can scan the QR code or enter the code to join.
	</p>

	<div class="share-modal__content">
		<div class="share-modal__qr-container">
			<canvas bind:this={qrCanvas}></canvas>
		</div>

		<div class="share-modal__code-container">
			<span class="share-modal__code-label">Or join with code</span>
			<div class="share-modal__code-actions">
				<span class="share-modal__code">{code}</span>
			</div>
		</div>
	</div>

	<div class="share-modal__footer">
		<Button variant="primary" href={manageUrl} fullWidth>Manage</Button>
		<Button variant="outline" href={participateUrl} fullWidth>Participate</Button>
	</div>
</ModalDialog>

<style lang="scss">
	.share-modal {
		&__title {
			font-size: $font-size-xl;
			font-weight: $font-weight-semibold;
			text-align: center;
			margin-bottom: $spacing-xs;
		}

		&__description {
			font-size: $font-size-md;
			color: $color-text-secondary;
			text-align: center;
			margin-bottom: $spacing-xl;
		}

		&__content {
			display: flex;
			flex-direction: column;
			gap: $spacing-xl;
		}

		&__qr-container {
			display: flex;
			justify-content: center;
			align-items: center;
			background-color: $color-surface-alt;
			border-radius: $border-radius-lg;
			padding: $spacing-md;
			canvas {
				border-radius: $border-radius-md;
				max-width: 100%;
				height: auto;
			}
		}

		&__code-container {
			display: flex;
			flex-direction: column;
			align-items: center;
			gap: $spacing-sm;
		}

		&__code-label {
			font-size: $font-size-sm;
			color: $color-text-secondary;
		}

		&__code-actions {
			display: flex;
			align-items: center;
			gap: $spacing-md;
			width: 100%;
		}

		&__code {
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			letter-spacing: 2px;
			color: $color-text-primary;
			background-color: $color-surface-alt;
			padding: $spacing-sm $spacing-lg;
			border-radius: $border-radius-md;
			border: 1px solid $color-border-light;
			width: 100%;
			text-align: center;
		}

		&__footer {
			margin-top: $spacing-xl;
			padding-top: $spacing-lg;
			border-top: 1px solid $color-border-light;
			display: flex;
			flex-direction: row;
			gap: $spacing-md;
			justify-content: space-between;
		}
	}
</style>
