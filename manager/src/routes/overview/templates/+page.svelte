<script lang="ts">
	import Button from '$components/elements/typography/Button.svelte';
	import Input from '$components/elements/typography/Input.svelte';
	import TemplateRow from '$components/dashboard/TemplateRow.svelte';

	import ModalDialog from '$components/elements/ModalDialog.svelte';
	import Select from '$components/elements/typography/Select.svelte'; // Assuming Select is in elements
	import CreateTemplateModal from '$components/elements/CreateTemplateModal.svelte';
	import DataTable from '$components/dashboard/DataTable.svelte';
	import type { ColumnHeader } from '$components/dashboard/DataTable.svelte'; // Assuming ColumnHeader is exported as a type

	interface Template {
		id: string;
		title: string;
		code: string;
		dateCreated: string;
		status: 'Active' | 'Inactive';
		tags: string[];
	}

	// --- State ---
	let searchTerm = $state('');
	let currentPage = $state(1);
	let totalPages = $state(1);
	let templates = $state<Template[]>([
		// Dummy data...
		{
			id: 't41585',
			title: 'Quiz 1',
			code: '#41585',
			dateCreated: '2025-04-01',
			status: 'Inactive',
			tags: ['ndbi046']
		},
		{
			id: 't41586',
			title: 'Quiz 2',
			code: '#41586',
			dateCreated: '2025-04-01',
			status: 'Inactive',
			tags: ['ndbi046']
		},
		{
			id: 't41587',
			title: 'Poll - Lecture 3',
			code: '#41587',
			dateCreated: '2025-04-03',
			status: 'Active',
			tags: ['lecture3', 'feedback']
		},
		{
			id: 't41588',
			title: 'Midterm Review',
			code: '#41588',
			dateCreated: '2025-04-10',
			status: 'Inactive',
			tags: ['ndbi046', 'review']
		}
	]);

	// --- Column Definitions for the DataTable ---
	const columns: ColumnHeader<Template>[] = [
		{ key: 'title', label: 'Title', sortable: true },
		{ key: 'dateCreated', label: 'Date created', sortable: true },
		{ key: 'status', label: 'Status', sortable: true },
		{ key: 'tags', label: 'Tags', sortable: false }, // Tags array is not easily sortable
		{ key: 'id', label: 'Actions', sortable: false } // 'id' is a placeholder key for the actions column
	];

	// --- NEW State for Modal ---
	let isCreateModalOpen = $state(false);
	let newTemplateName = $state('');
	let deriveFromTemplateId = $state<string>('none'); // Default to 'none'

	const deriveOptions = $derived(() => {
		const options = [{ value: 'none', label: 'None' }];
		// In real app, map existing templates:
		// templates.forEach(t => options.push({ value: t.id, label: t.title }));
		return options;
	});

	// --- Dummy Handlers ---
	function handleSearch() {
		/* ... */
	}
	function handleAction(templateId: string) {
		/* ... */
	}
	function handlePreviousPage() {
		/* ... */
	}
	function handleNextPage() {
		/* ... */
	}

	// --- NEW Modal Handlers ---
	function openCreateModal(): void {
		newTemplateName = ''; // Reset form
		deriveFromTemplateId = 'none'; // Reset form
		isCreateModalOpen = true;
	}

	function closeCreateModal(): void {
		isCreateModalOpen = false;
	}

	function handleCreateSubmit(): void {
		if (!newTemplateName.trim()) {
			alert('Please enter a template name.'); // Basic validation
			return;
		}
		console.log('Creating template:', {
			name: newTemplateName,
			deriveFrom: deriveFromTemplateId
		});
		// --- TODO: Add actual API call here ---
		// Example: Add to dummy data
		const newId = `t${Math.random().toString(16).substring(2, 8)}`;
		templates.push({
			id: newId,
			title: newTemplateName,
			code: `#${newId.substring(1)}`,
			dateCreated: new Date().toISOString(),
			status: 'Inactive',
			tags: []
		});
		templates = templates; // Trigger reactivity if needed for non-rune state

		closeCreateModal(); // Close modal after submission
	}

	// --- Handler for when modal confirms creation ---
	// This function now receives data from the modal component
	function handleCreateTemplateSubmit(data: { name: string; deriveFromId: string }): void {
		console.log('Creating template (from page):', data);

		// --- TODO: Add actual API call here ---

		// Example: Add to dummy data
		const newId = `t${Math.random().toString(16).substring(2, 8)}`;
		templates.push({
			id: newId,
			title: data.name,
			code: `#${newId.substring(1)}`,
			dateCreated: new Date().toISOString(),
			status: 'Inactive',
			tags: []
		});
		templates = templates; // Trigger reactivity

		// Modal closes itself internally after calling this,
		// but we already set isCreateModalOpen = false via the onclose binding.
		// No need to call closeCreateModal() here explicitly if using onclose prop.
	}

	// --- Update original createNewTemplate handler ---
	function createNewTemplate(): void {
		openCreateModal();
	}

	function getFilteredTemplates() {
		if (!searchTerm.trim()) {
			return templates;
		}
		const lowerSearch = searchTerm.toLowerCase();
		return templates.filter(
			(t) =>
				t.title.toLowerCase().includes(lowerSearch) ||
				t.code.toLowerCase().includes(lowerSearch) ||
				t.tags.some((tag) => tag.toLowerCase().includes(lowerSearch))
		);
	}

	// --- Computed property for filtering ---
	// Add explicit return type <Template[]> to $derived
	let filteredTemplates = $derived(getFilteredTemplates());
</script>

<svelte:head>
	<title>My Templates - EngaGenie</title>
</svelte:head>

<div class="templates-page">
	<DataTable
		title="My Templates"
		searchPlaceholder="Search Templates by title, type, code"
		items={filteredTemplates}
		{columns}
		noResultsMessage="No templates found."
		onNewClick={createNewTemplate}
		bind:searchTerm
		bind:currentPage
		{totalPages}
	>
		<svelte:fragment slot="row" let:item>
			<TemplateRow template={item} onActionClick={handleAction} />
		</svelte:fragment>
	</DataTable>

	<CreateTemplateModal
		bind:open={isCreateModalOpen}
		{templates}
		onCreate={handleCreateTemplateSubmit}
		onclose={closeCreateModal}
	/>
</div>

<style lang="scss">
	@import '../../../styles/variables.scss';

	// Block: templates-page (Page Root)
	.templates-page {
		&__title {
			// Added Title style
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			margin: 0; // Remove default margin
			// Ensure it doesn't cause wrapping issues with search/button
			flex-grow: 1; // Allow title to take space if header is flex
		}
		// Element: Header Section
		&__header {
			display: flex;
			justify-content: space-between;
			align-items: center;
			margin-bottom: $spacing-xl;
			gap: $spacing-lg;
			flex-wrap: wrap; // Allow wrapping on smaller screens
		}

		// Element: Search within Header
		&__search {
			flex-grow: 1;
			max-width: 400px;
			position: relative;

			// Styling the Input component wrapper (May still need :global for component's root)
			// Adjust if Input component has a specific wrapper class we can target instead.
			:global(.input-wrapper) {
				width: 100%; // Ensure the input wrapper takes space if needed
			}
			// If Input component itself has class="input-element" on its root:
			// .input-element { /* styles */ }
		}

		// Element: Table Wrapper
		&__table-wrapper {
			background-color: $color-surface;
			border-radius: $border-radius-lg;
			box-shadow: $box-shadow-sm;
			overflow-x: auto; // Allow horizontal scroll on small screens
			margin-bottom: $spacing-xl; // Space before pagination
		}

		// Element: Pagination Footer
		&__pagination {
			display: flex;
			justify-content: space-between;
			align-items: center;
			flex-wrap: wrap; // Allow wrapping if needed
			gap: $spacing-md;
			padding: $spacing-sm; // Add some padding around pagination
			font-size: $font-size-sm;
			color: $color-text-secondary;
		}

		// Element: Pagination Controls within Footer
		&__pagination-controls {
			display: flex;
			gap: $spacing-sm;
		}
	}

	// Block: data-table (Reusable Table Styles)
	.data-table {
		width: 100%;
		border-collapse: collapse;
		font-size: $font-size-sm;

		// Style table head section
		thead {
			th {
				padding: $spacing-md;
				text-align: left;
				border-bottom: $border-width-thin solid $color-border-light;
				white-space: nowrap;
				font-weight: $font-weight-semibold;
				color: $color-text-secondary;
				background-color: $color-surface-alt; // Slight background for header

				// Apply border radius to first/last header cells
				&:first-child {
					border-top-left-radius: $border-radius-lg;
				}
				&:last-child {
					border-top-right-radius: $border-radius-lg;
				}
			}
		}

		// Style table body section
		tbody {
			// Styles for direct children (td) can be nested or defined separately
			td {
				padding: $spacing-md;
				text-align: left;
				border-bottom: $border-width-thin solid $color-border-light;
				white-space: nowrap; // Avoid wrapping cell content
				color: $color-text-primary;
				vertical-align: middle;
			}

			// Styling for rows if needed (e.g., hover effect)
			// tr {
			//     &:hover { background-color: $color-surface-alt; }
			// }
		}

		// Elements rendered inside TemplateRow, styled via this block
		// No :global() needed assuming TemplateRow applies these classes.

		// Element: Primary content in a cell
		&__cell-primary {
			font-weight: $font-weight-medium;
			color: $color-text-primary;
		}

		// Element: Secondary content in a cell
		&__cell-secondary {
			font-size: $font-size-xs;
			color: $color-text-secondary;
			margin-top: $spacing-xs * 0.5; // Small space below primary content
		}

		// Element: Container for tags
		&__tags {
			display: flex;
			flex-wrap: wrap;
			gap: $spacing-xs;
			max-width: 250px; // Limit width and allow wrapping for many tags
			white-space: normal; // Allow tags container itself to wrap if needed
		}

		// Element: Individual tag
		&__tag {
			background-color: $color-surface-alt;
			color: $color-text-secondary;
			padding: $spacing-xs $spacing-sm;
			border-radius: $border-radius-pill;
			font-size: $font-size-xs;
			font-weight: $font-weight-medium;
			white-space: nowrap; // Keep individual tags from wrapping
		}

		// Element: Status Indicator
		&__status {
			display: inline-block; // Allows padding/background
			padding: $spacing-xs $spacing-sm;
			border-radius: $border-radius-pill;
			font-size: $font-size-xs;
			font-weight: $font-weight-medium;
			text-transform: uppercase;
			white-space: nowrap;

			// Modifier: Inactive status
			&--inactive {
				background-color: $color-surface-alt;
				color: $color-text-secondary;
			}
			// Modifier: Active status
			&--active {
				// Use a success color, slightly muted
				background-color: rgba($color-success, 0.15);
				color: darken($color-success, 10%);
			}
			// Add other statuses as needed
		}

		// Element: Action button (ellipsis)
		&__action-button {
			background: none;
			border: none;
			padding: $spacing-xs;
			cursor: pointer;
			color: $color-text-secondary;
			border-radius: $border-radius-circle;
			display: inline-flex; // Ensures proper alignment/sizing of icon
			line-height: 0; // Prevent extra space around icon

			// Nested SVG styling
			svg {
				width: 18px;
				height: 18px;
				stroke-width: 1.5; // Make icon slightly bolder if needed
			}

			// Hover state modifier (using nesting)
			&:hover {
				background-color: $color-surface-alt;
				color: $color-text-primary;
			}
			&:focus-visible {
				outline: 2px solid $color-primary-light;
				outline-offset: 1px;
			}
		}

		// Element: Row shown when no results
		&__no-results {
			// This applies to the TD itself when rendered
			text-align: center;
			padding: $spacing-xl; // Generous padding
			color: $color-text-secondary;
		}
	}
</style>
