import type { ActivityResult, StaticActivityType } from '$lib/activities/types';
import { getToken } from '$lib/auth/auth';
import { API_URL } from '$lib/config';
import type { Activity } from '$lib/activities/types';
import type { Statistics } from './types';
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

/**
 * Fetches general usage statistics for the current user from the API.
 * @returns A promise that resolves to a Statistics object, or null if an error occurs.
 */
export async function getStatistics(): Promise<Statistics | null> {
	const token = getToken();
	if (!token) {
		console.error('Authentication token not found.');
		return null;
	}

	try {
		const response = await fetch(`${API_URL}/api/v1/statistics`, {
			method: 'GET',
			headers: {
				Authorization: `Bearer ${token}`
			}
		});

		if (!response.ok) {
			console.error('Failed to fetch statistics:', response.status, await response.text());
			return null;
		}

		return await response.json();
	} catch (err) {
		console.error('API error when fetching statistics:', err);
		return null;
	}
}

/**
 * Triggers a file download of the user's statistics in the specified format.
 * @param format The desired file format, either 'csv' or 'json'.
 */
export async function exportStatistics(format: 'csv' | 'json'): Promise<void> {
	const token = getToken();
	if (!token) {
		console.error('Authentication token not found.');
		return;
	}

	try {
		const response = await fetch(`${API_URL}/api/v1/statistics/export?format=${format}`, {
			method: 'GET',
			headers: {
				Authorization: `Bearer ${token}`
			}
		});

		if (!response.ok) {
			console.error('Failed to export statistics:', response.status, await response.text());
			return;
		}

		const blob = await response.blob();
		const url = window.URL.createObjectURL(blob);
		const a = document.createElement('a');
		a.style.display = 'none';
		a.href = url;
		// e.g., statistics-2025-06-29.csv
		a.download = `statistics-${new Date().toISOString().split('T')[0]}.${format}`;
		document.body.appendChild(a);
		a.click();
		window.URL.revokeObjectURL(url);
		a.remove();
	} catch (err) {
		console.error(`API error during statistics export (format: ${format}):`, err);
	}
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
