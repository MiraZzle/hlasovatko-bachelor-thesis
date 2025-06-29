<script lang="ts">
	/**
	 * @file Sessions Overview Page
	 * This page displays a list of all user sessions with options to create new sessions.
	 */
	import SessionRow from '$components/dashboard/SessionRow.svelte';
	import CreateSessionModal from '$components/elements/modals/CreateSessionModal.svelte';
	import DataTable from '$components/dashboard/DataTable.svelte';
	import type { ColumnHeader } from '$components/dashboard/DataTable.svelte';
	import type { Session } from '$lib/sessions/types';
	import { getAllSessions } from '$lib/sessions/session_utils';
	import type { TemplateStub } from '$lib/templates/types';
	import { createNewSession } from '$lib/sessions/session_utils';
	import type { SessionMode } from '$lib/shared_types';
	import { onMount } from 'svelte';
	import { getAllTemplates } from '$lib/templates/template_utils';
	import { get } from 'svelte/store';
	import { deleteSession } from '$lib/sessions/session_utils';

	// state management
	let isCreateSessionModalOpen = $state(false);
	let searchTerm = $state('');
	let currentPage = $state(1);

	let availableTemplates = $state<TemplateStub[]>([]);
	let sessions = $state<Session[]>([]);
	let isLoading = $state(true);

	// Define columns for the DataTable
	const columns: ColumnHeader<Session>[] = [
		{ key: 'title', label: 'Title', sortable: true },
		{ key: 'created', label: 'Created', sortable: true },
		{ key: 'status', label: 'Status', sortable: true },
		{ key: 'participants', label: 'Participants', sortable: true },
		{ key: 'id', label: 'Actions', sortable: false }
	];

	// Fetch data when the component mounts
	onMount(async () => {
		isLoading = true;
		const [templatesData, sessionsData] = await Promise.all([getAllTemplates(), getAllSessions()]);

		// Map the full Template objects to TemplateStub objects for the modal
		availableTemplates = templatesData.map((t) => ({
			id: t.id,
			title: t.settings?.title ?? 'Untitled Template'
		}));

		sessions = sessionsData;
		isLoading = false;
	});

	// Filters sessions based on search term
	function getFilteredSessions(): Session[] {
		return sessions.filter((session) => {
			const lowerSearch = searchTerm.toLowerCase();
			return (
				session.title.toLowerCase().includes(lowerSearch) ||
				session.joinCode?.toLowerCase().includes(lowerSearch) ||
				session.status.toLowerCase().includes(lowerSearch)
			);
		});
	}

	function openCreateSessionModal(): void {
		isCreateSessionModalOpen = true;
	}

	function closeCreateSessionModal(): void {
		isCreateSessionModalOpen = false;
	}

	async function handleDeleteSession(sessionId: string): Promise<void> {
		let deleteSuccesful = await deleteSession(sessionId);
		if (deleteSuccesful) {
			sessions = sessions.filter((session) => session.id !== sessionId);
		} else {
			console.error('Failed to delete session with ID:', sessionId);
		}
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
	<title>EngaGenie | My Sessions</title>
</svelte:head>

<div class="sessions-overview-page">
	<DataTable
		title="My Sessions"
		searchPlaceholder="Search sessions by title, code, status..."
		items={filteredSessions}
		{columns}
		noResultsMessage="You haven't run any sessions yet."
		onNewClick={openCreateSessionModal}
		bind:searchTerm
		bind:currentPage
		newItemLabel="Add new Session"
	>
		<svelte:fragment slot="row" let:item>
			<SessionRow session={item} onDelete={() => handleDeleteSession(item.id)} />
		</svelte:fragment>
	</DataTable>

	<CreateSessionModal
		bind:open={isCreateSessionModalOpen}
		templates={availableTemplates}
		onCreate={handleCreateSessionSubmit}
		onclose={closeCreateSessionModal}
	/>
</div>
