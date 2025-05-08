<script lang="ts">
	// Props: checked (bindable), disabled, label (optional for accessibility)
	type Props = {
		checked?: boolean;
		disabled?: boolean;
		label?: string; // For aria-label
		onchange?: (event: { checked: boolean }) => void; // Optional callback
	};

	let {
		checked = $bindable(false), // Bindable state
		disabled = false,
		label = 'Toggle Switch', // Default accessible label
		onchange = undefined // Optional callback for state change
	}: Props = $props();

	// Internal toggle function
	function toggle(): void {
		if (!disabled) {
			// Update the bound state directly
			checked = !checked;
			// Call the onchange callback if provided
			if (onchange) {
				onchange({ checked });
			}
		}
	}

	// Handle keyboard interaction (Enter/Space)
	function handleKeydown(event: KeyboardEvent): void {
		if (!disabled && (event.key === 'Enter' || event.key === ' ')) {
			event.preventDefault(); // Prevent space scrolling
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
	@import '../../styles/variables.scss'; // Adjust path if needed

	// Block: toggle-switch
	.toggle-switch {
		position: relative;
		display: inline-block; // Allows placement next to text
		width: 44px; // Slightly wider
		height: 24px;
		background-color: $color-border; // Default off background
		border-radius: $border-radius-pill; // Fully rounded
		cursor: pointer;
		transition: background-color $transition-duration-fast;
		vertical-align: middle; // Align nicely with text
		border: none; // Remove default button border
		padding: 0; // Remove default button padding

		// Element: Knob (the circle)
		&__knob {
			position: absolute;
			content: '';
			width: 20px; // Knob size
			height: 20px;
			background-color: $color-surface; // White knob
			border-radius: 50%;
			top: 2px; // Position within the track
			left: 2px;
			transition: transform $transition-duration-fast ease-in-out;
			box-shadow: $box-shadow-sm; // Subtle shadow on knob
		}

		// Modifier: Checked state
		&--checked {
			background-color: $color-primary; // Use primary color for on state

			// Move knob to the right when checked
			.toggle-switch__knob {
				transform: translateX(20px); // Move knob (width - knob_width - 2*offset)
			}
		}

		// Modifier: Disabled state
		&--disabled {
			background-color: $color-button-disabled-bg;
			cursor: not-allowed;
			opacity: 0.7;

			.toggle-switch__knob {
				background-color: darken($color-surface, 10%);
			}
		}

		// Focus state
		&:focus-visible {
			outline: 2px solid $color-primary-light;
			outline-offset: 2px;
		}
	}
</style>
