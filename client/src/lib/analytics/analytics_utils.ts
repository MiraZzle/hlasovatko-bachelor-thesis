import type { ActivityResult } from '../activities/types';

export function getActivityResultsForSession(sessionId: string): ActivityResult[] {
	console.log(`Fetching activity results for session ${sessionId}`);

	return [
		{
			activityRef: {
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
			baseActivityType: 'poll',
			results: [
				{ id: 'o1', text: 'Topic A', count: 12 },
				{ id: 'o2', text: 'Topic B', count: 6 }
			]
		},
		{
			activityRef: {
				id: 'sact2',
				type: 'multiple_choice',
				title: 'What is the powerhouse of the cell?',
				definition: {
					type: 'MultipleChoice',
					options: [
						{ id: 'm1', text: 'Nucleus' },
						{ id: 'm2', text: 'Ribosome' },
						{ id: 'm3', text: 'Mitochondrion' }
					],
					correctOptionId: 'm3'
				}
			},
			baseActivityType: 'multiple_choice',
			results: [
				{ id: 'm1', text: 'Nucleus', count: 2 },
				{ id: 'm2', text: 'Ribosome', count: 1 },
				{ id: 'm3', text: 'Mitochondrion', count: 16 }
			]
		},
		{
			activityRef: {
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
			baseActivityType: 'scale_rating',
			results: [
				{ rating: 1, count: 0 },
				{ rating: 2, count: 1 },
				{ rating: 3, count: 5 },
				{ rating: 4, count: 8 },
				{ rating: 5, count: 3 }
			]
		},
		{
			activityRef: {
				id: 'sact4',
				type: 'open_ended',
				title: 'Any remaining questions?',
				definition: { type: 'OpenEnded' }
			},
			baseActivityType: 'open_ended',
			results: [
				'No questions.',
				'Explain slide 10 again.',
				'Can we have more examples?',
				'None',
				'What is the deadline?'
			]
		},
		{
			activityRef: {
				id: 'sact5',
				type: 'custom_activity',
				title: 'Custom Activity Format',
				definition: { customField: 'value', structure: { nested: true } }
			},
			results: []
		}
	];
}
