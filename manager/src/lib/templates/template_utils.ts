export function getAvailableTemplates() {}

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

export function getAllTemplates() {
	return [
		{
			id: 't41585',
			title: 'Quiz 1 - NDBI046 Spring',
			created: '2025-04-01T10:00:00Z',
			createdBy: 'Matúš',
			activitiesCount: 5,
			sessionsCount: 2
		},
		{
			id: 't41586',
			title: 'Quiz 2 - NDBI046 Spring',
			created: '2025-04-08T10:00:00Z',
			createdBy: 'Matúš',
			activitiesCount: 4,
			sessionsCount: 1
		},
		{
			id: 't41587',
			title: 'Poll - Lecture 3 Feedback',
			created: '2025-04-03T14:30:00Z',
			createdBy: 'Matúš',
			activitiesCount: 3,
			sessionsCount: 1
		},
		{
			id: 't41588',
			title: 'Midterm Review Session',
			created: '2025-04-10T09:00:00Z',
			createdBy: 'Matúš',
			activitiesCount: 2,
			sessionsCount: 0
		}
	];
}
