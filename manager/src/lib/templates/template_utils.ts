import type { Template, TemplateSettingsDTO } from '$lib/templates/types';

export function createNewTemplate() {}

export function getTemplateById() {}

export function getAvailableBaseTemplates() {
	return [
		{ id: 't41585', title: 'Quiz 1' },
		{ id: 't41586', title: 'Quiz 2' },
		{ id: 't41587', title: 'Poll' },
		{ id: 't41588', title: 'Midterm Review' }
	];
}

export function getAllTemplates(): Template[] {
	return [
		{
			id: 't41585',
			title: 'Weekly Team Retrospective',
			definition: {},
			ownerId: 'user_alpha_123',
			version: 1.0,
			tags: ['meeting', 'agile', 'team-sync'],
			dateCreated: '2025-06-15T10:00:00Z'
		},
		{
			id: 't41586',
			title: 'New Feature Brainstorm',
			definition: {},
			ownerId: 'user_beta_456',
			version: 1.0,
			tags: ['ideation', 'product', 'creative'],
			dateCreated: '2025-06-16T11:30:00Z'
		},
		{
			id: 't41587',
			title: 'Daily Stand-up',
			definition: {},
			ownerId: 'user_alpha_123',
			version: 1.0,
			tags: ['daily', 'check-in', 'agile'],
			dateCreated: '2025-06-17T09:00:00Z'
		},
		{
			id: 't41588',
			title: 'Quarterly Planning Workshop',
			definition: {},
			ownerId: 'user_gamma_789',
			version: 1.0,
			tags: ['planning', 'strategy', 'workshop'],
			dateCreated: '2025-05-20T14:00:00Z'
		},
		{
			id: 't41589',
			title: 'User Feedback Review',
			definition: {},
			ownerId: 'user_beta_456',
			version: 1.0,
			tags: ['ux', 'feedback', 'research'],
			dateCreated: '2025-06-01T16:45:00Z'
		}
	];
}

export function getTemplateSettingsById(templateId: string): Promise<TemplateSettingsDTO> {
	// Simulate fetching settings for a specific template
	console.log(`Fetching settings for template ${templateId}...`);

	return new Promise((resolve) => {
		setTimeout(() => {
			const settings: TemplateSettingsDTO = {
				sessionPacing: 'teacher-paced',
				resultsVisibleDefault: true,
				title: `Template ${templateId}`,
				tags: ['Example', 'Physics 101']
			};
			resolve(settings);
		}, 500);
	});
}

export function updateTemplateSettings(
	templateId: string,
	settings: TemplateSettingsDTO
): Promise<boolean> {
	console.log(`Updating settings for template ${templateId}...`, settings);

	return new Promise((resolve) => {
		setTimeout(() => {
			console.log(`Settings for template ${templateId} updated successfully.`);
			resolve(true);
		}, 100);
	});
}
