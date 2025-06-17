import { getToken } from '$lib/auth/auth';

export function getManageSessionLink(sessionID: string, includeToken: boolean = true): string {
	const token = getToken();
	if (includeToken && token) {
		return import.meta.env.VITE_CLIENT_URL + '/manage/' + sessionID + '?token=' + token;
	} else return import.meta.env.VITE_CLIENT_URL + '/manage/' + sessionID;
}

export function getParticipateSessionLink(sessionID: string): string {
	return import.meta.env.VITE_CLIENT_URL + '/participate/' + sessionID;
}
