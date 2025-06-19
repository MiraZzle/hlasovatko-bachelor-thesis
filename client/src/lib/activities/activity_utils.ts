import type { Activity } from './types.ts';
import type { SubmitPayload } from './definition_types.ts';

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

export function gotoNextActivity(sessionId: string, currentActivityId: string): void {
	console.log(`Going to next activity for session ${sessionId} from activity ${currentActivityId}`);
}

export function submitActivityAnswer(payload: SubmitPayload): Promise<boolean> {
	console.log(`Submitting answer for activity ${payload.activityId}: ${payload.value}`);

	return new Promise((resolve) => {
		// Simulate an API call to submit the answer
		setTimeout(() => {
			console.log(`Answer for activity ${payload.activityId} submitted successfully.`);
			resolve(true);
		}, 500);
	});
}
