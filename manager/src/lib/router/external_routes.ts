import { getToken } from '$lib/auth/auth';
import { CLIENT_URL } from '$lib/config';
import { getSessionInfoByJoinCode } from '$lib/sessions/session_utils';

export function getManageSessionLink(sessionID: string, includeToken: boolean = true): string {
	const token = getToken();
	if (includeToken && token) {
		return CLIENT_URL + '/manage/' + sessionID + '?token=' + token;
	} else return CLIENT_URL + '/manage/' + sessionID;
}

export function getParticipateSessionLink(sessionID: string): string {
	return CLIENT_URL + '/participate/' + sessionID;
}

export async function getParticipateSessionLinkWithCode(code: string): Promise<string> {
	const sessionJoinInfo = await getSessionInfoByJoinCode(code);
	if (!sessionJoinInfo) {
		throw new Error('Session not found for join code: ' + code);
	}

	const sessionId = sessionJoinInfo.id;

	return CLIENT_URL + '/participate/' + sessionId;
}
