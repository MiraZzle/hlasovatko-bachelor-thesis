import type { PredefinedActivity, NewActivityData, ActivityType, Activity } from './types.ts';

export function getDefinedActivityTypes(): ActivityType[] {
	return [{ name: 'Quiz' }, { name: 'Rating' }, { name: 'Word Cloud' }, { name: 'Poll' }];
}

export function registerNewActivity(newActivity: NewActivityData): PredefinedActivity {
	console.log('Adding activity to bank:', newActivity);

	const activity: PredefinedActivity = {
		id: crypto.randomUUID(),
		refActivity: {
			id: '1',
			type: newActivity.type,
			title: newActivity.title,
			definition: newActivity.definition
		},
		categories: newActivity.categories
	};

	return activity;
}

export function getAllActivitiesFromBank(): PredefinedActivity[] {
	return [
		{
			id: 'ab0',
			refActivity: {
				id: 'ab1',
				type: 'poll',
				title: 'Physics Brainstorm Kick-off',
				definition: {
					type: 'Poll',
					options: [
						{ id: 'o1', text: 'Classical Mechanics' },
						{ id: 'o2', text: 'Quantum Mechanics' },
						{ id: 'o3', text: 'Thermodynamics' },
						{ id: 'o4', text: 'Electromagnetism' }
					]
				}
			},
			categories: ['Physics', 'Event']
		},
		{
			id: 'ab1',
			refActivity: {
				id: 'ab2',
				type: 'scale_rating',
				title: 'How well did you understand the lecture on Thermodynamics?',
				definition: {
					type: 'ScaleRating',
					min: 1,
					max: 5,
					minLabel: 'Not at all',
					maxLabel: 'Completely'
				}
			},
			categories: ['Event', 'Feedback', 'Physics']
		},
		{
			id: 'ab2',
			refActivity: {
				id: 'ab4',
				type: 'multiple_choice',
				title: 'What is the first law of thermodynamics?',
				definition: {
					type: 'MultipleChoice',
					options: [
						{ id: 'm1', text: 'Energy cannot be created or destroyed.' },
						{ id: 'm2', text: 'The entropy of an isolated system always increases.' },
						{
							id: 'm3',
							text: 'The entropy of a system approaches a constant value as the temperature approaches absolute zero.'
						}
					],
					correctOptionId: 'm1',
					allowMultiple: false
				}
			},
			categories: ['Physics', 'Definitions']
		},
		{
			id: 'ab3',
			refActivity: {
				id: 'ab5',
				type: 'open_ended',
				title: 'In your own words, what is entropy?',
				definition: {
					type: 'OpenEnded'
				}
			},
			categories: ['Physics', 'Concept']
		},
		{
			id: 'ab4',
			refActivity: {
				id: 'ab6',
				type: 'poll',
				title: 'What topic should the next lecture focus on?',
				definition: {
					type: 'Poll',
					options: [
						{ id: 'o1', text: 'Special Relativity' },
						{ id: 'o2', text: 'Particle Physics' },
						{ id: 'o3', text: 'Cosmology' }
					]
				}
			},
			categories: ['Planning']
		},
		{
			id: 'ab5',
			refActivity: {
				id: 'ab7',
				type: 'multiple_choice',
				title: 'Which of the following is a noble gas?',
				definition: {
					type: 'MultipleChoice',
					options: [
						{ id: 'm1', text: 'Oxygen' },
						{ id: 'm2', text: 'Helium' },
						{ id: 'm3', text: 'Nitrogen' },
						{ id: 'm4', text: 'Carbon' }
					],
					correctOptionId: 'm2',
					allowMultiple: false
				}
			},
			categories: ['Chemistry', 'Definitions']
		}
	];
}

export function createActivity(data: NewActivityData): void {
	const newActivity: PredefinedActivity = {
		id: crypto.randomUUID(), // Generate a unique ID for the new activity
		refActivity: {
			id: '1',
			type: data.type,
			title: data.title,
			definition: data.definition
		},
		categories: data.categories
	};

	console.log('Successfully added activity:', newActivity);
}

export function getActivitiesFromSession(sessionId: string): Activity[] {
	console.log(`Fetching activities for session ${sessionId}`);

	return [
		{
			id: 'sact1',
			type: 'poll',
			title: 'Which topic should we cover next?',
			definition: {
				type: 'Poll',
				options: [
					{ id: 'o1', text: 'Topic A' },
					{ id: 'o2', text: 'Topic B' }
				]
			}
		},
		{
			id: 'sact2',
			type: 'multiple_choice',
			title: 'What is the powerhouse of the cell?',
			definition: {
				type: 'MultipleChoice',
				options: [
					{ id: 'm1', text: 'Nucleus' },
					{ id: 'm2', text: 'Ribosome' },
					{ id: 'm3', text: 'Mitochondrion' },
					{ id: 'm4', text: 'Chloroplast' }
				],
				correctOptionId: 'm3',
				allowMultiple: false
			}
		},
		{
			id: 'sact3',
			type: 'scale_rating',
			title: 'Rate your understanding (1-5)',
			definition: {
				type: 'ScaleRating',
				min: 1,
				max: 5,
				minLabel: 'Confused',
				maxLabel: 'Confident'
			}
		},
		{
			id: 'sact4',
			type: 'open_ended',
			title: 'Any remaining questions?',
			definition: { type: 'OpenEnded' }
		},
		{
			id: 'sact5',
			type: 'custom_activity',
			title: 'Custom Activity Format',
			definition: { customField: 'value', structure: { nested: true } }
		}
	];
}
