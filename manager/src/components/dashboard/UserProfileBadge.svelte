<script lang="ts">
	import { onMount, onDestroy, tick } from 'svelte';
	import { goto } from '$app/navigation'; // For navigation

	// --- Component Props ---
	type Props = {
		initials?: string;
		ariaLabel?: string;
		// Add any other props if needed, e.g., user object for more details
	};

	let { initials = '??', ariaLabel = 'User menu' }: Props = $props();

	// --- State for Dropdown ---
	let isDropdownOpen = $state(false);
	let dropdownElement: HTMLDivElement | null = null; // Ref to the dropdown
	let buttonElement: HTMLButtonElement | null = null; // Ref to the button

	// --- Handlers ---
	function toggleDropdown(): void {
		isDropdownOpen = !isDropdownOpen;
		if (isDropdownOpen) {
			// Optional: focus the dropdown or first item after it opens
			tick().then(() => dropdownElement?.focus());
		}
	}

	function closeDropdown(): void {
		isDropdownOpen = false;
	}

	// Close dropdown if clicked outside
	function handleClickOutside(event: MouseEvent): void {
		if (
			isDropdownOpen &&
			dropdownElement &&
			!dropdownElement.contains(event.target as Node) &&
			buttonElement &&
			!buttonElement.contains(event.target as Node)
		) {
			closeDropdown();
		}
	}

	// Close dropdown on Escape key
	function handleKeydown(event: KeyboardEvent): void {
		if (isDropdownOpen && event.key === 'Escape') {
			closeDropdown();
			buttonElement?.focus(); // Return focus to the button
		}
	}

	// --- Menu Item Actions ---
	function goToProfile(): void {
		console.log('Navigate to profile');
		goto('/overview/profile'); // Adjust route as needed
		closeDropdown();
	}

	function handleSignOut(): void {
		console.log('Signing out...');
		// TODO: Implement actual sign-out logic (clear session, API call, etc.)
		alert('Signed out (Placeholder)');
		goto('/login'); // Redirect to login page
		closeDropdown();
	}

	// --- Lifecycle for Global Listeners ---
	onMount(() => {
		window.addEventListener('click', handleClickOutside, true); // Use capture phase
		window.addEventListener('keydown', handleKeydown);
	});

	onDestroy(() => {
		window.removeEventListener('click', handleClickOutside, true);
		window.removeEventListener('keydown', handleKeydown);
	});

	// Placeholder icons (replace with actual SVGs or components)
	const IconUser = '👤';
	const IconSignOut = '🚪';
</script>

<div class="user-profile">
	<button
		class="user-profile__badge"
		type="button"
		aria-label={ariaLabel}
		aria-haspopup="true"
		aria-expanded={isDropdownOpen}
		onclick={toggleDropdown}
		bind:this={buttonElement}
	>
		<span class="user-profile__initials" aria-hidden="true">{initials}</span>
	</button>

	{#if isDropdownOpen}
		<div
			class="user-profile__dropdown"
			role="menu"
			aria-orientation="vertical"
			aria-labelledby={buttonElement?.id || undefined}
			bind:this={dropdownElement}
			tabindex="-1"
		>
			<ul class="user-profile__menu-list">
				<li class="user-profile__menu-item" role="none">
					<button
						type="button"
						class="user-profile__menu-button"
						role="menuitem"
						onclick={goToProfile}
					>
						<span class="user-profile__menu-icon" aria-hidden="true">{IconUser}</span>
						Profile
					</button>
				</li>
				<li class="user-profile__menu-item" role="none">
					<button
						type="button"
						class="user-profile__menu-button"
						role="menuitem"
						onclick={handleSignOut}
					>
						<span class="user-profile__menu-icon" aria-hidden="true">{IconSignOut}</span>
						Sign Out
					</button>
				</li>
			</ul>
		</div>
	{/if}
</div>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path if needed

	// Block: user-profile
	.user-profile {
		position: relative; // For positioning the dropdown
		display: inline-block; // So it fits in the top bar

		// Element: Badge (the clickable circle)
		&__badge {
			display: inline-flex;
			align-items: center;
			justify-content: center;
			width: 40px;
			height: 40px;
			border-radius: $border-radius-circle;
			background-color: $color-primary;
			color: $color-text-on-primary;
			font-weight: $font-weight-medium;
			font-size: $font-size-md;
			cursor: pointer;
			border: none;
			overflow: hidden;
			transition:
				opacity $transition-duration-fast,
				box-shadow $transition-duration-fast;

			&:hover {
				opacity: 0.9;
			}
			&:focus-visible {
				outline: 2px solid $color-primary-light;
				outline-offset: 2px;
				box-shadow: 0 0 0 4px rgba($color-primary-light, 0.3);
			}
			// Style when dropdown is open
			&[aria-expanded='true'] {
				box-shadow: 0 0 0 3px rgba($color-primary-light, 0.5); // Indicate active state
			}
		}

		// Element: Initials text
		&__initials {
			// No specific styles needed unless changing font/size from badge
		}

		// Element: Dropdown Menu
		&__dropdown {
			position: absolute;
			top: calc(100% + #{$spacing-xs}); // Position below the badge
			right: 0;
			background-color: $color-surface;
			border-radius: $border-radius-md;
			box-shadow: $box-shadow-lg; // Prominent shadow
			border: 1px solid $color-border-light;
			min-width: 180px; // Minimum width for the dropdown
			z-index: 1100; // Ensure it's above other topbar content
			padding: $spacing-xs 0; // Padding for top/bottom of menu list
			outline: none; // Remove focus outline from the div itself

			// Animation (optional)
			// animation: fadeIn 0.15s ease-out;
		}

		// Element: Menu List
		&__menu-list {
			list-style: none;
			padding: 0;
			margin: 0;
		}

		// Element: Menu Item (li wrapper)
		&__menu-item {
			// No specific styles needed for li itself
		}

		// Element: Menu Button (actual clickable item)
		&__menu-button {
			display: flex;
			align-items: center;
			gap: $spacing-sm;
			width: 100%;
			padding: $spacing-sm $spacing-md;
			background: none;
			border: none;
			text-align: left;
			font-size: $font-size-sm;
			color: $color-text-primary;
			cursor: pointer;
			transition: background-color $transition-duration-fast;

			&:hover {
				background-color: $color-surface-alt;
				color: $color-primary;
			}
			&:focus-visible {
				outline: 2px solid $color-primary-light;
				outline-offset: -2px; // Inside padding
				background-color: $color-surface-alt;
			}
		}

		// Element: Menu Icon
		&__menu-icon {
			display: inline-flex;
			align-items: center;
			justify-content: center;
			width: 16px; // Smaller icon
			height: 16px;
			color: $color-text-secondary; // Default icon color
			flex-shrink: 0;

			.user-profile__menu-button:hover & {
				color: $color-primary; // Icon color on hover
			}
		}
	}

	// Optional: Fade-in animation for dropdown
	// @keyframes fadeIn {
	//     from { opacity: 0; transform: translateY(-5px); }
	//     to { opacity: 1; transform: translateY(0); }
	// }
</style>
