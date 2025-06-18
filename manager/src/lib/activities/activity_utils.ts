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
				title: 'Physics Brainstorm',
				definition: {}
			},
			categories: ['Physics', 'Event']
		},
		{
			id: 'ab1',
			refActivity: {
				id: 'ab2',
				type: 'scale_rating',
				title: 'Lecture Feedback',
				definition: {}
			},
			categories: ['Event', 'Feedback']
		},
		{
			id: 'ab2',
			refActivity: {
				id: 'ab4',
				type: 'scale_rating',
				title: 'Thermodynamics Concept',
				definition: {}
			},
			categories: ['Physics', 'Definitions']
		},
		{
			id: 'ab3',
			refActivity: {
				id: 'ab5',
				type: 'scale_rating',
				title: 'Entropy Description',
				definition: {}
			},
			categories: ['Physics', 'Concept']
		},
		{
			id: 'ab4',
			refActivity: {
				id: 'ab6',
				type: 'scale_rating',
				title: 'Lecture Planning',
				definition: {}
			},
			categories: ['Planning']
		},
		{
			id: 'ab5',
			refActivity: {
				id: 'ab7',
				type: 'scale_rating',
				title: 'Chemistry Basics',
				definition: {}
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
	// TODO: Add an API call here to persist the change to your backend.
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
