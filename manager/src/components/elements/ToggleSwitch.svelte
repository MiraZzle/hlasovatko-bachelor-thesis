<script lang="ts">
	type Props = {
		checked?: boolean;
		disabled?: boolean;
		label?: string;
		onchange?: (event: { checked: boolean }) => void;
	};

	let {
		checked = $bindable(false),
		disabled = false,
		label = 'Toggle Switch',
		onchange = undefined
	}: Props = $props();

	function toggle(): void {
		if (!disabled) {
			checked = !checked;
			if (onchange) {
				onchange({ checked });
			}
		}
	}

	function handleKeydown(event: KeyboardEvent): void {
		if (!disabled && (event.key === 'Enter' || event.key === ' ')) {
			event.preventDefault();
			toggle();
		}
	}
</script>

<button
	type="button"
	class="toggle-switch"
	class:toggle-switch--checked={checked}
	class:toggle-switch--disabled={disabled}
	role="switch"
	aria-checked={checked}
	aria-label={label}
	tabindex={disabled ? -1 : 0}
	onclick={toggle}
	onkeydown={handleKeydown}
	{disabled}
>
	<span class="toggle-switch__knob" aria-hidden="true"></span>
</button>

<style lang="scss">
	.toggle-switch {
		position: relative;
		display: inline-block;
		width: 44px;
		height: 24px;
		background-color: $color-border;
		border-radius: $border-radius-pill;
		cursor: pointer;
		transition: background-color $transition-duration-fast;
		vertical-align: middle;
		border: none;
		padding: 0;

		&__knob {
			position: absolute;
			content: '';
			width: 20px;
			height: 20px;
			background-color: $color-surface;
			border-radius: 50%;
			top: 2px;
			left: 2px;
			transition: transform $transition-duration-fast ease-in-out;
			box-shadow: $box-shadow-sm;
		}

		&--checked {
			background-color: $color-primary;

			.toggle-switch__knob {
				transform: translateX(20px);
			}
		}
		&--disabled {
			background-color: $color-button-disabled-bg;
			cursor: not-allowed;
			opacity: 0.7;

			.toggle-switch__knob {
				background-color: darken($color-surface, 10%);
			}
		}
		&:focus-visible {
			outline: 2px solid $color-primary-light;
			outline-offset: 2px;
		}
	}
</style>
