<script module lang="ts">
	// Define a type for our column headers
	export interface ColumnHeader<T> {
		key: keyof T | (string & {}); // The key in the data object
		label: string; // The text to display in the <th>
		sortable?: boolean; // Can this column be sorted?
		// Optional custom class for the header cell
		class?: string;
	}
</script>

<script lang="ts">
	import Button from '$components/elements/typography/Button.svelte';
	import Input from '$components/elements/typography/Input.svelte';

	// --- Component Props using Svelte 5 Runes ---
	type T = $$Generic; // Define a generic type for our data items

	let {
		items,
		columns,
		title,
		newItemLabel,
		searchPlaceholder,
		noResultsMessage = 'No items found.',
		onNewClick,
		// Two-way binding for search term and pagination
		searchTerm = $bindable(''),
		currentPage = $bindable(1),
		totalPages = 1
	} = $props<{
		items: T[];
		columns: ColumnHeader<T>[];
		title?: string;
		newItemLabel?: string;
		searchPlaceholder: string;
		noResultsMessage?: string;
		onNewClick?: () => void;
		searchTerm?: string;
		currentPage?: number;
		totalPages?: number;
	}>();

	// --- State for Sorting ---
	let sortKey = $state<keyof T | (string & {}) | null>(null);
	let sortDirection = $state<'asc' | 'desc'>('asc');

	// --- Handlers ---
	function handleSort(key: keyof T | (string & {})) {
		if (sortKey === key) {
			sortDirection = sortDirection === 'asc' ? 'desc' : 'asc';
		} else {
			sortKey = key;
			sortDirection = 'asc';
		}
	}

	function handlePreviousPage() {
		if (currentPage > 1) currentPage--;
	}

	function handleNextPage() {
		if (currentPage < totalPages) currentPage++;
	}

	// --- Derived State for Sorted Items ---
	const sortedItems = $derived.by(() => {
		if (!sortKey) return [...items];

		const sorted = [...items].sort((a, b) => {
			const valA = a[sortKey as keyof T];
			const valB = b[sortKey as keyof T];

			// Handle different data types for robust sorting
			if (typeof valA === 'number' && typeof valB === 'number') {
				return valA - valB;
			}
			if (typeof valA === 'string' && typeof valB === 'string') {
				// Check if strings are dates
				const dateA = new Date(valA);
				const dateB = new Date(valB);
				if (!isNaN(dateA.getTime()) && !isNaN(dateB.getTime())) {
					return dateA.getTime() - dateB.getTime();
				}
				// Otherwise, string comparison
				return valA.localeCompare(valB);
			}
			// Fallback for other types or mixed types
			if (valA < valB) return -1;
			if (valA > valB) return 1;
			return 0;
		});

		if (sortDirection === 'desc') {
			sorted.reverse();
		}
		return sorted;
	});
</script>

<div class="data-table-page">
	<header class="data-table-page__header">
		<h1 class="data-table-page__title">{title}</h1>
		<div class="data-table-page__search">
			<Input
				placeholder={searchPlaceholder}
				ariaLabel={searchPlaceholder}
				bind:value={searchTerm}
			/>
		</div>
		{#if onNewClick}
			<Button variant="primary" onclick={onNewClick}>+ New {newItemLabel ?? ''}</Button>
		{/if}
	</header>

	<div class="data-table-page__table-wrapper">
		<table class="data-table">
			<thead>
				<tr>
					{#each columns as column}
						<th class={column.class ?? ''}>
							{#if column.sortable}
								<button class="sortable-header" onclick={() => handleSort(column.key)}>
									{column.label}
									{#if sortKey === column.key}
										<span class="sort-icon">{sortDirection === 'asc' ? '▲' : '▼'}</span>
									{/if}
								</button>
							{:else}
								{column.label}
							{/if}
						</th>
					{/each}
				</tr>
			</thead>
			<tbody>
				{#each sortedItems as item (item.id)}
					<slot name="row" {item} />
				{:else}
					<tr>
						<td colspan={columns.length} class="data-table__no-results">
							{noResultsMessage}
						</td>
					</tr>
				{/each}
			</tbody>
		</table>
	</div>

	<footer class="data-table-page__pagination">
		<span>Page {currentPage}</span>
		<div class="data-table-page__pagination-controls">
			<Button variant="outline" onclick={handlePreviousPage} disabled={currentPage <= 1}>
				&larr; Previous
			</Button>
			<Button variant="outline" onclick={handleNextPage} disabled={currentPage >= totalPages}>
				Next &rarr;
			</Button>
		</div>
	</footer>
</div>

<style lang="scss">
	// Import your global styles
	@import '../../styles/variables.scss';

	.data-table-page {
		&__header {
			display: flex;
			justify-content: space-between;
			align-items: center;
			margin-bottom: $spacing-xl;
			gap: $spacing-lg;
			flex-wrap: wrap;
		}

		&__title {
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			margin: 0;
			flex-grow: 1;
		}

		&__search {
			max-width: 400px;
			min-width: 250px;
			:global(.input-wrapper) {
				width: 100%;
			}
		}

		&__table-wrapper {
			background-color: $color-surface;
			border-radius: $border-radius-lg;
			box-shadow: $box-shadow-sm;
			overflow-x: auto;
			margin-bottom: $spacing-xl;
		}

		&__pagination {
			display: flex;
			justify-content: space-between;
			align-items: center;
			padding: $spacing-sm;
			font-size: $font-size-sm;
			color: $color-text-secondary;
		}

		&__pagination-controls {
			display: flex;
			gap: $spacing-sm;
		}
	}

	.data-table {
		width: 100%;
		border-collapse: collapse;

		thead th {
			padding: $spacing-md;
			text-align: left;
			border-bottom: $border-width-thin solid $color-border-light;
			white-space: nowrap;
			font-weight: $font-weight-semibold;
			color: $color-text-secondary;
			background-color: #f9fafb; // A slightly different background for the header

			&:first-child {
				border-top-left-radius: $border-radius-lg;
			}
			&:last-child {
				border-top-right-radius: $border-radius-lg;
			}
		}

		&__no-results {
			text-align: center;
			padding: $spacing-xl;
			color: $color-text-secondary;
		}
	}

	.sortable-header {
		background: none;
		border: none;
		cursor: pointer;
		font-family: inherit;
		font-size: inherit;
		font-weight: inherit;
		color: inherit;
		padding: 0;
		display: flex;
		align-items: center;
		gap: $spacing-xs;

		&:hover {
			color: $color-text-primary;
		}
	}

	.sort-icon {
		font-size: 0.7em;
	}
</style>
