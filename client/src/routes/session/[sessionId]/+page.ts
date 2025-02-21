import { API_URL } from '$lib/config';
import type { PageLoad } from './$types';

export const load: PageLoad = async ({ params }: { params: { sessionId: string } }) => {
	const { sessionId } = params;

	try {
		const res = await fetch(`${API_URL}/api/sessions/${sessionId}`);

		if (!res.ok) {
			throw new Error('Session not found');
		}

		const session = await res.json();
		return { session };
	} catch (error) {
		console.error('Error fetching session:', error);
		return { session: null };
	}
};
