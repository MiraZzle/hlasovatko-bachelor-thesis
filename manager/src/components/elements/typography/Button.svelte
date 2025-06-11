<script lang="ts">
	import type { Snippet } from 'svelte';

	type ButtonVariant =
		| 'primary'
		| 'secondary'
		| 'outline'
		| 'danger'
		| 'success'
		| 'warning'
		| 'info'
		| 'danger-outline'
		| 'success-outline'
		| 'warning-outline'
		| 'info-outline'
		| 'link';

	let {
		variant = 'primary',
		onclick = () => {
			console.log('Button clicked!');
		},
		href = null,
		type = 'button',
		disabled = false,
		fullWidth = false,
		size = 'md',
		children
	}: {
		variant?: ButtonVariant;
		onclick?: (event: MouseEvent) => void;
		href?: string | null;
		type?: 'button' | 'submit' | 'reset';
		disabled?: boolean;
		fullWidth?: boolean;
		size?: 'sm' | 'md' | 'lg';
		children: Snippet;
	} = $props();

	// dynamic class for button type based on props
	let buttonClass = $derived(
		`button button--${variant} button--size-${size} ${fullWidth ? 'button--full-width' : ''}`
	);
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
		display: inline-flex;
		align-items: center;
		justify-content: center;
		border-radius: $border-radius-md;
		font-family: $font-family-primary;
		font-weight: 500;
		text-align: center;
		text-decoration: none;
		cursor: pointer;
		border: $border-width-medium solid transparent;
		transition:
			background-color $transition-duration-fast $transition-timing-function,
			border-color $transition-duration-fast $transition-timing-function,
			color $transition-duration-fast $transition-timing-function,
			opacity $transition-duration-fast $transition-timing-function,
			box-shadow $transition-duration-fast $transition-timing-function;
		white-space: nowrap;
		user-select: none;

		&:hover:not(:disabled) {
			opacity: 0.9;
		}

		&:focus-visible {
			outline: 2px solid $color-primary-light;
			outline-offset: 2px;
		}

		&:disabled {
			cursor: not-allowed;
			opacity: 0.6;
			transform: none;
			box-shadow: none;
		}

		&--size-sm {
			padding: $spacing-xs $spacing-md;
			font-size: $font-size-sm;
			border-width: 1px;
		}
		&--size-md {
			padding: $spacing-sm $spacing-lg;
			font-size: $font-size-md;
			border-width: $border-width-medium;
		}
		&--size-lg {
			padding: $spacing-md $spacing-xl;
			font-size: $font-size-lg;
			border-width: $border-width-medium;
		}

		&--primary {
			background-color: $color-primary;
			color: $color-text-on-primary;
			border-color: $color-primary;
			&:hover:not(:disabled) {
				background-color: $color-primary-dark;
				border-color: $color-primary-dark;
				opacity: 1;
			}
			&:disabled {
				background-color: $color-button-disabled-bg;
				border-color: $color-button-disabled-bg;
				color: $color-text-disabled;
			}
			&:focus-visible {
				outline-color: $color-primary-light;
			}
		}

		&--secondary {
			background-color: $color-secondary;
			color: $color-text-on-secondary;
			border-color: $color-secondary;
			&:hover:not(:disabled) {
				background-color: $color-secondary-dark;
				border-color: $color-secondary-dark;
				opacity: 1;
			}
			&:disabled {
				background-color: $color-button-disabled-bg;
				border-color: $color-button-disabled-bg;
				color: $color-text-disabled;
			}
			&:focus-visible {
				outline-color: $color-secondary-light;
			}
		}

		&--outline {
			background-color: transparent;
			color: $color-primary;
			border-color: $color-primary;
			&:hover:not(:disabled) {
				background-color: rgba($color-primary, 0.08);
				opacity: 1;
			}
			&:disabled {
				border-color: $color-button-disabled-bg;
				color: $color-text-disabled;
				background-color: transparent;
			}
			&:focus-visible {
				outline-color: $color-primary-light;
			}
		}

		&--danger {
			background-color: $color-error;
			color: $color-text-on-primary;
			border-color: $color-error;
			&:hover:not(:disabled) {
				background-color: darken($color-error, 10%);
				border-color: darken($color-error, 10%);
				opacity: 1;
			}
			&:disabled {
				background-color: $color-button-disabled-bg;
				border-color: $color-button-disabled-bg;
				color: $color-text-disabled;
			}
			&:focus-visible {
				outline-color: lighten($color-error, 20%);
			}
		}
		&--danger-outline {
			background-color: transparent;
			color: $color-error;
			border-color: $color-error;
			&:hover:not(:disabled) {
				background-color: rgba($color-error, 0.08);
				opacity: 1;
			}
			&:disabled {
				border-color: $color-button-disabled-bg;
				color: $color-text-disabled;
				background-color: transparent;
			}
			&:focus-visible {
				outline-color: lighten($color-error, 20%);
			}
		}
		&--success {
			background-color: $color-success;
			color: $color-text-on-primary;
			border-color: $color-success;
			&:hover:not(:disabled) {
				background-color: darken($color-success, 10%);
				border-color: darken($color-success, 10%);
				opacity: 1;
			}
			&:disabled {
				background-color: $color-button-disabled-bg;
				border-color: $color-button-disabled-bg;
				color: $color-text-disabled;
			}
			&:focus-visible {
				outline-color: lighten($color-success, 20%);
			}
		}
		&--success-outline {
			background-color: transparent;
			color: $color-success;
			border-color: $color-success;
			&:hover:not(:disabled) {
				background-color: rgba($color-success, 0.08);
				opacity: 1;
			}
			&:disabled {
				border-color: $color-button-disabled-bg;
				color: $color-text-disabled;
				background-color: transparent;
			}
			&:focus-visible {
				outline-color: lighten($color-success, 20%);
			}
		}

		&--warning {
			background-color: $color-warning;
			color: $color-text-primary;
			border-color: $color-warning;
			&:hover:not(:disabled) {
				background-color: darken($color-warning, 10%);
				border-color: darken($color-warning, 10%);
				opacity: 1;
			}
			&:disabled {
				background-color: $color-button-disabled-bg;
				border-color: $color-button-disabled-bg;
				color: $color-text-disabled;
			}
			&:focus-visible {
				outline-color: lighten($color-warning, 20%);
			}
		}
		&--warning-outline {
			background-color: transparent;
			color: darken($color-warning, 10%);
			border-color: $color-warning;
			&:hover:not(:disabled) {
				background-color: rgba($color-warning, 0.08);
				opacity: 1;
			}
			&:disabled {
				border-color: $color-button-disabled-bg;
				color: $color-text-disabled;
				background-color: transparent;
			}
			&:focus-visible {
				outline-color: lighten($color-warning, 20%);
			}
		}

		&--info {
			background-color: $color-info;
			color: $color-text-on-primary;
			border-color: $color-info;
			&:hover:not(:disabled) {
				background-color: darken($color-info, 10%);
				border-color: darken($color-info, 10%);
				opacity: 1;
			}
			&:disabled {
				background-color: $color-button-disabled-bg;
				border-color: $color-button-disabled-bg;
				color: $color-text-disabled;
			}
			&:focus-visible {
				outline-color: lighten($color-info, 20%);
			}
		}
		&--info-outline {
			background-color: transparent;
			color: $color-info;
			border-color: $color-info;
			&:hover:not(:disabled) {
				background-color: rgba($color-info, 0.08);
				opacity: 1;
			}
			&:disabled {
				border-color: $color-button-disabled-bg;
				color: $color-text-disabled;
				background-color: transparent;
			}
			&:focus-visible {
				outline-color: lighten($color-info, 20%);
			}
		}

		&--link {
			background: none;
			border: none;
			color: $color-link;
			text-decoration: underline;
			padding: 0;
			height: auto;
			font-weight: $font-weight-regular;
			border-width: 0;

			&:hover:not(:disabled) {
				color: $color-link-hover;
				opacity: 1;
				text-decoration: underline;
			}
			&:disabled {
				color: $color-text-disabled;
				text-decoration: none;
				opacity: 0.6;
			}
			&:focus-visible {
				outline: none;
				text-decoration: underline;
				box-shadow: 0 1px $color-primary-light;
			}
		}

		&--full-width {
			width: 100%;
			display: flex;
		}
	}
</style>
