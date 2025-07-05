import { API_URL, API_BASE } from '$lib/config';
import type { Activity, ActivityResult, StaticActivityType } from '$lib/activities/types';
import type { SubmitPayload } from '$lib/activities/definition_types';
import { getParticipantToken } from '$lib/auth/auth';
import { getToken } from '$lib/auth/auth';

interface ActivityResultDto {
	activityRef: ActivityResponseDto;
	results: unknown[];
}

interface ActivityResponseDto {
	id: string;
	title: string;
	activityType: string;
	definition: object;
	tags: string[];
}

/**
 * Submits an answer for an activity.
 */
export async function submitAnswer(sessionId: string, payload: SubmitPayload): Promise<boolean> {
	const token = getParticipantToken(sessionId);
	let answerJson: object;

	if (!token) {
		console.error('Participant token not found for this session.');
		return false;
	}

	switch (payload.activityType) {
		case 'poll':
			answerJson = { selectedOptionId: payload.value };
			break;
		case 'multiple_choice':
			answerJson = {
				selectedOptionIds: Array.isArray(payload.value) ? payload.value : [payload.value]
			};
			break;
		case 'open_ended':
			answerJson = { text: payload.value };
			break;
		case 'scale_rating':
			answerJson = { value: payload.value };
			break;
		default:
			console.error(`Unknown activity type for submission: ${payload.activityType}`);
			return false;
	}

	const response = await fetch(`${API_URL}${API_BASE}/answer/${sessionId}`, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
			Authorization: `Bearer ${token}`
		},
		body: JSON.stringify({
			activityId: payload.activityId,
			answerJson: answerJson
		})
	});

	if (!response.ok) {
		const error = await response.json();
		console.error('Answer submission failed:', error.message);
	}
	return response.ok;
}

/**
 * Fetches the aggregated results for a single activity.
 * @param sessionId The ID of the session.
 * @param activityId The ID of the activity.
 */
export async function getActivityResults(
	sessionId: string,
	activityId: string
): Promise<ActivityResult | null> {
	const token = getParticipantToken(sessionId) || getToken();

	if (!token) {
		console.error('Participant token not found for this session.');
		return null;
	}

	try {
		const response = await fetch(
			`${API_URL}${API_BASE}/answer/session/${sessionId}/activity/${activityId}/results`,
			{
				method: 'GET',
				headers: {
					Authorization: `Bearer ${token}`
				}
			}
		);

		if (!response.ok) {
			console.error('Failed to fetch activity results:', response.statusText);
			return null;
		}

		const dto: ActivityResultDto = await response.json();

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
	} catch (err) {
		console.error(`API error when fetching results for activity ${activityId}:`, err);
		return null;
	}
}
