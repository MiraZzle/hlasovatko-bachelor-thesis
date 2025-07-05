import { browser } from '$app/environment';
import type { LayoutLoad } from './$types';
import { saveToken } from '$lib/auth/auth';

export const load: LayoutLoad = async ({ url }) => {
	if (browser) {
		const token = url.searchParams.get('token');

		if (token) {
			saveToken(token);

			const newUrl = new URL(url);
			newUrl.searchParams.delete('token');

			history.replaceState(history.state, '', newUrl);
		}
	}

	return {};
};
