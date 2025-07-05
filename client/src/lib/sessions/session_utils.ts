import type { Session } from './types';
import type { SessionStatus } from './types';
import type { SessionMode } from '$lib/shared_types';
import { API_URL, API_BASE } from '$lib/config';
import type { Activity, ActivityResponse } from '$lib/activities/types';
import type { SessionJoinInfo } from './types';
import { getToken, getParticipantToken } from '$lib/auth/auth';

export interface ParticipantSessionState {
	sessionId: string;
	status: SessionStatus;
	currentActivityId: string | null;
	showResults: boolean;
}

interface SessionResponseDto {
	id: string;
	title: string;
	templateId: string;
	templateVersion: number;
	status: SessionStatus;
	joinCode: string;
	activationDate?: string;
	mode: SessionMode;
	participants: number;
	currentActivity?: number;
}

/**
 * Maps a SessionResponseDto from the backend to a Session object for the frontend.
 */
function mapResponseToSession(dto: SessionResponseDto): Session {
	return {
		id: dto.id,
		title: dto.title,
		templateID: dto.templateId,
		templateVersion: dto.templateVersion.toString(),
		status: dto.status,
		created: dto.activationDate || new Date().toISOString(),
		joinCode: dto.joinCode,
		activationDate: dto.activationDate,
		mode: dto.mode,
		participants: dto.participants,
		currentActivity: dto.currentActivity ?? undefined
	};
}

/**
 * Fetches a single session by its ID from the backend.
 * @param sessionId The id of the session to fetch.
 * @returns A Promise that resolves to the Session object or null if not found.
 */
export async function getSessionById(sessionId: string): Promise<Session | null> {
	const token = getParticipantToken(sessionId) || getToken();

	const response = await fetch(`${API_URL}${API_BASE}/session/${sessionId}`, {
		method: 'GET',
		headers: {
			'Content-Type': 'application/json',
			Authorization: `Bearer ${token}`
		}
	});

	if (!response.ok) {
		if (response.status === 404) {
			return null;
		}
		throw new Error(`Failed to fetch session with ID: ${sessionId}`);
	}

	const sessionDto: SessionResponseDto = await response.json();
	return mapResponseToSession(sessionDto);
}

/**
 * Fetches all activities for a given session.
 * @param sessionId The ID of the session.
 * @returns A promise that resolves to an array of activities.
 */
export async function getActivitiesFromSession(sessionId: string): Promise<Activity[]> {
	const token = getParticipantToken(sessionId) || getToken();
	if (!token) {
		console.error('Authentication token not found.');
		return [];
	}

	try {
		const response = await fetch(`${API_URL}${API_BASE}/session/${sessionId}/activities`, {
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

/**
 * Fetches the lightweight current state of a session for polling.
 */
export async function getSessionState(sessionId: string): Promise<ParticipantSessionState | null> {
	const token = getParticipantToken(sessionId);
	if (!token) {
		console.error('Participant token not found for this session.');
		return null;
	}

	try {
		const response = await fetch(`${API_URL}${API_BASE}/session/${sessionId}/state`, {
			headers: {
				Authorization: `Bearer ${token}`
			}
		});
		if (!response.ok) {
			return null;
		}
		const dto: ParticipantSessionState = await response.json();

		return {
			sessionId: dto.sessionId,
			status: dto.status,
			currentActivityId: dto.currentActivityId,
			showResults: dto.showResults
		};
	} catch (err) {
		console.error('API error fetching session state:', err);
		return null;
	}
}

/**
 * Fetches the basic session info (ID, title, mode) using the join code.
 * This is the FIRST call a participant makes.
 */
export async function getSessionInfoByJoinCode(joinCode: string): Promise<SessionJoinInfo | null> {
	try {
		const response = await fetch(`${API_URL}${API_BASE}/session/join/${joinCode}`);
		if (!response.ok) {
			return null;
		}
		const dto: SessionJoinInfo = await response.json();

		return {
			id: dto.id,
			title: dto.title,
			mode: dto.mode
		};
	} catch (err) {
		console.error('API error fetching session info:', err);
		return null;
	}
}

/**
 * Sends a request to advance to the next activity.
 */
export async function nextActivity(sessionId: string): Promise<Session | null> {
	const token = getToken();
	try {
		const response = await fetch(`${API_URL}${API_BASE}/session/${sessionId}/next`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
				Authorization: `Bearer ${token}`
			}
		});
		if (!response.ok) {
			throw new Error('Failed to advance to next activity');
		}
		const sessionDto: SessionResponseDto = await response.json();
		return mapResponseToSession(sessionDto);
	} catch (error) {
		console.error('Next activity error:', error);
		return null;
	}
}
