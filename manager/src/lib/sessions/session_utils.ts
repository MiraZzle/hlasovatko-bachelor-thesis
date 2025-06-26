import type { Session, SessionMetrics, SessionStatus } from '$lib/sessions/types';
import type { SessionMode } from '$lib/shared_types';
import { API_URL } from '$lib/config';
import { getToken } from '$lib/auth/auth';

/**
 * Interface for the Session Data Transfer Object from the backend.
 * Properties are in PascalCase.
 */
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
 * @param dto The session data from the backend.
 * @returns A frontend-compatible Session object.
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
		participants: dto.participants
	};
}

/**
 * Fetches a single session by its ID from the backend.
 * @param sessionID The ID of the session to fetch.
 * @returns A Promise that resolves to the Session object or null if not found.
 */
export async function getSessionById(sessionID: string): Promise<Session | null> {
	const token = getToken();

	const response = await fetch(`${API_URL}/api/v1/session/${sessionID}`, {
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
		throw new Error(`Failed to fetch session with ID: ${sessionID}`);
	}

	const sessionDto: SessionResponseDto = await response.json();
	return mapResponseToSession(sessionDto);
}

/**
 * Returns metrics about a session.
 * @param sessionID The ID of the session.
 * @returns Mocked session metrics.
 */
export function getSessionMetrics(sessionID: string): SessionMetrics {
	console.log(`Fetching metrics for session with ID: ${sessionID}`);

	return {
		participants: 28,
		activitiesRun: 3,
		answersReceived: 15
	};
}

/**
 * Fetches all sessions from the backend.
 * @returns A Promise that resolves to an array of Session objects.
 */
export async function getAllSessions(): Promise<Session[]> {
	const token = getToken();

	const response = await fetch(`${API_URL}/api/v1/session`, {
		method: 'GET',
		headers: {
			'Content-Type': 'application/json',
			Authorization: `Bearer ${token}`
		}
	});

	if (!response.ok) {
		throw new Error('Failed to fetch sessions');
	}

	const sessionsDto: SessionResponseDto[] = await response.json();
	return sessionsDto.map(mapResponseToSession);
}

/**
 * Fetches all sessions for a specific template ID.
 * @param templateID The ID of the template.
 * @returns A Promise that resolves to an array of Session objects.
 */
export async function getSessionsByTemplate(templateID: string): Promise<Session[]> {
	const token = getToken();

	const response = await fetch(`${API_URL}/api/v1/session/template/${templateID}`, {
		method: 'GET',
		headers: {
			'Content-Type': 'application/json',
			Authorization: `Bearer ${token}`
		}
	});

	if (!response.ok) {
		throw new Error(`Failed to fetch sessions for template ID: ${templateID}`);
	}

	const sessionsDto: SessionResponseDto[] = await response.json();
	return sessionsDto.map(mapResponseToSession);
}

/**
 * Creates a new session.
 * @param templateId The ID of the template to use for the session.
 * @param title The title of the new session.
 * @param activationDate Optional activation date for the session.
 * @param mode Optional session mode.
 * @returns A Promise that resolves to the newly created Session object.
 */
export async function createNewSession(
	templateId: string,
	title: string,
	activationDate?: string,
	mode?: SessionMode
): Promise<Session> {
	const token = getToken();

	const response = await fetch(`${API_URL}/api/v1/session`, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
			Authorization: `Bearer ${token}`
		},
		body: JSON.stringify({
			TemplateId: templateId,
			Title: title,
			ActivationDate: activationDate,
			Mode: mode
		})
	});

	if (!response.ok) {
		throw new Error('Failed to create session');
	}

	const sessionDto: SessionResponseDto = await response.json();
	return mapResponseToSession(sessionDto);
}
