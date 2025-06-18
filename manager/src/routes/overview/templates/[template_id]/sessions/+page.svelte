<script lang="ts">
	import SessionRow from '$components/dashboard/SessionRow.svelte';
	import DataTable from '$components/dashboard/DataTable.svelte';
	import type { ColumnHeader } from '$components/dashboard/DataTable.svelte';
	import { page } from '$app/state';
	import { getSessionsByTemplate } from '$lib/sessions/session_utils';
	import type { Session } from '$lib/sessions/types';

	const templateId = $derived(page.params.template_id);

	// state management
	let searchTerm = $state('');
	let currentPage = $state(1);

	// define columns for data table
	const columns: ColumnHeader<Session>[] = [
		{ key: 'title', label: 'Title', sortable: true },
		{ key: 'created', label: 'Created', sortable: true },
		{ key: 'status', label: 'Status', sortable: true },
		{ key: 'participants', label: 'Participants', sortable: true },
		{ key: 'id', label: 'Actions', sortable: false }
	];

	let sessionsForTemplate = $derived<Session[]>(getSessionsByTemplate(templateId));

	/*
	Filter sessions based on search term.
	Search is case-insensitive and checks both title and id.
	*/
	function getFilteredSessions(): Session[] {
		return sessionsForTemplate.filter((session) => {
			const lowerSearch = searchTerm.toLowerCase();
			return (
				session.title.toLowerCase().includes(lowerSearch) ||
				session.id.toLowerCase().includes(lowerSearch)
			);
		});
	}

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
