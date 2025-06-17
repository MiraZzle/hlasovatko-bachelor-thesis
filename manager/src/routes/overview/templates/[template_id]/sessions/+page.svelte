<script lang="ts">
	// --- Imports ---
	import Button from '$components/elements/typography/Button.svelte'; // Verify path
	import Input from '$components/elements/typography/Input.svelte'; // Verify path
	import SessionRow from '$components/dashboard/SessionRow.svelte'; // Reuse SessionRow
	import CreateSessionModal from '$components/elements/modals/CreateSessionModal.svelte';
	import DataTable from '$components/dashboard/DataTable.svelte';
	import type { ColumnHeader } from '$components/dashboard/DataTable.svelte';
	import { page } from '$app/state';

	// Define Session type (or import from global types)
	interface Session {
		id: string;
		title: string; // Title of the session (might be same as template or custom)
		templateCode: string; // Which template was used
		created: string; // Date/time session was created/started
		status: 'Active' | 'Inactive' | 'Finished'; // Session status
		participants: number; // Number of participants
	}

	const templateId = $derived(page.params.template_id);

	let isCreateSessionModalOpen = $state(false);

	interface TemplateStub {
		id: string;
		title: string;
	}

	let availableTemplates = $state<TemplateStub[]>([
		{ id: 't41585', title: 'Quiz 1' },
		{ id: 't41586', title: 'Quiz 2' },
		{ id: 't41587', title: 'Poll - Lecture 3' },
		{ id: 't41588', title: 'Midterm Review' }
	]);

	// --- State ---
	let searchTerm = $state('');
	let currentPage = $state(1);

	const columns: ColumnHeader<Session>[] = [
		{ key: 'title', label: 'Title', sortable: true },
		{ key: 'created', label: 'Created', sortable: true },
		{ key: 'status', label: 'Status', sortable: true },
		{ key: 'participants', label: 'Participants', sortable: true },
		{ key: 'id', label: 'Actions', sortable: false } // Using 'id' as a stable key for the actions column
	];

	let sessions = $state<Session[]>([
		{
			id: 's1',
			title: 'Quiz 1 - NDBI046 Spring',
			templateCode: `#t41585`,
			created: '2025-04-01T10:00:00Z',
			status: 'Finished',
			participants: 12
		},
		{
			id: 's2',
			title: 'Quiz 1 - NDBI046 Fall',
			templateCode: `#t41585`,
			created: '2024-10-15T11:00:00Z',
			status: 'Finished',
			participants: 15
		},
		{
			id: 's3',
			title: 'Poll - Lecture 3 Feedback',
			templateCode: `#t41587`,
			created: '2025-04-03T14:30:00Z',
			status: 'Active',
			participants: 28
		},
		{
			id: 's4',
			title: 'Midterm Review Session',
			templateCode: `#t41588`,
			created: '2025-04-10T09:00:00Z',
			status: 'Inactive',
			participants: 0
		},
		{
			id: 's5',
			title: 'Quiz 2 - NDBI046 Spring',
			templateCode: `#t41586`,
			created: '2025-04-08T10:00:00Z',
			status: 'Finished',
			participants: 11
		}
	]);

	// --- Handlers ---
	function handleSearch() {
		// Implement search logic if needed beyond filtering
		console.log('Searching sessions for:', searchTerm);
	}
	function handleAction(sessionId: string) {
		// Actions for a specific session (e.g., view details, results, delete)
		console.log('Session action for:', sessionId);
		// goto(`/sessions/${sessionId}/results`); // Example navigation
	}
	function handlePreviousPage() {
		if (currentPage > 1) currentPage--;
		// Fetch previous page data
	}
	function handleNextPage() {
		currentPage++; // Add total pages check
		// Fetch next page data
	}

	function getFilteredSessions() {
		return sessions.filter((session) => {
			const lowerSearch = searchTerm.toLowerCase();
			return (
				session.title.toLowerCase().includes(lowerSearch) ||
				session.templateCode.toLowerCase().includes(lowerSearch)
			);
		});
	}

	// --- Modal Handlers ---
	function openCreateSessionModal(): void {
		isCreateSessionModalOpen = true;
	}

	function closeCreateSessionModal(): void {
		isCreateSessionModalOpen = false;
	}

	// This function receives data from the modal's onCreate prop
	function handleCreateSessionSubmit(data: { templateId: string; title: string }): void {
		console.log('Creating session (from page):', data);
		// --- TODO: Call API to create session ---

		// Example: Add to dummy session list
		const newId = `s${Math.random().toString(16).substring(2, 8)}`;
		const template = availableTemplates.find((t) => t.id === data.templateId);
		sessions.push({
			id: newId,
			title: data.title,
			templateCode: template ? `#${template.id.substring(1)}` : '#????',
			created: new Date().toISOString(),
			status: 'Inactive', // Or 'Active' if starting immediately?
			participants: 0
		});
		sessions = sessions; // Trigger reactivity

		// Modal closes itself via onclose binding / callback
	}

	// Connect the button handler to open the modal
	function createNewSession(): void {
		openCreateSessionModal();
	}

	// --- Filtering ---
	let filteredSessions = $derived(getFilteredSessions());
</script>

<svelte:head>
	<title>EngaGenie | Template {templateId} - Sessions</title>
</svelte:head>

<div class="sessions-overview-page">
	<DataTable
		title="Session for Template {templateId}"
		searchPlaceholder="Search sessions by title, code..."
		items={filteredSessions}
		{columns}
		noResultsMessage="You haven't run any sessions yet."
		bind:searchTerm
		bind:currentPage
	>
		<svelte:fragment slot="row" let:item>
			<SessionRow session={item} />
		</svelte:fragment>
	</DataTable>
</div>

<style lang="scss">
	.sessions-overview-page {
		&__title {
			font-size: $font-size-3xl;
			font-weight: $font-weight-bold;
			margin: 0;
			flex-grow: 1;
		}

		&__header {
			display: flex;
			justify-content: space-between; // Keep space-between
			align-items: center;
			margin-bottom: $spacing-xl;
			gap: $spacing-lg;
			flex-wrap: wrap;
		}

		// Element: Search within Header
		&__search {
			// Removed flex-grow to allow title and button space
			max-width: 400px;
			min-width: 250px; // Ensure it doesn't get too small
			position: relative;

			:global(.input-wrapper) {
				width: 100%;
			}
		}

		// Element: Table Wrapper
		&__table-wrapper {
			background-color: $color-surface;
			border-radius: $border-radius-lg;
			box-shadow: $box-shadow-sm;
			overflow-x: auto;
			margin-bottom: $spacing-xl;
		}

		// Element: Pagination Footer
		&__pagination {
			display: flex;
			justify-content: space-between;
			align-items: center;
			flex-wrap: wrap;
			gap: $spacing-md;
			padding: $spacing-sm;
			font-size: $font-size-sm;
			color: $color-text-secondary;
		}

		// Element: Pagination Controls within Footer
		&__pagination-controls {
			display: flex;
			gap: $spacing-sm;
		}
	}

	// Block: data-table (Styles for overall table structure and header)
	.data-table {
		width: 100%;
		border-collapse: collapse;
		font-size: $font-size-sm;

		thead {
			th {
				padding: $spacing-md; // 8px horizontal padding
				text-align: left;
				border-bottom: $border-width-thin solid $color-border-light;
				white-space: nowrap;
				font-weight: $font-weight-semibold;
				color: $color-text-secondary;
				background-color: $color-surface-alt;

				&:first-child {
					border-top-left-radius: $border-radius-lg;
				}
				&:last-child {
					border-top-right-radius: $border-radius-lg;
				}
			}
		}

		&__no-results {
			text-align: center;
			padding: $spacing-xl;
			color: $color-text-secondary;
		}
	}
</style>
