<script lang="ts">
	/**
	 * @file Custom toaster component for displaying notifications.
	 */

	import { toast, type ToastType } from '$lib/stores/toast_store';
	import { fade } from 'svelte/transition';

	const toastDetails: Record<ToastType, { icon: string; modifier: string }> = {
		success: { icon: '✅', modifier: 'toaster__toast--success' },
		error: { icon: '❌', modifier: 'toaster__toast--error' },
		info: { icon: 'ℹ️', modifier: 'toaster__toast--info' }
	};
</script>

<div class="toaster">
	{#each $toast as t (t.id)}
		<div
			class="toaster__toast {toastDetails[t.type].modifier}"
			in:fade={{ duration: 200 }}
			out:fade={{ duration: 250 }}
			role="alert"
		>
			<span class="toaster__icon">{toastDetails[t.type].icon}</span>
			<p class="toaster__message">{t.message}</p>
			<button class="toaster__close-button" onclick={() => toast.remove(t.id)} aria-label="Close">
				<span>&times;</span>
			</button>
		</div>
	{/each}
</div>

<style lang="scss">
	.toaster {
		position: fixed;
		bottom: 1rem;
		right: 1rem;
		z-index: $toast-z-index;
		display: flex;
		flex-direction: column;
		gap: 0.75rem;

		&__toast {
			display: flex;
			align-items: center;
			padding: $spacing-md $spacing-lg;
			font-family: $font-family-primary;
			min-width: 280px;
			max-width: 350px;

			color: $color-text-primary;
			background-color: rgba($color-surface, 0.85);
			backdrop-filter: blur(5px);
			border-radius: $border-radius-md;
			box-shadow: $box-shadow-lg;
			border-left-width: 5px;
			border-left-style: solid;

			&--success {
				border-left-color: $color-success;
			}

			&--error {
				border-left-color: $color-error;
			}

			&--info {
				border-left-color: $color-info;
			}
		}

		&__icon {
			margin-right: 0.75rem;
			font-size: 1.25rem;
			flex-shrink: 0;
		}

		&__message {
			flex-grow: 1;
			word-break: break-word;
			margin: 0;
		}

		&__close-button {
			margin-left: $spacing-md;
			padding: 0;
			background: transparent;
			border: none;
			color: $color-text-secondary;
			cursor: pointer;
			border-radius: 9999px;
			line-height: 1;
			font-size: 1.5rem;
			font-weight: bold;
			opacity: 0.7;
			width: 24px;
			height: 24px;
			display: flex;
			align-items: center;
			justify-content: center;
			transition: opacity 0.2s;

			&:hover {
				opacity: 1;
			}
		}
	}
</style>
