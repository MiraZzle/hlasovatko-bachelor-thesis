import type { Session, SessionStatus } from '$lib/sessions/types';
import type { SessionMode } from '$lib/shared_types';
import { API_URL } from '$lib/config';
import { getToken } from '$lib/auth/auth';
import type { SessionJoinInfo } from '$lib/sessions/types';

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
	createdAt?: string;
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
		created: dto.createdAt || new Date().toISOString(),
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
	if (!token) return null;

	try {
		const response = await fetch(`${API_URL}/api/v1/session/${sessionID}`, {
			method: 'GET',
			headers: {
				'Content-Type': 'application/json',
				Authorization: `Bearer ${token}`
			}
		});

		if (!response.ok) {
			console.error(`Failed to fetch session with ID ${sessionID}:`, response.statusText);
			return null;
		}

		const sessionDto: SessionResponseDto = await response.json();
		return mapResponseToSession(sessionDto);
	} catch (err) {
		console.error('API error fetching session:', err);
		return null;
	}
}

/**
 * Fetches the basic session info (ID, title, mode) using the join code.
 * This is the FIRST call a participant makes.
 */
export async function getSessionInfoByJoinCode(joinCode: string): Promise<SessionJoinInfo | null> {
	try {
		const response = await fetch(`${API_URL}/api/v1/session/join/${joinCode}`);
		if (!response.ok) {
			return null;
		}
		const dto: SessionJoinInfo = await response.json();

		// Manually map the DTO to the frontend type for type safety.
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

/**
 * Sends a request to start a session.
 * @param sessionId The ID of the session to start.
 * @returns A promise that resolves to the updated Session object.
 */
export async function startSession(sessionId: string): Promise<Session> {
	const token = getToken();

	const response = await fetch(`${API_URL}/api/v1/session/${sessionId}/start`, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
			Authorization: `Bearer ${token}`
		}
	});

	if (!response.ok) {
		let errorMessage = 'Failed to start the session.';
		try {
			const errorData = await response.json();
			errorMessage = errorData.message || errorMessage;
		} catch (e) {
			console.log('Error parsing response:', e);
		}
		throw new Error(errorMessage);
	}

	const sessionDto: SessionResponseDto = await response.json();
	return mapResponseToSession(sessionDto);
}

/**
 * Sends a request to stop a session.
 * @param sessionId The ID of the session to stop.
 * @returns A promise that resolves to the updated Session object.
 */
export async function stopSession(sessionId: string): Promise<Session> {
	const token = getToken();
	const response = await fetch(`${API_URL}/api/v1/session/${sessionId}/stop`, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
			Authorization: `Bearer ${token}`
		}
	});

	if (!response.ok) {
		let errorMessage = 'Failed to stop the session.';
		try {
			const errorData = await response.json();
			errorMessage = errorData.message || errorMessage;
		} catch (e) {
			console.log('Error parsing response:', e);
		}
		throw new Error(errorMessage);
	}

	const sessionDto: SessionResponseDto = await response.json();
	return mapResponseToSession(sessionDto);
}

/**
 * Deletes a session by its ID.
 * @param sessionId The ID of the session to delete.
 * @returns True on success, false otherwise.
 */
export async function deleteSession(sessionId: string): Promise<boolean> {
	const token = getToken();
	if (!token) return false;

	try {
		const res = await fetch(`${API_URL}/api/v1/session/${sessionId}`, {
			method: 'DELETE',
			headers: {
				Authorization: `Bearer ${token}`
			}
		});

		return res.ok;
	} catch (err) {
		console.error('Delete session API error:', err);
		return false;
	}
}
