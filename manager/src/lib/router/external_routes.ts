import { getToken } from '$lib/auth/auth';
import { CLIENT_URL } from '$lib/config';

export function getManageSessionLink(sessionID: string, includeToken: boolean = true): string {
	const token = getToken();
	if (includeToken && token) {
		return CLIENT_URL + '/manage/' + sessionID + '?token=' + token;
	} else return CLIENT_URL + '/manage/' + sessionID;
}

export function getParticipateSessionLink(sessionID: string): string {
	return CLIENT_URL + '/participate/' + sessionID;
}

export function getParticipateSessionLinkWithCode(code: string): string {
	return CLIENT_URL + '/participate/?code=' + code;
}
