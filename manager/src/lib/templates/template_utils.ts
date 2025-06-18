import type { Template, TemplateSettingsDTO } from '$lib/templates/types';

export function createNewTemplate() {}

export function getTemplateById(templateId: string): Template {
	console.log(`Fetching template with ID: ${templateId}`);
	return {
		id: 'template_b7c2f8e1-a9d5-4b8e-8e4f-1a2b3c4d5e6f',
		ownerId: 'user_a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d',
		version: 1,
		dateCreated: '2025-06-18T08:45:00.000Z',
		settings: {
			title: 'Live Mid-Term Review Session',
			sessionPacing: 'student-paced',
			resultsVisibleDefault: true,
			tags: ['Interactive', 'Review']
		},
		definition: [
			{
				id: 'act1',
				type: 'Poll',
				title: 'Which topic is most challenging?',
				definition: {
					type: 'Poll',
					options: [
						{ id: 'o1', text: 'Newtonian Mechanics' },
						{ id: 'o2', text: 'Thermodynamics' },
						{ id: 'o3', text: 'Electromagnetism' }
					]
				}
			},
			{
				id: 'act2',
				type: 'MultipleChoice',
				title: 'Which planet has the strongest gravitational pull?',
				definition: {
					type: 'MultipleChoice',
					options: [
						{ id: 'm1', text: 'Mars' },
						{ id: 'm2', text: 'Earth' },
						{ id: 'm3', text: 'Jupiter' }
					],
					correctOptionId: 'm3'
				}
			},
			{
				id: 'act3',
				type: 'ScaleRating',
				title: 'How confident are you about the upcoming exam?',
				definition: {
					type: 'ScaleRating',
					min: 1,
					max: 5,
					minLabel: 'Not at all',
					maxLabel: 'Very confident'
				}
			},
			{
				id: 'act4',
				type: 'OpenEnded',
				title: 'What are your remaining questions?',
				definition: {
					type: 'OpenEnded',
					placeholder: 'Enter your questions here...'
				}
			}
		]
	};
}

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
			definition: [],
			ownerId: 'user_alpha_123',
			version: 1.0,
			dateCreated: '2025-06-15T10:00:00Z',
			settings: {
				title: 'Weekly Team Retrospective',
				tags: ['meeting', 'agile', 'team-sync'],
				sessionPacing: 'teacher-paced',
				resultsVisibleDefault: false
			}
		},
		{
			id: 't41586',
			definition: [],
			ownerId: 'user_beta_456',
			version: 1.0,
			dateCreated: '2025-06-16T11:30:00Z',
			settings: {
				title: 'New Feature Brainstorm',
				tags: ['ideation', 'product', 'creative'],
				sessionPacing: 'student-paced',
				resultsVisibleDefault: true
			}
		},
		{
			id: 't41587',
			definition: [],
			ownerId: 'user_alpha_123',
			version: 1.0,
			dateCreated: '2025-06-17T09:00:00Z',
			settings: {
				title: 'Daily Stand-up',
				tags: ['daily', 'check-in', 'agile'],
				sessionPacing: 'teacher-paced',
				resultsVisibleDefault: true
			}
		},
		{
			id: 't41588',
			definition: [],
			ownerId: 'user_gamma_789',
			version: 1.0,
			dateCreated: '2025-05-20T14:00:00Z',
			settings: {
				title: 'Quarterly Planning Workshop',
				tags: ['planning', 'strategy', 'workshop'],
				sessionPacing: 'student-paced',
				resultsVisibleDefault: false
			}
		},
		{
			id: 't41589',
			definition: [],
			ownerId: 'user_beta_456',
			version: 1.0,
			dateCreated: '2025-06-01T16:45:00Z',
			settings: {
				title: 'User Feedback Review',
				tags: ['ux', 'feedback', 'research'],
				sessionPacing: 'teacher-paced',
				resultsVisibleDefault: true
			}
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
