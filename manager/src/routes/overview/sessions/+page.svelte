<script lang="ts">
	import SessionRow from '$components/dashboard/SessionRow.svelte';
	import CreateSessionModal from '$components/elements/modals/CreateSessionModal.svelte';
	import DataTable from '$components/dashboard/DataTable.svelte';
	import type { ColumnHeader } from '$components/dashboard/DataTable.svelte';
	import type { Session } from '$lib/sessions/types';
	import { getAllSessions } from '$lib/sessions/session_utils';
	import type { TemplateStub } from '$lib/templates/types';
	import { getAvailableBaseTemplates } from '$lib/templates/template_utils';
	import { createNewSession } from '$lib/sessions/session_utils';
	import type { SessionMode } from '$lib/shared_types';

	// state management
	let isCreateSessionModalOpen = $state(false);
	let searchTerm = $state('');
	let currentPage = $state(1);

	let availableTemplates = $state<TemplateStub[]>(getAvailableBaseTemplates());
	let sessions = $state<Session[]>(getAllSessions());

	// Define columns for the DataTable
	const columns: ColumnHeader<Session>[] = [
		{ key: 'title', label: 'Title', sortable: true },
		{ key: 'created', label: 'Created', sortable: true },
		{ key: 'status', label: 'Status', sortable: true },
		{ key: 'participants', label: 'Participants', sortable: true },
		{ key: 'id', label: 'Actions', sortable: false }
	];

	// Filters sessions based on search term
	function getFilteredSessions(): Session[] {
		return sessions.filter((session) => {
			const lowerSearch = searchTerm.toLowerCase();
			return (
				session.title.toLowerCase().includes(lowerSearch) ||
				session.templateID.toLowerCase().includes(lowerSearch)
			);
		});
	}

	function openCreateSessionModal(): void {
		isCreateSessionModalOpen = true;
	}

	function closeCreateSessionModal(): void {
		isCreateSessionModalOpen = false;
	}

	async function handleCreateSessionSubmit(
		templateId: string,
		title: string,
		activationDate?: string,
		sessionMode?: SessionMode
	): Promise<void> {
		try {
			const session = await createNewSession(templateId, title, activationDate, sessionMode);
			sessions.push(session);
			sessions = sessions;
		} catch (error) {
			console.error('Session creation failed:', error);
		}
	}

	let filteredSessions = $derived(getFilteredSessions());
</script>

<svelte:head>
	<title>My Sessions - EngaGenie</title>
</svelte:head>

<div class="sessions-overview-page">
	<DataTable
		title="My Sessions"
		searchPlaceholder="Search sessions by title, code..."
		items={filteredSessions}
		{columns}
		noResultsMessage="You haven't run any sessions yet."
		onNewClick={openCreateSessionModal}
		bind:searchTerm
		bind:currentPage
	>
		<svelte:fragment slot="row" let:item>
			<SessionRow session={item} />
		</svelte:fragment>
	</DataTable>

	<CreateSessionModal
		bind:open={isCreateSessionModalOpen}
		templates={availableTemplates}
		onCreate={handleCreateSessionSubmit}
		onclose={closeCreateSessionModal}
	/>
</div>
