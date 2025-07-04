<script lang="ts">
	/**
	 * @file Template Sessions Page
	 * This page displays all sessions associated with a specific template.
	 */
	import SessionRow from '$components/dashboard/SessionRow.svelte';
	import DataTable from '$components/dashboard/DataTable.svelte';
	import type { ColumnHeader } from '$components/dashboard/DataTable.svelte';
	import { page } from '$app/state';
	import { getSessionsByTemplate } from '$lib/sessions/session_utils';
	import type { Session } from '$lib/sessions/types';
	import { onMount } from 'svelte';
	import { deleteSession } from '$lib/sessions/session_utils';

	const templateId = $derived(page.params.template_id);

	// state management
	let searchTerm = $state('');
	let currentPage = $state(1);

	// Define columns for data table
	const columns: ColumnHeader<Session>[] = [
		{ key: 'title', label: 'Title', sortable: true },
		{ key: 'created', label: 'Created', sortable: true },
		{ key: 'status', label: 'Status', sortable: true },
		{ key: 'participants', label: 'Participants', sortable: true },
		{ key: 'id', label: 'Actions', sortable: false }
	];

	let sessionsForTemplate = $state<Session[]>([]);

	// Fetch sessions for the template when the component mounts
	onMount(async () => {
		sessionsForTemplate = await getSessionsByTemplate(templateId);
	});

	/**
	Filter sessions based on search term.
	Search is case-insensitive and checks both title and id.
	@returns {Session[]} Filtered list of sessions
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

	async function handleDeleteSession(sessionId: string): Promise<void> {
		let deleteSuccesful = await deleteSession(sessionId);
		if (deleteSuccesful) {
			sessionsForTemplate = sessionsForTemplate.filter((session) => session.id !== sessionId);
		} else {
			console.error('Failed to delete session with ID:', sessionId);
		}
	}

	let filteredSessions = $derived(getFilteredSessions());
</script>

<svelte:head>
	<title>EngaGenie | Template {templateId} - Sessions</title>
</svelte:head>

<div class="sessions-overview-page">
	<DataTable
		title="Sessions based on this template"
		searchPlaceholder="Search sessions by title, code..."
		items={filteredSessions}
		{columns}
		noResultsMessage="You haven't run any sessions yet."
		bind:searchTerm
		bind:currentPage
	>
		<svelte:fragment slot="row" let:item>
			<SessionRow session={item} onDelete={() => handleDeleteSession(item.id)} />
		</svelte:fragment>
	</DataTable>
</div>
