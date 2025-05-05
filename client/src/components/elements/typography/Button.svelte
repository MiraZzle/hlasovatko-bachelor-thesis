<script lang="ts">
	import type { Snippet } from 'svelte';

	type ButtonVariant = 'primary' | 'secondary' | 'outline'; // Add more as needed

	let {
		variant = 'primary' as ButtonVariant,
		onclick = () => {
			console.log('Button clicked!');
		},
		href = null as string | null,
		type = 'button' as 'button' | 'submit' | 'reset',
		disabled = false,
		fullWidth = false,
		children
	}: {
		variant?: ButtonVariant;
		onclick?: (event: MouseEvent) => void;
		href?: string | null;
		type?: 'button' | 'submit' | 'reset';
		disabled?: boolean;
		fullWidth?: boolean;
		children: Snippet;
	} = $props();

	// Reactive class list based on props
	let buttonClass = $derived(`button button--${variant} ${fullWidth ? 'button--full-width' : ''}`);
</script>

{#if href}
	<a {href} class={buttonClass} aria-disabled={disabled} role="button">
		{@render children()}
	</a>
{:else}
	<button {type} class={buttonClass} {disabled} {onclick}>
		{@render children()}
	</button>
{/if}

<style lang="scss">
	@import '../../../styles/variables.scss';

	.button {
		display: inline-flex; // Use inline-flex for alignment
		align-items: center;
		justify-content: center;
		padding: $spacing-sm $spacing-lg; // Example padding
		border-radius: $border-radius-md;
		font-family: $font-family-primary;
		font-size: $font-size-md;
		font-weight: $font-weight-medium;
		text-align: center;
		text-decoration: none;
		cursor: pointer;
		border: $border-width-medium solid transparent;
		transition:
			background-color $transition-duration-fast $transition-timing-function,
			border-color $transition-duration-fast $transition-timing-function,
			color $transition-duration-fast $transition-timing-function,
			opacity $transition-duration-fast $transition-timing-function;
		white-space: nowrap; // Prevent wrapping

		&:hover:not(:disabled) {
			opacity: 0.9; // Slight opacity change on hover
		}

		&:focus-visible {
			outline: 2px solid $color-primary-light; // Accessibility focus ring
			outline-offset: 2px;
		}

		&:disabled {
			cursor: not-allowed;
			opacity: 0.6;
		}

		// Variants using BEM modifier convention
		&--primary {
			background-color: $color-primary;
			color: $color-text-on-primary;
			border-color: $color-primary;

			&:hover:not(:disabled) {
				background-color: $color-primary-dark;
				border-color: $color-primary-dark;
			}

			&:disabled {
				background-color: $color-button-disabled-bg;
				border-color: $color-button-disabled-bg;
				color: $color-text-disabled;
			}
		}

		&--secondary {
			background-color: $color-secondary;
			color: $color-text-on-secondary;
			border-color: $color-secondary;

			&:hover:not(:disabled) {
				background-color: $color-secondary-dark;
				border-color: $color-secondary-dark;
			}
			&:disabled {
				background-color: $color-button-disabled-bg;
				border-color: $color-button-disabled-bg;
				color: $color-text-disabled;
			}
		}

		&--outline {
			background-color: transparent;
			color: $color-primary;
			border-color: $color-primary;

			&:hover:not(:disabled) {
				background-color: $color-primary;
				color: $color-text-on-primary;
			}
			&:disabled {
				border-color: $color-button-disabled-bg;
				color: $color-text-disabled;
				background-color: transparent;
			}
		}

		// Modifier for full width
		&--full-width {
			width: 100%;
			display: flex; // Ensure flex properties apply
		}
	}
</style>
