import type { PageLoad } from './$types';

export const load: PageLoad = async ({ params }: { params: { sessionId: string } }) => {
	const { sessionId } = params;
	return { sessionId };
};
