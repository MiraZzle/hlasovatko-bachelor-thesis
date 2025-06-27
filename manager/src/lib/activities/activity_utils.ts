import type { ActivityResponse, ActivityType, Activity } from './types.ts';
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
		return activities.map((act: ActivityResponse) => ({
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

/**
 * Fetches all activities for a given session.
 * @param sessionId The ID of the session.
 * @returns A promise that resolves to an array of activities.
 */
export async function getActivitiesFromSession(sessionId: string): Promise<Activity[]> {
	const token = getToken();
	if (!token) {
		console.error('Authentication token not found.');
		return [];
	}

	try {
		const response = await fetch(`${API_URL}/api/v1/session/${sessionId}/activities`, {
			method: 'GET',
			headers: {
				Authorization: `Bearer ${token}`
			}
		});

		if (!response.ok) {
			const errorText = await response.text();
			console.error(
				`Failed to fetch activities for session ${sessionId}:`,
				response.status,
				errorText
			);
			return [];
		}

		const activitiesFromApi = await response.json();

		return activitiesFromApi.map((activity: ActivityResponse) => ({
			id: activity.id,
			title: activity.title,
			type: activity.activityType,
			definition: activity.definition,
			tags: activity.tags
		}));
	} catch (err) {
		console.error(`API error when fetching activities for session ${sessionId}:`, err);
		return [];
	}
}
