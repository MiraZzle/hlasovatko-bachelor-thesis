<script lang="ts">
	let { initials = '??', ariaLabel = 'User menu' }: { initials?: string; ariaLabel?: string } =
		$props();

	function handleClick() {
		console.log('User profile badge clicked - show menu');
		// Dummy action - dropdown menu would likely appear here
	}
</script>

<button class="user-badge" type="button" onclick={handleClick}>
	<span class="user-badge__initials" aria-hidden="true">{initials}</span>
</button>

<style lang="scss">
	@import '../../styles/variables.scss';

	.user-badge {
		display: inline-flex;
		align-items: center;
		justify-content: center;
		width: 40px;
		height: 40px;
		border-radius: $border-radius-circle;
		background-color: $color-primary; // Use primary color for background
		color: $color-text-on-primary; // White text
		font-weight: $font-weight-medium;
		font-size: $font-size-md;
		cursor: pointer;
		border: none;
		overflow: hidden; // Ensure no overflow
		transition: opacity $transition-duration-fast;

		&:hover {
			opacity: 0.9;
		}

		&:focus-visible {
			outline: 2px solid $color-primary-light;
			outline-offset: 2px;
		}
	}

	.user-badge__initials {
		// Initials specific styles if needed
	}
</style>
