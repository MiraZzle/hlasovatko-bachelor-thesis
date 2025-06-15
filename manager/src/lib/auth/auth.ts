import type { User } from './types';

export function logout(): void {
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

// return user
export async function login(email: string, password: string): Promise<User | null> {
	if (email === 'test@example.com' && password === '1234') {
		const user: User = {
			id: '1',
			name: 'Test User',
			email: email,
			token: 'fake-jwt-token'
		};
		localStorage.setItem('token', user.token);
		localStorage.setItem('user', JSON.stringify(user));
		return user;
	} else {
		return null;
	}

	try {
		const res = await fetch('/api/auth/login', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify({ email, password })
		});

		if (!res.ok) return null;

		const data = await res.json();

		// Validate shape if necessary
		const user: User = {
			id: data.user.id,
			name: data.user.name,
			email: data.user.email,
			token: data.token
		};

		// Persist user
		localStorage.setItem('token', user.token);
		localStorage.setItem('user', JSON.stringify(user));
		return user;
	} catch (err) {
		console.error('Login error:', err);
		return null;
	}
}

export function getUser(): { username: string } | null {
	const user = localStorage.getItem('user');
	if (user) {
		try {
			return JSON.parse(user);
		} catch (e) {
			console.error('Error parsing user data:', e);
			return null;
		}
	}
	return null;
}

export async function changePassword(oldPassword: string, newPassword: string): Promise<boolean> {
	if (oldPassword === '1234' && newPassword.length >= 6) {
		localStorage.setItem('password', newPassword);
		return true;
	}
	return false;

	// Enable later
	/*
    try {
        const res = await fetch('/api/auth/change-password', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('token')}`
            },
            body: JSON.stringify({ oldPassword, newPassword })
        });

        return res.ok;
    } catch (err) {
        console.error('Change password error:', err);
        return false;
    }
    */
}

export async function regenerateApiKey(): Promise<string> {
	return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
	// Simulate API key generation

	/*
	try {
		const res = await fetch('/api/auth/generate-api-key', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
				Authorization: `Bearer ${localStorage.getItem('token')}`
			}
		});
		if (!res.ok) {
			throw new Error('Failed to generate API key');
		}
		const data = await res.json();
		return data.apiKey;
	} catch (err) {
		console.error('API key generation error:', err);
		return '';
	}
	*/
}

export async function getPartialApiKey(): Promise<string> {
	// Simulate fetching a partial API key
	return Math.random().toString(36).substring(2, 8);
	/*
	try {
		const res = await fetch('/api/auth/get-partial-api-key', {
			method: 'GET',
			headers: {
				Authorization: `Bearer ${localStorage.getItem('token')}`
			}
		});
		if (!res.ok) {
			throw new Error('Failed to fetch partial API key');
		}
		const data = await res.json();
		return data.partialApiKey;
	} catch (err) {
		console.error('Partial API key fetch error:', err);
		return '';
	}
	*/
}
