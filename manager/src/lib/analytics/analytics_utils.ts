import type { TimeFrame } from '$lib/analytics/types';
import type { ActivityResult, StaticActivityType } from '$lib/activities/types';
import { getToken } from '$lib/auth/auth';
import { API_URL } from '$lib/config';
import type { Activity } from '$lib/activities/types';
interface ActivityResponseDto {
	id: string;
	title: string;
	activityType: string;
	definition: object;
	tags: string[];
}

interface ActivityResultDto {
	activityRef: ActivityResponseDto;
	results: unknown[];
}

export function getTotalResponses(timeFrame: TimeFrame): number {
	console.log(`Fetching total responses for time frame: ${timeFrame}`);
	return 123;
}
export function getMostPopularActivityType(timeFrame: TimeFrame): string {
	console.log(`Fetching total responses for time frame: ${timeFrame}`);
	return 'Poll';
}
export function getTotalNumberOfSessions(timeFrame: TimeFrame): number {
	console.log(`Fetching total responses for time frame: ${timeFrame}`);
	return 10;
}

/**
 * Fetches and processes the aggregated results for all activities in a given session.
 * @param sessionId The ID of the session to get results for.
 * @returns A promise that resolves to an array of ActivityResult objects.
 */
export async function getActivityResultsForSession(sessionId: string): Promise<ActivityResult[]> {
	const token = getToken();
	if (!token) {
		console.error('Authentication token not found.');
		return [];
	}

	try {
		const response = await fetch(`${API_URL}/api/v1/answer/session/${sessionId}/results`, {
			method: 'GET',
			headers: {
				Authorization: `Bearer ${token}`
			}
		});

		if (!response.ok) {
			const errorText = await response.text();
			console.error(
				`Failed to fetch activity results for session ${sessionId}:`,
				response.status,
				errorText
			);
			return [];
		}

		const resultsDto: ActivityResultDto[] = await response.json();
		return resultsDto.map((dto) => {
			const activity: Activity = {
				id: dto.activityRef.id,
				title: dto.activityRef.title,
				type: dto.activityRef.activityType as StaticActivityType,
				definition: dto.activityRef.definition,
				tags: dto.activityRef.tags
			};

			return {
				activityRef: activity,
				baseActivityType: activity.type,
				results: dto.results
			};
		});
	} catch (err) {
		console.error(`API error when fetching activity results for session ${sessionId}:`, err);
		return [];
	}
}
