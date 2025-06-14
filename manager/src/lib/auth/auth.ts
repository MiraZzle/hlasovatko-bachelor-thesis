export function signout(): void {
	localStorage.removeItem('token');
	localStorage.removeItem('user');
	/*
	await fetch('/api/auth/logout', {
		method: 'POST',
		headers: {
			Authorization: `Bearer ${localStorage.getItem('token')}`
		}
	});
	*/
}

export function isAuthenticated(): boolean {
	const token = localStorage.getItem('token');
	return token !== null && token !== '';

	/*
	try {
		const payload = JSON.parse(atob(token.split('.')[1]));
		return payload && Date.now() / 1000 < payload.exp;
	} catch {
		return false;
	}
	*/
}

export async function login(username: string, password: string): Promise<boolean> {
	if (username === 'test@example.com' && password === '1234') {
		localStorage.setItem('token', 'fake-jwt-token');
		localStorage.setItem('user', JSON.stringify({ username }));
		return true;
	}

	// enable later
	/*
	try {
		const res = await fetch('/api/auth/login', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify({ username, password })
		});

		if (!res.ok) {
			return false;
		}

		const data = await res.json();

		localStorage.setItem('token', data.token);
		localStorage.setItem('user', JSON.stringify(data.user));

		return true;
	} catch (err) {
		console.error('Login error:', err);
		return false;
	}
	*/

	return false;
}
