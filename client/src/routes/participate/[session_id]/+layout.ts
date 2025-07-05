import { error, redirect } from '@sveltejs/kit';
import type { LayoutLoad } from './$types';
import { browser } from '$app/environment';
import { getParticipantToken, joinSession, saveParticipantToken } from '$lib/auth/auth';

export const load: LayoutLoad = async ({ params, url }) => {
	const { session_id } = params;

	if (browser) {
		let token = getParticipantToken(session_id);

		if (!token) {
			const joinCode = url.searchParams.get('code');

			if (!joinCode) {
				throw redirect(307, '/');
			}

			const newToken = await joinSession(session_id, joinCode);

			if (newToken) {
				saveParticipantToken(session_id, newToken);
				token = newToken;
			} else {
				throw error(403, 'Invalid join code or session is not active.');
			}
		}

		if (token) {
			return {
				sessionId: session_id,
				participantToken: token
			};
		}
	}

	return {
		sessionId: session_id,
		participantToken: null
	};
};
