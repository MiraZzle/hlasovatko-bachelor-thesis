import type { PredefinedActivity, NewActivityData, ActivityType } from './types.ts';

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
				type: 'Quiz',
				title: 'Physics Brainstorm',
				definition: '{}'
			},
			categories: ['Physics', 'Event']
		},
		{
			id: 'ab1',
			refActivity: {
				id: 'ab2',
				type: 'Rating',
				title: 'Lecture Feedback',
				definition: '{}'
			},
			categories: ['Event', 'Feedback']
		},
		{
			id: 'ab2',
			refActivity: {
				id: 'ab4',
				type: 'Quiz',
				title: 'Thermodynamics Concept',
				definition: '{}'
			},
			categories: ['Physics', 'Definitions']
		},
		{
			id: 'ab3',
			refActivity: {
				id: 'ab5',
				type: 'Word Cloud',
				title: 'Entropy Description',
				definition: '{}'
			},
			categories: ['Physics', 'Concept']
		},
		{
			id: 'ab4',
			refActivity: {
				id: 'ab6',
				type: 'Poll',
				title: 'Lecture Planning',
				definition: '{}'
			},
			categories: ['Planning']
		},
		{
			id: 'ab5',
			refActivity: {
				id: 'ab7',
				type: 'Quiz',
				title: 'Chemistry Basics',
				definition: '{}'
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

	// Update the store, which will trigger reactivity in the component.

	console.log('Successfully added activity:', newActivity);
	// TODO: Add an API call here to persist the change to your backend.
}
