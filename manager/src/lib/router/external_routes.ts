import { getToken } from '$lib/auth/auth';
import { CLIENT_URL } from '$lib/config';
import { getSessionInfoByJoinCode } from '$lib/sessions/session_utils';

/**
 * Generates a link to manage a session.
 * @param sessionID The ID of the session.
 * @param includeToken Whether to include the authentication token in the URL.
 * @returns The URL to manage the session.
 */
export function getManageSessionLink(sessionID: string, includeToken: boolean = true): string {
	const token = getToken();
	if (includeToken && token) {
		return CLIENT_URL + '/manage/' + sessionID + '?token=' + token;
	} else return CLIENT_URL + '/manage/' + sessionID;
}

/**
 * Generates a link to participate in a session.
 * @param sessionID The ID of the session.
 * @param joinCode The join code for the session.
 * @returns The URL to participate in the session.
 */
export function getParticipateSessionLink(sessionID: string, joinCode: string): string {
	return CLIENT_URL + '/participate/' + sessionID + '?code=' + joinCode;
}

/**
 * Generates a link to participate in a session using the join code.
 * This is used when the user has the join code but not the session ID.
 * @param code The join code for the session.
 * @returns The URL to participate in the session.
 */
export async function getParticipateSessionLinkWithCode(code: string): Promise<string> {
	const sessionJoinInfo = await getSessionInfoByJoinCode(code);
	if (!sessionJoinInfo) {
		throw new Error('Session not found for join code: ' + code);
	}

	const sessionId = sessionJoinInfo.id;
	return getParticipateSessionLink(sessionId, code);
}
