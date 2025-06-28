import { browser } from '$app/environment';
import { goto } from '$app/navigation';
import type { LayoutLoad } from './$types';

export const load: LayoutLoad = async ({ url }) => {
	if (browser) {
		const token = url.searchParams.get('token');

		if (token) {
			console.log('JWT token found in URL, saving to local storage.');
			localStorage.setItem('jwt_token', token);
			const newUrl = new URL(url);
			newUrl.searchParams.delete('token');

			await goto(newUrl.toString(), { replaceState: true });
		}
	}

	return {};
};
