import type { ActivityBankResponse, ActivityType, Activity } from './types.ts';
import { getToken } from '$lib/auth/auth';
import { API_URL } from '$lib/config';
export function getDefinedActivityTypes(): ActivityType[] {
	return [
		{ name: 'multiple_choice' },
		{ name: 'poll' },
		{ name: 'scale_rating' },
		{ name: 'open_ended' }
	];
}

/**
 * Adds a new activity to the user's Activity Bank.
 * @param activityData The data for the new activity.
 * @returns The newly created activity object, or null on failure.
 */
export async function addActivityToBank(activityData: {
	title: string;
	type: string;
	definition: object;
	tags: string[];
}): Promise<Activity | null> {
	const token = getToken();
	if (!token) return null;

	try {
		const res = await fetch(`${API_URL}/api/v1/activity-bank`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
				Authorization: `Bearer ${token}`
			},
			body: JSON.stringify({
				title: activityData.title,
				activityType: activityData.type,
				definition: JSON.stringify(activityData.definition),
				tags: activityData.tags
			})
		});

		if (!res.ok) {
			console.error('Failed to create activity:', res.statusText);
			return null;
		}

		const newActivity = await res.json();
		return {
			id: newActivity.id,
			title: newActivity.title,
			type: newActivity.activityType,
			definition: newActivity.definition,
			tags: newActivity.tags
		};
	} catch (err) {
		console.error('Create activity API error:', err);
		return null;
	}
}

/**
 * Fetches all activities from the logged-in user's "Activity Bank".
 * @returns A promise that resolves to an array of activities.
 */
export async function getActivityBank(): Promise<Activity[]> {
	const token = getToken();
	if (!token) return [];

	try {
		const res = await fetch(`${API_URL}/api/v1/activity-bank`, {
			method: 'GET',
			headers: {
				Authorization: `Bearer ${token}`
			}
		});

		if (!res.ok) {
			console.error('Failed to fetch activity bank:', res.statusText);
			return [];
		}

		const activities = await res.json();
		return activities.map((act: ActivityBankResponse) => ({
			id: act.id,
			title: act.title,
			type: act.activityType,
			definition: act.definition,
			tags: act.tags
		}));
	} catch (err) {
		console.error('Get activity bank API error:', err);
		return [];
	}
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
