<script lang="ts">
	import Button from '$components/elements/typography/Button.svelte';
	import Input from '$components/elements/typography/Input.svelte';
	import SessionRow from '$components/dashboard/SessionRow.svelte';
	import CreateSessionModal from '$components/elements/modals/CreateSessionModal.svelte';
	import DataTable from '$components/dashboard/DataTable.svelte';
	import type { ColumnHeader } from '$components/dashboard/DataTable.svelte';
	import type { Session } from '$lib/sessions/types';

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

	let searchTerm = $state('');
	let currentPage = $state(1);

	const columns: ColumnHeader<Session>[] = [
		{ key: 'title', label: 'Title', sortable: true },
		{ key: 'created', label: 'Created', sortable: true },
		{ key: 'status', label: 'Status', sortable: true },
		{ key: 'participants', label: 'Participants', sortable: true },
		{ key: 'id', label: 'Actions', sortable: false }
	];

	let sessions = $state<Session[]>([
		{
			id: 's1',
			title: 'Quiz 1 - NDBI046 Spring',
			templateID: `#t41585`,
			created: '2025-04-01T10:00:00Z',
			status: 'Finished',
			participants: 12
		},
		{
			id: 's2',
			title: 'Quiz 1 - NDBI046 Fall',
			templateID: `#t41585`,
			created: '2024-10-15T11:00:00Z',
			status: 'Finished',
			participants: 15
		},
		{
			id: 's3',
			title: 'Poll - Lecture 3 Feedback',
			templateID: `#t41587`,
			created: '2025-04-03T14:30:00Z',
			status: 'Active',
			participants: 28
		},
		{
			id: 's4',
			title: 'Midterm Review Session',
			templateID: `#t41588`,
			created: '2025-04-10T09:00:00Z',
			status: 'Inactive',
			participants: 0
		},
		{
			id: 's5',
			title: 'Quiz 2 - NDBI046 Spring',
			templateID: `#t41586`,
			created: '2025-04-08T10:00:00Z',
			status: 'Finished',
			participants: 11
		}
	]);

	function getFilteredSessions() {
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

	function handleCreateSessionSubmit(data: { templateId: string; title: string }): void {
		console.log('Creating session (from page):', data);
		const newId = `s${Math.random().toString(16).substring(2, 8)}`;
		const template = availableTemplates.find((t) => t.id === data.templateId);
		sessions.push({
			id: newId,
			title: data.title,
			templateID: template ? `#${template.id.substring(1)}` : '#????',
			created: new Date().toISOString(),
			status: 'Inactive',
			participants: 0
		});
		sessions = sessions;
	}

	function createNewSession(): void {
		openCreateSessionModal();
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
		onNewClick={createNewSession}
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
			justify-content: space-between;
			align-items: center;
			margin-bottom: $spacing-xl;
			gap: $spacing-lg;
			flex-wrap: wrap;
		}

		&__search {
			max-width: 400px;
			min-width: 250px;
			position: relative;

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
			flex-wrap: wrap;
			gap: $spacing-md;
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
		font-size: $font-size-sm;

		thead {
			th {
				padding: $spacing-md;
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
