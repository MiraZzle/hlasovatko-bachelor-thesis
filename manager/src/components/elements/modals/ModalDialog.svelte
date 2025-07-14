<script lang="ts">
	/**
	 * @file Modal dialog component for displaying a modal dialog.
	 * Provides a customizable dialog with options for width, close behavior, and content.
	 */
	import { onMount, onDestroy, tick } from 'svelte';
	import type { Snippet } from 'svelte';
	import { browser } from '$app/environment';

	type ModalWidth = 'auto' | 'sm' | 'md' | 'lg';

	let {
		open = $bindable(false),
		onclose = () => {},
		width = 'auto',
		closeOnEscape = true,
		closeOnOutsideClick = true,
		titleId = null,
		descriptionId = null,
		children
	}: {
		open?: boolean;
		onclose?: () => void;
		width?: ModalWidth;
		closeOnEscape?: boolean;
		closeOnOutsideClick?: boolean;
		titleId?: string | null;
		descriptionId?: string | null;
		children: Snippet;
	} = $props();

	let dialogElement: HTMLDivElement | null = $state(null);

	function requestClose(): void {
		if (onclose) {
			onclose();
		} else {
			open = false;
		}
	}

	function handleGlobalKeydown(event: KeyboardEvent): void {
		if (open && closeOnEscape && event.key === 'Escape') {
			event.preventDefault();
			requestClose();
		}
	}

	// Handle clicks on the overlay to close the dialog
	function handleOverlayClick(event: MouseEvent): void {
		if (closeOnOutsideClick && event.target === event.currentTarget) {
			requestClose();
		}
	}

	function handleOverlayKeydown(event: KeyboardEvent): void {
		if (closeOnOutsideClick && event.target === event.currentTarget) {
			if (event.key === 'Enter' || event.key === ' ') {
				event.preventDefault();
				requestClose();
			}
		}
	}

	// Add global keydown listener for Escape key when the dialog is open
	onMount(() => {
		if (browser && closeOnEscape) {
			window.addEventListener('keydown', handleGlobalKeydown);
		}
		return () => {
			if (browser && closeOnEscape) {
				window.removeEventListener('keydown', handleGlobalKeydown);
			}
		};
	});

	// Focus the dialog when it opens
	$effect(() => {
		if (open && dialogElement) {
			tick().then(() => {
				dialogElement?.focus();
			});
		}
	});
</script>

{#if open}
	<div
		class="modal-dialog__overlay"
		onclick={handleOverlayClick}
		onkeydown={handleOverlayKeydown}
		role="button"
		tabindex="-1"
	>
		<div
			class="modal-dialog modal-dialog--width-{width}"
			role="dialog"
			aria-modal="true"
			aria-labelledby={titleId}
			aria-describedby={descriptionId}
			bind:this={dialogElement}
			tabindex="-1"
		>
			<button
				class="modal-dialog__close-button"
				onclick={requestClose}
				aria-label="Close dialog"
				type="button"
			>
				<span aria-hidden="true">&times;</span>
			</button>

			<div class="modal-dialog__content">
				{@render children()}
			</div>
		</div>
	</div>
{/if}

<style lang="scss">
	.modal-dialog {
		&__overlay {
			position: fixed;
			inset: 0;
			z-index: $modal-z-index;
			background-color: rgba(0, 0, 0, 0.35);
			backdrop-filter: blur(3px);
			display: flex;
			align-items: center;
			justify-content: center;
			padding: $spacing-md;
			overflow-y: auto;
			outline: none;
		}

		& {
			background-color: $color-surface;
			border-radius: $border-radius-lg;
			box-shadow: $box-shadow-xl;
			padding: $spacing-xl;
			position: relative;
			max-height: calc(95vh - 2 * $spacing-md);
			max-width: 95vw;
			display: flex;
			flex-direction: column;
			overflow: hidden;
			margin: auto;
			outline: none;

			&--width-auto {
				width: auto;
			}
			&--width-sm {
				width: 480px;
			}
			&--width-md {
				width: 600px;
			}
			&--width-lg {
				width: 800px;
			}
		}

		&__close-button {
			position: absolute;
			top: $spacing-md;
			right: $spacing-md;
			background: none;
			border: none;
			padding: $spacing-xs;
			margin: -$spacing-xs;
			cursor: pointer;
			color: $color-text-secondary;
			border-radius: $border-radius-circle;
			display: inline-flex;
			align-items: center;
			justify-content: center;
			line-height: 0;
			z-index: 1;
			height: $font-size-3xl;
			width: $font-size-3xl;
			font-size: $font-size-4xl;

			&:hover {
				color: $color-text-primary;
				background-color: $color-surface-alt;
			}
			&:focus-visible {
				outline: 2px solid $color-primary-light;
				outline-offset: 1px;
				background-color: $color-surface-alt;
			}
		}

		&__content {
			margin-top: $spacing-lg;
			overflow-y: auto;
			overflow-x: hidden;
			padding-right: $spacing-xs;
			margin-right: -$spacing-xs;

			:global(h1),
			:global(h2),
			:global(h3) {
				margin-top: 0;
			}
		}
	}
</style>
