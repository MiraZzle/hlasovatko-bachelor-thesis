<script lang="ts">
	import { goto } from '$app/navigation'; // For navigation
	interface Session {
		id: string;
		title: string;
		templateCode: string; // Assuming sessions relate to a template code
		created: string;
		status: 'Active' | 'Inactive' | 'Finished'; // Example statuses
		participants: number;
	}

	// Props
	let {
		session,
		onActionClick = (id: string) => {
			console.warn('onActionClick not provided for session', id);
		}
	}: {
		session: Session;
		onActionClick?: (id: string) => void;
	} = $props();

	// Format date helper (using $: for reactivity)
	function formatDate(dateString: string): string {
		try {
			return new Date(dateString).toLocaleDateString(undefined, {
				year: 'numeric',
				month: '2-digit',
				day: '2-digit'
			});
		} catch (e) {
			console.error('Error formatting date:', dateString, e);
			return 'Invalid Date';
		}
	}
	let formattedDate = $derived(formatDate(session.created));

	// --- NEW: Row Click Handler ---
	function handleRowClick(): void {
		// Navigate to the analytics page for this specific session
		goto(`/overview/sessions/${session.id}/overview`);
	}

	// Lowercase status for class modifier
	const statusModifier = $derived(() => session.status.toLowerCase());
</script>

<tr
	class="session-row"
	onclick={handleRowClick}
	title={`View analytics for ${session.title}`}
	aria-label={`View analytics for ${session.title}`}
>
	<td class="session-row__cell session-row__cell--title-code">
		<span class="session-row__title">{session.title}</span>
		<span class="session-row__code">({session.templateCode})</span>
	</td>

	<td class="session-row__cell session-row__cell--date">{formattedDate}</td>

	<td class="session-row__cell session-row__cell--status">
		<span class="session-row__status session-row__status--{statusModifier}">
			{session.status}
		</span>
	</td>

	<td class="session-row__cell session-row__cell--participants">{session.participants}</td>

	<td class="session-row__cell session-row__cell--actions">
		<button
			class="session-row__action-button"
			aria-label={`Actions for session ${session.title}`}
			onclick={() => onActionClick(session.id)}
			type="button"
		>
			<svg
				width="20"
				height="20"
				viewBox="0 0 24 24"
				fill="none"
				stroke="currentColor"
				stroke-width="1.5"
			>
				<path
					d="M12 5.25a.75.75 0 1 1 0-1.5.75.75 0 0 1 0 1.5Z"
					stroke-linecap="round"
					stroke-linejoin="round"
				/>
				<path
					d="M12 12.75a.75.75 0 1 1 0-1.5.75.75 0 0 1 0 1.5Z"
					stroke-linecap="round"
					stroke-linejoin="round"
				/>
				<path
					d="M12 20.25a.75.75 0 1 1 0-1.5.75.75 0 0 1 0 1.5Z"
					stroke-linecap="round"
					stroke-linejoin="round"
				/>
			</svg>
		</button>
	</td>
</tr>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path if needed

	// Block: session-row (applies to the <tr>)
	.session-row {
		transition: background-color $transition-duration-fast;
		cursor: pointer;
		&:hover {
			background-color: $color-surface-alt;
		}

		// Element: Cell (td)
		&__cell {
			padding: $spacing-sm $spacing-md; // Vertical: 16px, Horizontal: 8px
			text-align: left;
			border-bottom: $border-width-thin solid $color-border-light;
			white-space: nowrap;
			color: $color-text-primary;
			vertical-align: middle;

			// --- Cell Modifiers ---
			&--title-code {
				/* specific styles */
			}
			&--date {
				white-space: nowrap;
			}
			&--status {
				/* specific styles */
			}
			&--participants {
			} // Align numbers right
			&--actions {
			}
		}

		// Element: Title text
		&__title {
			display: block;
			font-weight: $font-weight-medium;
			color: $color-text-primary;
		}

		// Element: Code text (Template code)
		&__code {
			display: block;
			font-size: $font-size-xs;
			color: $color-text-secondary;
			margin-top: $spacing-xs * 0.5;
		}

		// Element: Status Indicator
		&__status {
			display: inline-block;
			border-radius: $border-radius-pill;
			font-size: $font-size-xs;
			font-weight: $font-weight-medium;
			text-transform: uppercase;
			white-space: nowrap;

			// -- Status Modifiers --
			&--inactive {
				background-color: $color-surface-alt;
				color: $color-text-secondary;
			}
			&--active {
				background-color: rgba($color-success, 0.15);
				// color: darken($color-success, 10%);
			}
			&--finished {
				// Added Finished status style
				// background-color: darken($color-surface-alt, 5%);
				color: $color-text-disabled;
			}
		}

		// Element: Action Button
		&__action-button {
			background: none;
			border: none;
			padding: $spacing-xs;
			cursor: pointer;
			color: $color-text-secondary;
			border-radius: $border-radius-circle;
			display: inline-flex;
			align-items: center;
			justify-content: center;
			line-height: 0;
			svg {
				width: 18px;
				height: 18px;
				stroke-width: 1.5;
			}
			&:hover {
				background-color: $color-surface-alt;
				color: $color-text-primary;
			}
			&:focus-visible {
				outline: 2px solid $color-primary-light;
				outline-offset: 1px;
			}
		}
		// Note: No __tags or __tag elements in SessionRow
	}
</style>
