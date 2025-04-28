<script lang="ts">
	// If defined globally: import type { Template } from '$lib/types';
	// Or define locally:
	interface Template {
		id: string;
		title: string;
		code: string;
		dateCreated: string;
		status: string; // Keep as string if statuses vary widely
		tags: string[];
	}

	// Props: template data and the action callback
	let {
		template,
		onActionClick = (id: string) => {
			console.warn('onActionClick not provided for', id);
		}
	}: {
		template: Template;
		onActionClick?: (id: string) => void; // Callback function prop
	} = $props();

	// Format date for display
	const formattedDate = $derived(() => {
		try {
			return new Date(template.dateCreated).toLocaleDateString(undefined, {
				// Use locale-aware formatting
				year: 'numeric',
				month: '2-digit',
				day: '2-digit'
			});
		} catch (e) {
			return 'Invalid Date';
		}
	});
</script>

<tr>
	<td>
		<div class="data-table__cell-primary">{template.title}</div>
		<div class="data-table__cell-secondary">{template.code}</div>
	</td>

	<td>{formattedDate}</td>

	<td>
		<span class="data-table__status data-table__status--{template.status.toLowerCase()}">
			{template.status}
		</span>
	</td>

	<td>
		<div class="data-table__tags">
			{#each template.tags as tag (tag)}
				<span class="data-table__tag">{tag}</span>
			{/each}
		</div>
	</td>

	<td>
		<button
			class="data-table__action-button"
			aria-label={`Actions for ${template.title}`}
			onclick={() => onActionClick(template.id)}
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
	@import '../../styles/variables.scss'; // Import if using variables here

	// Example: Styling for status indicator
	.data-table__status {
		padding: $spacing-xs $spacing-sm;
		border-radius: $border-radius-pill;
		font-size: $font-size-xs;
		font-weight: $font-weight-medium;
		text-transform: uppercase;

		&--inactive {
			background-color: $color-surface-alt;
			color: $color-text-secondary;
		}
		&--active {
			background-color: rgba($color-success, 0.15);
			color: darken($color-success, 10%);
		}
		// Add more status styles as needed
	}

	// Ensure parent styles apply correctly, if not, duplicate necessary td/tag styles here.
	// Styles like padding, text-align, borders should ideally be handled by .data-table td/th
</style>
