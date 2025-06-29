import { API_URL } from '$lib/config';
import type { Activity, ActivityResult, StaticActivityType } from '$lib/activities/types';
import type { SubmitPayload } from '$lib/activities/definition_types';

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
 * Gets or creates a unique participant ID for the current browser session.
 */
export function getParticipantId(): string {
	let participantId = sessionStorage.getItem('participantId');
	if (!participantId) {
		participantId = crypto.randomUUID();
		sessionStorage.setItem('participantId', participantId);
	}
	return participantId;
}

/**
 * Submits an answer for an activity.
 */
export async function submitAnswer(sessionId: string, payload: SubmitPayload): Promise<boolean> {
	const participantId = getParticipantId();
	let answerJson: object;

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

	const response = await fetch(`${API_URL}/api/v1/answer/${sessionId}`, {
		method: 'POST',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify({
			activityId: payload.activityId,
			participantId: participantId,
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
 */
export async function getActivityResults(
	sessionId: string,
	activityId: string
): Promise<ActivityResult | null> {
	const response = await fetch(
		`${API_URL}/api/v1/answer/session/${sessionId}/activity/${activityId}/results`
	);
	if (!response.ok) return null;
	const dto: ActivityResultDto = await response.json();

	// Correctly map the DTO to the client-side Activity type
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
}
