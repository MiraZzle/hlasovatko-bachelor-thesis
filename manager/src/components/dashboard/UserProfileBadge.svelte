<script lang="ts">
	import { onMount, onDestroy, tick } from 'svelte';
	import { goto } from '$app/navigation';
	import { signout } from '$lib/auth/scripts';

	let {
		initials = 'XY',
		ariaLabel = 'User menu'
	}: {
		initials?: string;
		ariaLabel?: string;
	} = $props();

	let isDropdownOpen = $state(false);
	let dropdownElement: HTMLDivElement | null = $state(null);
	let buttonElement: HTMLButtonElement | null = $state(null);

	function toggleDropdown(): void {
		isDropdownOpen = !isDropdownOpen;
		if (isDropdownOpen) {
			tick().then(() => dropdownElement?.focus());
		}
	}

	function closeDropdown(): void {
		isDropdownOpen = false;
	}

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

	function handleKeydown(event: KeyboardEvent): void {
		if (isDropdownOpen && event.key === 'Escape') {
			closeDropdown();
			buttonElement?.focus();
		}
	}

	function goToProfile(): void {
		goto('/overview/profile');
		closeDropdown();
	}

	function handleSignOut(): void {
		signout();
		closeDropdown();
	}

	onMount(() => {
		window.addEventListener('click', handleClickOutside, true);
		window.addEventListener('keydown', handleKeydown);
	});

	onDestroy(() => {
		window.removeEventListener('click', handleClickOutside, true);
		window.removeEventListener('keydown', handleKeydown);
	});

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
	@import '../../styles/variables.scss';

	.user-profile {
		position: relative;
		display: inline-block;

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
			&[aria-expanded='true'] {
				box-shadow: 0 0 0 3px rgba($color-primary-light, 0.5);
			}
		}
		&__dropdown {
			position: absolute;
			top: calc(100% + #{$spacing-xs});
			right: 0;
			background-color: $color-surface;
			border-radius: $border-radius-md;
			box-shadow: $box-shadow-lg;
			border: 1px solid $color-border-light;
			min-width: 180px;
			z-index: 1100;
			padding: $spacing-xs 0;
			outline: none;
		}

		&__menu-list {
			list-style: none;
			padding: 0;
			margin: 0;
		}

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
				outline-offset: -2px;
				background-color: $color-surface-alt;
			}
		}

		&__menu-icon {
			display: inline-flex;
			align-items: center;
			justify-content: center;
			width: 16px;
			height: 16px;
			color: $color-text-secondary;
			flex-shrink: 0;

			.user-profile__menu-button:hover & {
				color: $color-primary;
			}
		}
	}
</style>
