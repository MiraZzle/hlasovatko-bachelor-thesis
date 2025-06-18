<script lang="ts">
	import TemplateRow from '$components/dashboard/TemplateRow.svelte';
	import CreateTemplateModal from '$components/elements/modals/CreateTemplateModal.svelte';
	import DataTable from '$components/dashboard/DataTable.svelte';
	import type { ColumnHeader } from '$components/dashboard/DataTable.svelte';
	import { getAllTemplates } from '$lib/templates/template_utils';
	import type { Template } from '$lib/templates/types';

	// state management
	let searchTerm = $state('');
	let currentPage = $state(1);
	let templates = $state<Template[]>(getAllTemplates());

	// define columns for datatable
	const columns: ColumnHeader<Template>[] = [
		{ key: 'settings.title', label: 'Title', sortable: true },
		{ key: 'dateCreated', label: 'Date created', sortable: true },
		{ key: 'settings.tags', label: 'Tags', sortable: false },
		{ key: 'id', label: 'Actions', sortable: false }
	];

	let isCreateModalOpen = $state(false);
	let newTemplateName = $state('');
	let deriveFromTemplateId = $state<string>('none');

	function openCreateModal(): void {
		newTemplateName = '';
		deriveFromTemplateId = 'none';
		isCreateModalOpen = true;
	}

	function closeCreateModal(): void {
		isCreateModalOpen = false;
	}

	function handleCreateTemplateSubmit(data: { name: string; deriveFromId: string }): void {
		console.log('Creating template (from page):', data);
		const newId = `t${Math.random().toString(16).substring(2, 8)}`;
		const newTemplate: Template = {
			id: 't41589',
			definition: [],
			ownerId: 'user_beta_456',
			version: 1.0,
			dateCreated: '2025-06-01T16:45:00Z',
			settings: {
				title: 'New Template',
				tags: ['ux', 'feedback', 'research'],
				sessionPacing: 'teacher-paced',
				resultsVisibleDefault: true
			}
		};
		templates.push(newTemplate);
		templates = templates;
	}

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
				t.settings!.title.toLowerCase().includes(lowerSearch) ||
				t.id.toLowerCase().includes(lowerSearch) ||
				t.settings!.tags?.some((tag) => tag.toLowerCase().includes(lowerSearch))
		);
	}

	let filteredTemplates = $derived(getFilteredTemplates());
</script>

<svelte:head>
	<title>EngaGenie | My Templates</title>
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
	>
		<svelte:fragment slot="row" let:item>
			<TemplateRow template={item} />
		</svelte:fragment>
	</DataTable>

	<CreateTemplateModal
		bind:open={isCreateModalOpen}
		{templates}
		onCreate={handleCreateTemplateSubmit}
		onclose={closeCreateModal}
	/>
</div>
