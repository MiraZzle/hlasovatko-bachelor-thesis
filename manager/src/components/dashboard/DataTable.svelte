<script module lang="ts">
	/**
	 * @file Reusable component for displaying a data table.
	 * Renders a table with sortable columns, pagination, and search functionality.
	 */

	// Generic type for items in the table
	export interface ColumnHeader<T> {
		key: keyof T | (string & {});
		label: string;
		sortable?: boolean;
		class?: string;
		render?: (item: T) => string;
	}
</script>

<script lang="ts">
	import Button from '$components/elements/typography/Button.svelte';
	import Input from '$components/elements/typography/Input.svelte';

	// Generic type for items in the table
	type T = $$Generic;

	let {
		items,
		columns,
		title,
		newItemLabel,
		searchPlaceholder,
		noResultsMessage = 'No items found.',
		onNewClick,
		searchTerm = $bindable(''),
		currentPage = $bindable(1),
		pageSize = 10
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
		pageSize?: number;
	}>();

	let sortKey = $state<keyof T | (string & {}) | null>(null);
	let sortDirection = $state<'asc' | 'desc'>('asc');
	const totalPages = $derived(Math.ceil(items.length / pageSize));

	/**
	 * Handles sorting by a specific key.
	 * Sets direction if the same key is clicked, otherwise sets to ascending.
	 * @param key - The key to sort by, must be a valid key of T or a string.
	 */
	function handleSort(key: keyof T | (string & {})): void {
		if (sortKey === key) {
			sortDirection = sortDirection === 'asc' ? 'desc' : 'asc';
		} else {
			sortKey = key;
			sortDirection = 'asc';
		}
	}

	// Pagination handlers
	function handlePreviousPage(): void {
		if (currentPage > 1) currentPage--;
	}

	function handleNextPage(): void {
		if (currentPage < totalPages) currentPage++;
	}

	const paginatedItems = $derived.by(() => {
		if (currentPage > totalPages && totalPages > 0) {
			currentPage = totalPages;
		}

		const startIndex = (currentPage - 1) * pageSize;
		return sortedItems.slice(startIndex, startIndex + pageSize);
	});

	function getValueByPath<T>(obj: T, path: string): any {
		return path.split('.').reduce((o, k) => (o ? o[k] : undefined), obj as any);
	}

	// Dynamic sorting logic
	const sortedItems = $derived.by(() => {
		if (!sortKey) return [...items];

		const sorted = [...items].sort((a, b) => {
			const valA = getValueByPath(a, sortKey as string);
			const valB = getValueByPath(b, sortKey as string);

			// Number comparison
			if (typeof valA === 'number' && typeof valB === 'number') {
				return valA - valB;
			}
			if (typeof valA === 'string' && typeof valB === 'string') {
				// Date comparison
				const dateA = new Date(valA);
				const dateB = new Date(valB);
				if (!isNaN(dateA.getTime()) && !isNaN(dateB.getTime())) {
					return dateA.getTime() - dateB.getTime();
				}
				// String comparison
				return valA.localeCompare(valB);
			}
			// Fallback for other types
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
			<Button variant="primary" onclick={onNewClick}>+ {newItemLabel ?? ''}</Button>
		{/if}
	</header>

	<div class="data-table-page__table-wrapper">
		<table class="data-table">
			<thead>
				<tr>
					{#each columns as column}
						<th class:sortable-header-active={sortKey === column.key}>
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
				{#each paginatedItems as item, i (item.id ?? i)}
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
			max-width: 320px;
			min-width: 250px;

			width: 100%;
			:global(.input-wrapper) {
				width: 100%;
			}
		}

		&__table-wrapper {
			background-color: $color-surface;
			border-radius: $border-radius-lg;
			box-shadow: $box-shadow-sm;
			margin-bottom: $spacing-xl;

			@media screen and (max-width: 768px) {
				overflow-x: auto;
			}
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

		thead th.sortable-header-active {
			font-weight: $font-weight-bold;
			color: $color-text-primary;
		}

		thead th {
			padding: $spacing-md;
			text-align: left;
			border-bottom: $border-width-thin solid $color-border-light;
			white-space: nowrap;
			font-weight: $font-weight-semibold;
			color: $color-text-secondary;

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
