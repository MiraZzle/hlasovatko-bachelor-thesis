<script lang="ts">
	import TemplateRow from '$components/dashboard/TemplateRow.svelte';
	import CreateTemplateModal from '$components/elements/modals/CreateTemplateModal.svelte';
	import DataTable from '$components/dashboard/DataTable.svelte';
	import type { ColumnHeader } from '$components/dashboard/DataTable.svelte';
	import { getAllTemplates } from '$lib/templates/template_utils';
	import type { Template } from '$lib/templates/types';
	import { createNewTemplate } from '$lib/templates/template_utils';

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

	async function handleCreateTemplateSubmit(data: {
		name: string;
		deriveFromId: string;
	}): Promise<void> {
		console.log('Creating template (from page):', data);
		const newTemplate: Template = await createNewTemplate(data.name, data.deriveFromId);
		templates.push(newTemplate);
		templates = templates;
	}

	function handleCreateNewTemplate(): void {
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
		onNewClick={handleCreateNewTemplate}
		newItemLabel="Add new Template"
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
