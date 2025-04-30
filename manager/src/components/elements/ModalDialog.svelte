<script lang="ts">
	import { onMount, onDestroy, tick } from 'svelte';
	import type { Snippet } from 'svelte';
	import { browser } from '$app/environment';

	type ModalWidth = 'auto' | 'sm' | 'md' | 'lg';

	type ModalProps = {
		open?: boolean;
		onclose?: () => void;
		width?: ModalWidth;
		closeOnEscape?: boolean;
		closeOnOutsideClick?: boolean;
		titleId?: string | null;
		descriptionId?: string | null;
		children: Snippet;
	};

	let {
		open = $bindable(false),
		onclose = () => {},
		width = 'auto',
		closeOnEscape = true,
		closeOnOutsideClick = true,
		titleId = null,
		descriptionId = null,
		children // Default slot content
	}: ModalProps = $props(); // Use the defined type with $props()

	// --- Internal Logic ---
	let dialogElement: HTMLDivElement | null = null; // Ref for focus management

	// Function to request closing the modal
	function requestClose(): void {
		// Don't directly set open = false here, let the parent decide via the binding
		// Only call the onclose callback if provided.
		if (onclose) {
			onclose();
		} else {
			// Fallback if no onclose is provided but binding is used:
			// Assume the intention is to close via binding.
			// This makes it work similarly to the Svelte 4 example's dispatch.
			open = false;
		}
	}

	// Keyboard event handler (Escape key)
	function handleGlobalKeydown(event: KeyboardEvent): void {
		if (open && closeOnEscape && event.key === 'Escape') {
			event.preventDefault(); // Prevent other Escape behavior
			requestClose();
		}
	}

	// Click handler for the overlay (outside click)
	function handleOverlayClick(event: MouseEvent): void {
		// Ensure the click is directly on the overlay, not on the dialog content
		if (closeOnOutsideClick && event.target === event.currentTarget) {
			requestClose();
		}
	}

	// Keydown on overlay (for accessibility - allow closing overlay via Enter/Space)
	function handleOverlayKeydown(event: KeyboardEvent): void {
		if (closeOnOutsideClick && event.target === event.currentTarget) {
			if (event.key === 'Enter' || event.key === ' ') {
				event.preventDefault();
				requestClose();
			}
		}
	}

	// --- Lifecycle for Global Listener ---
	onMount(() => {
		// This code ONLY runs on the client AFTER mount
		if (browser && closeOnEscape) {
			// Guarded access
			window.addEventListener('keydown', handleGlobalKeydown);
		}
		return () => {
			// This cleanup code ONLY runs on the client BEFORE unmount
			if (browser && closeOnEscape) {
				// Guarded access
				window.removeEventListener('keydown', handleGlobalKeydown);
			}
		};
	});

	// --- Focus Management (Basic) ---
	// When the modal opens, try to focus the dialog element itself.
	// Proper focus trapping is more complex and might involve libraries or more code.
	$effect(() => {
		if (open && dialogElement) {
			// Wait for DOM update before focusing
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
			onkeydown={handleGlobalKeydown}
		>
			<button
				class="modal-dialog__close-button"
				onclick={requestClose}
				aria-label="Close dialog"
				type="button"
			>
				<span aria-hidden="true">❌</span>
			</button>

			<div class="modal-dialog__content">
				{@render children()}
			</div>
		</div>
	</div>
{/if}

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path if needed

	// Block: modal-dialog
	.modal-dialog {
		// --- Overlay Styling ---
		&__overlay {
			position: fixed;
			inset: 0; // Equivalent to top, right, bottom, left = 0
			z-index: 1000; // Ensure it's on top
			background-color: rgba(0, 0, 0, 0.35); // Dimming effect (use a variable?)
			backdrop-filter: blur(3px); // Background blur
			display: flex;
			align-items: center; // Vertical centering
			justify-content: center; // Horizontal centering
			padding: $spacing-md; // Prevent content touching edges on small screens
			overflow-y: auto; // Allow overlay to scroll if content is too tall
			outline: none; // Remove focus outline from overlay itself
		}

		// --- Dialog Box Styling ---
		// Using '& {' to target the block root (.modal-dialog) within the BEM structure
		& {
			background-color: $color-surface;
			border-radius: $border-radius-lg;
			box-shadow: $box-shadow-xl; // Prominent shadow
			padding: $spacing-xl;
			position: relative; // For positioning close button
			max-height: calc(95vh - 2 * $spacing-md); // Limit height relative to viewport & padding
			max-width: 95vw; // Limit width relative to viewport
			display: flex;
			flex-direction: column;
			overflow: hidden; // Content area will scroll
			margin: auto; // Ensure centering if flex properties change
			outline: none; // Remove focus outline

			// -- Width Modifiers --
			&--width-auto {
				width: auto;
			}
			&--width-sm {
				width: 480px;
			} // Use variables if specific widths defined
			&--width-md {
				width: 600px;
			}
			&--width-lg {
				width: 800px;
			}
		}

		// Element: Close Button
		&__close-button {
			position: absolute;
			top: $spacing-md; // Position inside top-right corner
			right: $spacing-md;
			// Reset button styles
			background: none;
			border: none;
			padding: $spacing-xs;
			margin: -$spacing-xs; // Increase clickable area without affecting layout
			cursor: pointer;
			color: $color-text-secondary;
			border-radius: $border-radius-circle;
			display: inline-flex; // Align icon properly
			align-items: center;
			justify-content: center;
			line-height: 0; // Prevent extra height from line-height
			z-index: 1; // Ensure it's above content

			&:hover {
				color: $color-text-primary;
				background-color: $color-surface-alt;
			}
			&:focus-visible {
				outline: 2px solid $color-primary-light;
				outline-offset: 1px;
				background-color: $color-surface-alt; // Visual feedback on focus
			}

			// Style the icon (assuming SVG)
			svg {
				width: 20px;
				height: 20px;
			}
		}

		// Element: Content Area
		&__content {
			// Adjust top margin if close button placement needs more space
			margin-top: $spacing-lg; // Space below potential close button area
			overflow-y: auto; // Make content scrollable
			overflow-x: hidden; // Prevent horizontal scroll unless intended
			// Add padding to right to avoid scrollbar overlap (optional)
			padding-right: $spacing-xs;
			margin-right: -$spacing-xs;

			// Style direct children if needed (e.g., consistent heading/paragraph spacing)
			:global(h1),
			:global(h2),
			:global(h3) {
				margin-top: 0;
			}
		}
	}
</style>
