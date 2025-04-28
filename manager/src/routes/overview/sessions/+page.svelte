<script lang="ts">
	// --- Imports ---
	import Button from '$components/elements/typography/Button.svelte'; // Verify path
	import Input from '$components/elements/typography/Input.svelte'; // Verify path
	import SessionRow from '$components/dashboard/SessionRow.svelte'; // Reuse SessionRow

	// Define Session type (or import from global types)
	interface Session {
		id: string;
		title: string; // Title of the session (might be same as template or custom)
		templateCode: string; // Which template was used
		created: string; // Date/time session was created/started
		status: 'Active' | 'Inactive' | 'Finished'; // Session status
		participants: number; // Number of participants
	}

	// --- State ---
	let searchTerm = $state('');
	let currentPage = $state(1);
	// Dummy data for ALL sessions (replace with actual fetch)
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
	function createNewSession() {
		// Logic to start a new session (might involve selecting a template first)
		console.log('Create new session clicked');
		// Potentially open a modal or navigate to a "new session" flow
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

	// --- Filtering ---
	let filteredSessions = $derived(getFilteredSessions());
</script>

<svelte:head>
	<title>My Sessions - EngaGenie</title>
</svelte:head>

<div class="sessions-overview-page">
	<header class="sessions-overview-page__header">
		<h1 class="sessions-overview-page__title">My Sessions</h1>
		<div class="sessions-overview-page__search">
			<Input
				placeholder="Search sessions by title, code..."
				ariaLabel="Search sessions"
				bind:value={searchTerm}
				oninput={handleSearch}
			/>
		</div>
		<Button variant="primary" onclick={createNewSession}>+ New Session</Button>
	</header>

	<div class="sessions-overview-page__table-wrapper">
		<table class="data-table">
			<thead>
				<tr>
					<th>Title</th>
					<th>Created</th>
					<th>Status</th>
					<th>Participants</th>
					<th>Actions</th>
				</tr>
			</thead>
			<tbody>
				{#each filteredSessions as session}
					<SessionRow {session} onActionClick={handleAction} />
				{:else}
					<tr>
						<td colspan="5" class="data-table__no-results"> You haven't run any sessions yet. </td>
					</tr>
				{/each}
			</tbody>
		</table>
	</div>

	<footer class="sessions-overview-page__pagination">
		<span>Page {currentPage}</span>
		<div class="sessions-overview-page__pagination-controls">
			<Button variant="outline" onclick={handlePreviousPage} disabled={currentPage <= 1}>
				&larr; Previous
			</Button>
			<Button variant="outline" onclick={handleNextPage}>Next &rarr;</Button>
		</div>
	</footer>
</div>

<style lang="scss">
	@import '../../../styles/variables.scss'; // Adjust path if needed

	// Block: sessions-overview-page
	.sessions-overview-page {
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

	// Reuse data-table styles (ensure they are globally available or copy here)
	// --- PASTE .data-table SCSS block here if not globally available ---
	// .data-table { ... all nested styles ... }
	// Ensure SessionRow status styles (e.g., .data-table__status--finished) are included
</style>
