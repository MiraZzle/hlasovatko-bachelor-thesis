<script lang="ts">
	/**
	 * @file My Templates Page
	 * This page displays all templates created by the user and allows them to create new templates.
	 */
	import TemplateRow from '$components/dashboard/TemplateRow.svelte';
	import CreateTemplateModal from '$components/elements/modals/CreateTemplateModal.svelte';
	import DataTable from '$components/dashboard/DataTable.svelte';
	import type { ColumnHeader } from '$components/dashboard/DataTable.svelte';
	import { getAllTemplates } from '$lib/templates/template_utils';
	import type { Template } from '$lib/templates/types';
	import { createNewTemplate } from '$lib/templates/template_utils';
	import { onMount } from 'svelte';
	import { deleteTemplate } from '$lib/templates/template_utils';

	// state management
	let searchTerm = $state('');
	let currentPage = $state(1);
	let templates = $state<Template[]>([]);
	let isLoading = $state(true);

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

	onMount(async () => {
		isLoading = true;
		templates = await getAllTemplates();
		isLoading = false;
	});

	function openCreateModal(): void {
		newTemplateName = '';
		deriveFromTemplateId = 'none';
		isCreateModalOpen = true;
	}

	function closeCreateModal(): void {
		isCreateModalOpen = false;
	}

	async function handleCreateTemplateSubmit(data: { name: string }): Promise<void> {
		console.log('Creating template (from page):', data);
		const newTemplate = await createNewTemplate(
			{
				title: data.name,
				tags: [],
				sessionPacing: 'teacher-paced',
				resultsVisibleDefault: true
			},
			[]
		);

		if (newTemplate) {
			templates.push(newTemplate);
		} else {
			alert('Failed to create template.');
		}
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

	async function handleDeleteTemplate(templateId: string): Promise<void> {
		const success = await deleteTemplate(templateId);
		if (success) {
			templates = templates.filter((t) => t.id !== templateId);
		} else {
			alert('Failed to delete template.');
		}
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
			<TemplateRow template={item} onDelete={() => handleDeleteTemplate(item.id)} />
		</svelte:fragment>
	</DataTable>

	<CreateTemplateModal
		bind:open={isCreateModalOpen}
		{templates}
		onCreate={handleCreateTemplateSubmit}
		onclose={closeCreateModal}
	/>
</div>
