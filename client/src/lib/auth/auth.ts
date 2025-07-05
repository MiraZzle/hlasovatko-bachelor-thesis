import { API_URL, API_BASE } from '$lib/config';

const PARTICIPANT_TOKEN_KEY_PREFIX = 'participant_token_';
const TOKEN_KEY = 'token';

/**
 * Retrieves the user token from localStorage.
 * @returns The users token string, or null if not found.
 */
export function getToken(): string | null {
	return localStorage.getItem(TOKEN_KEY);
}

/**
 * Saves the user token to localStorage.
 * @param token The JWT for the user.
 */
export function saveToken(token: string): void {
	localStorage.setItem(TOKEN_KEY, token);
}

/**
 * Removes the user token from localStorage.
 */
export function removeToken(): void {
	localStorage.removeItem(TOKEN_KEY);
}

/**
 * Saves a participants token to localStorage, keyed by the session ID.
 * @param sessionId The ID of the session.
 * @param token The JWT for the participant.
 */
export function saveParticipantToken(sessionId: string, token: string): void {
	localStorage.setItem(`${PARTICIPANT_TOKEN_KEY_PREFIX}${sessionId}`, token);
}

/**
 * Retrieves a participants token from localStorage for a specific session.
 * @param sessionId The ID of the session.
 * @returns The token string, or null if not found.
 */
export function getParticipantToken(sessionId: string): string | null {
	return localStorage.getItem(`${PARTICIPANT_TOKEN_KEY_PREFIX}${sessionId}`);
}

/**
 * Removes a participants token for a specific session from localStorage.
 * @param sessionId The ID of the session.
 */
export function removeParticipantToken(sessionId: string): void {
	localStorage.removeItem(`${PARTICIPANT_TOKEN_KEY_PREFIX}${sessionId}`);
}

/**
 * Calls the backend to validate the join code and get a participant token.
 * @param sessionId The ID of the session to join.
 * @param joinCode The 6-character join code.
 * @returns A promise that resolves to the participant token string, or null on failure.
 */
export async function joinSession(sessionId: string, joinCode: string): Promise<string | null> {
	try {
		const response = await fetch(`${API_URL}${API_BASE}/auth/participant/join`, {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify({ sessionId, joinCode })
		});

		if (!response.ok) {
			console.error('Failed to join session:', response.statusText);
			return null;
		}

		const data = await response.json();
		return data.token;
	} catch (error) {
		console.error('API error while joining session:', error);
		return null;
	}
}
