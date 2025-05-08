<script lang="ts">
	import { goto } from '$app/navigation'; // For navigation
	// Define Template type (or import)
	interface Template {
		id: string;
		title: string;
		code: string;
		dateCreated: string;
		status: string; // Keep as string if statuses vary widely
		tags: string[];
	}

	// Props
	let {
		template,
		onActionClick = (id: string) => {
			console.warn('onActionClick not provided for template', id);
		}
	}: {
		template: Template;
		onActionClick?: (id: string) => void;
	} = $props();

	// Format date helper
	function formatDate(dateString: string): string {
		try {
			return new Date(dateString).toLocaleDateString(undefined, {
				year: 'numeric',
				month: '2-digit',
				day: '2-digit'
			});
		} catch (e) {
			console.error('Error formatting date:', e);
			return 'Invalid Date';
		}
	}
	// Reactive formatted date (using $: as requested previously)
	let formattedDate = $derived(formatDate(template.dateCreated));

	// Lowercase status for class modifier
	const statusModifier = $derived(() => template.status.toLowerCase());

	function handleRowClick(): void {
		// Navigate to the analytics page for this specific session
		goto(`/overview/templates/${template.id}/overview`);
	}
</script>

<tr
	class="template-row"
	onclick={handleRowClick}
	title={`View details for ${template.title}`}
	aria-label={`View details for ${template.title}`}
>
	<td class="template-row__cell template-row__cell--title-code">
		<span class="template-row__title">{template.title}</span>
		<span class="template-row__code">{template.code}</span>
	</td>

	<td class="template-row__cell template-row__cell--date">{formattedDate}</td>

	<td class="template-row__cell template-row__cell--status">
		<span class="template-row__status template-row__status--{statusModifier}">
			{template.status}
		</span>
	</td>

	<td class="template-row__cell template-row__cell--tags">
		<div class="template-row__tags">
			{#each template.tags as tag (tag)}
				<span class="template-row__tag">{tag}</span>
			{/each}
		</div>
	</td>

	<td class="template-row__cell template-row__cell--actions">
		<button
			class="template-row__action-button"
			aria-label={`Actions for ${template.title}`}
			onclick={() => onActionClick(template.id)}
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

	// Block: template-row (applies to the <tr>)
	.template-row {
		// Styles for the row itself (e.g., hover, transitions)
		transition: background-color $transition-duration-fast;
		cursor: pointer;

		&:hover {
			background-color: $color-surface-alt; // Example hover
		}

		// Element: Cell (td)
		&__cell {
			padding: $spacing-sm $spacing-md; // Vertical: 16px, Horizontal: 8px
			text-align: left;
			border-bottom: $border-width-thin solid $color-border-light;
			white-space: nowrap;
			color: $color-text-primary;
			vertical-align: middle; // Keep content centered vertically

			// --- Cell Modifiers ---
			&--title-code {
				// Specific styles if needed
			}
			&--date {
				// Specific styles if needed
				white-space: nowrap; // Ensure date doesn't wrap
			}
			&--status {
				// Specific styles if needed
			}
			&--tags {
				white-space: normal; // Allow this specific cell to wrap content
			}
			&--actions {
				text-align: right; // Align action button right
			}
		}

		// Element: Title text
		&__title {
			display: block; // Ensure it takes block space if needed
			font-weight: $font-weight-medium;
			color: $color-text-primary;
		}

		// Element: Code text
		&__code {
			display: block; // Ensure it takes block space
			font-size: $font-size-xs;
			color: $color-text-secondary;
			margin-top: $spacing-xs * 0.5;
		}

		// Element: Status Indicator
		&__status {
			display: inline-block; // Allows padding/background
			// padding: $spacing-xs $spacing-sm;
			border-radius: $border-radius-pill;
			font-size: $font-size-xs;
			font-weight: $font-weight-medium;
			text-transform: uppercase;
			white-space: nowrap;

			// -- Status Modifiers --
			&--inactive {
				// Corresponds to template.status = 'Inactive'
				background-color: $color-surface-alt;
				color: $color-text-secondary;
			}
			&--active {
				// Corresponds to template.status = 'Active'
				background-color: rgba($color-success, 0.15);
				color: darken($color-success, 10%);
			}
			// Add other status modifiers as needed
		}

		// Element: Tags Container
		&__tags {
			display: flex;
			flex-wrap: wrap;
			gap: $spacing-xs;
			max-width: 250px; // Keep max-width to prevent excessive cell stretching
		}

		// Element: Individual Tag
		&__tag {
			background-color: $color-surface-alt;
			color: $color-text-secondary;
			padding: $spacing-xs $spacing-sm;
			border-radius: $border-radius-pill;
			font-size: $font-size-xs;
			font-weight: $font-weight-medium;
			white-space: nowrap;
		}

		// Element: Action Button
		&__action-button {
			background: none;
			border: none;
			padding: $spacing-xs;
			cursor: pointer;
			color: $color-text-secondary;
			border-radius: $border-radius-circle;
			display: inline-flex; // Use inline-flex for alignment
			align-items: center;
			justify-content: center;
			line-height: 0; // Prevent extra height

			// --- SVG inside button ---
			svg {
				width: 18px;
				height: 18px;
				stroke-width: 1.5;
			}

			// --- Button States ---
			&:hover {
				background-color: $color-surface-alt;
				color: $color-text-primary;
			}
			&:focus-visible {
				outline: 2px solid $color-primary-light;
				outline-offset: 1px;
			}
		}
	}
</style>
