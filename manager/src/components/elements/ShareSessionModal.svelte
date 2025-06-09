<script lang="ts">
	import ModalDialog from '$components/elements/ModalDialog.svelte';
	import Button from '$components/elements/typography/Button.svelte';
	import { onMount } from 'svelte';
	import QRCode from 'qrcode';

	type Props = {
		open?: boolean;
		url: string;
		code: string;
		onclose?: () => void;
	};

	let {
		open = $bindable(false),
		url = $bindable(''),
		code = $bindable(''),
		onclose = () => {
			open = false;
		}
	}: Props = $props();

	let qrCanvas: HTMLCanvasElement;
	let copyButtonText = $state('Copy');

	const joinUrlWithCode = $derived(() => {
		try {
			const urlObject = new URL(url);
			urlObject.searchParams.set('code', code);
			return urlObject.href;
		} catch (e) {
			console.error('Invalid base URL provided to ShareSessionModal:', url);
			return '';
		}
	});

	$effect(() => {
		if (open && qrCanvas && joinUrlWithCode) {
			QRCode.toCanvas(
				qrCanvas,
				joinUrlWithCode(),
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

	$effect(() => {
		if (open) {
			copyButtonText = 'Copy';
		}
	});

	async function copyCodeToClipboard() {
		if (!navigator.clipboard) {
			alert('Clipboard access is not available in your browser.');
			return;
		}
		try {
			await navigator.clipboard.writeText(code);
			copyButtonText = 'Copied!';
			setTimeout(() => {
				if (copyButtonText === 'Copied!') {
					copyButtonText = 'Copy';
				}
			}, 2500);
		} catch (err) {
			console.error('Failed to copy code to clipboard:', err);
			alert('Could not copy code. Please copy it manually.');
		}
	}

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
				<Button variant="outline" onclick={copyCodeToClipboard}>
					{copyButtonText}
				</Button>
			</div>
		</div>
	</div>

	<div class="share-modal__footer">
		<Button variant="primary" onclick={onclose}>Done</Button>
	</div>
</ModalDialog>

<style lang="scss">
	@import '../../styles/variables.scss';

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
		}

		&__footer {
			margin-top: $spacing-xl;
			border-top: 1px solid $color-border-light;
			padding-top: $spacing-lg;
		}
	}
</style>
