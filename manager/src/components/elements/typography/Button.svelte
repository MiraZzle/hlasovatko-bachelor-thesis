<script lang="ts">
	/* eslint-disable @typescript-eslint/no-unused-vars */
	export let type: 'primary' | 'secondary' | 'dark' | 'danger' = 'primary';
	export let width: 'full' | 'auto' | '150' = 'auto';

	export let actionType: 'link' | 'action' = 'action';
	export let action = () => {};
	export let href: string = '';
	export let disabled: boolean = false;
	let btnType = `btn_${type}`;
	let btnWidth = `btn-width_${width}`;

	function handleClick() {
		if (disabled) return;
		if (actionType === 'link') {
			window.location.href = href;
		} else {
			action();
		}
	}
</script>

<button class="btn {btnType} {btnWidth} {disabled ? 'btn_disabled' : ''}" on:click={handleClick}>
	<slot></slot>
</button>

<style lang="scss">
	.btn {
		cursor: pointer;
		display: flex;
		border-radius: 8px;
		padding: 12px;
		border: none;
		gap: 8px;
		justify-content: center;
		align-items: center;
		font-weight: 500;
		transition:
			background-color 0.2s ease,
			color 0.2s ease;

		&.btn_disabled {
			cursor: not-allowed;
			opacity: 0.5;
		}

		&.btn-width_full {
			width: 100%;
		}

		&.btn-width_auto {
			width: auto;
		}

		&.btn-width_150 {
			width: 150px;
			padding: 0;
		}

		&_primary {
			background-color: #007bff;
			color: #ffffff;

			&:hover {
				background-color: #0056b3;
			}
		}

		&_secondary {
			background: none;
			border: 1px solid #6c757d;
			color: #6c757d;

			&:hover {
				background-color: #dfdfdf;
			}
		}

		&_dark {
			background-color: #343a40;
			color: #ffffff;

			&:hover {
				background-color: #1d2124;
			}
		}

		&_danger {
			background-color: #dc3545;
			color: #ffffff;

			&:hover {
				background-color: #bd2130;
			}
		}
	}
</style>
