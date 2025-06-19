import type { LayoutLoad } from './$types';
import { getSessionById } from '$lib/sessions/session_utils';

export const load: LayoutLoad = async ({ params, fetch }) => {
	const sessionData = getSessionById(params.session_id);

	if (!sessionData) {
		return {
			status: 404,
			error: new Error('Session not found')
		};
	}

	return {
		sessionDataForTopbar: {
			id: sessionData.id,
			title: sessionData.title,
			status: sessionData.status,
			joinCode: sessionData.joinCode
		}
	};
};
