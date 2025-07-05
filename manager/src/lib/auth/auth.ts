import { API_URL, API_BASE } from '$lib/config';
import type { User } from './types';
import { toast } from '$lib/stores/toast_store';
import { browser } from '$app/environment';

const TOKEN_KEY = 'token';
const USER_KEY = 'user';

/**
 * Logs the user out by clearing their session data from localStorage.
 */
export function logout(): void {
	if (!browser) {
		return;
	}
	localStorage.removeItem(TOKEN_KEY);
	localStorage.removeItem(USER_KEY);
	toast.show('You have been logged out.', 'info');
}

/**
 * Clears the JWT token from local storage.
 */
export function clearToken(): void {
	if (!browser) {
		return;
	}
	localStorage.removeItem(TOKEN_KEY);
}

/**
 * Retrieves the JWT from localStorage.
 * @returns The token string, or null if it doesn't exist.
 */
export function getToken(): string | null {
	if (!browser) {
		return null;
	}
	return localStorage.getItem(TOKEN_KEY);
}

/**
 * Checks if the user is authenticated.
 * For a more secure check, it decodes the JWT to see if it has expired.
 * @returns True if a valid, non-expired token exists, otherwise false.
 */
export function isAuthenticated(): boolean {
	const token = getToken();
	if (!token) {
		return false;
	}

	try {
		const payload = JSON.parse(atob(token.split('.')[1]));
		// Check if the token's expiration time is in the future.
		return payload && Date.now() / 1000 < payload.exp;
	} catch {
		// If the token is malformed, treat it as invalid.
		return false;
	}
}

/**
 * Retrieves the stored user object from localStorage.
 * @returns The user object, or null if not found or invalid.
 */
export function getUser(): User | null {
	if (!browser) {
		return null;
	}
	const userJson = localStorage.getItem(USER_KEY);
	if (userJson) {
		try {
			return JSON.parse(userJson) as User;
		} catch (e) {
			console.error('Error parsing user data:', e);
			return null;
		}
	}
	return null;
}

/**
 * Attempts to log in a user via the API.
 * On success, stores the user and token in localStorage.
 * @param email - The user's email.
 * @param password - The user's password.
 * @returns The User object on success, or null on failure.
 */
export async function login(email: string, password: string): Promise<User | null> {
	try {
		const res = await fetch(`${API_URL}${API_BASE}/auth/login`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify({ email, password })
		});

		if (!res.ok) {
			console.error('Login failed:', res.statusText);
			return null;
		}

		const data = await res.json();

		const user: User = {
			id: data.userId,
			name: data.name,
			email: data.email,
			token: data.token
		};

		if (browser) {
			localStorage.setItem(TOKEN_KEY, user.token);
			localStorage.setItem(USER_KEY, JSON.stringify(user));
			toast.show(`👋 Welcome back, ${user.name}!`, 'info');
		}
		return user;
	} catch (err) {
		console.error('Login API error:', err);
		return null;
	}
}

/**
 * Registers a new user via the API.
 * @param name - The new user's name.
 * @param email - The new user's email.
 * @param password - The new user's password.
 * @returns True on success, false on failure.
 */
export async function register(name: string, email: string, password: string): Promise<boolean> {
	try {
		const res = await fetch(`${API_URL}${API_BASE}/auth/register`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify({ name, email, password })
		});

		return res.ok;
	} catch (err) {
		console.error('Registration API error:', err);
		return false;
	}
}

/**
 * Changes the authenticated user's password.
 * @param oldPassword - The user's current password.
 * @param newPassword - The desired new password (at least 6 chars long).
 * @returns True on success, false on failure.
 */
export async function changePassword(oldPassword: string, newPassword: string): Promise<boolean> {
	const token = getToken();
	if (!token) {
		console.error('Cannot change password, user is not authenticated.');
		return false;
	}

	try {
		const res = await fetch(`${API_URL}${API_BASE}/auth/change-password`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
				Authorization: `Bearer ${token}`
			},
			body: JSON.stringify({ oldPassword, newPassword })
		});

		toast.show('Password changed successfully!', 'success');

		return res.ok;
	} catch (err) {
		console.error('Change password API error:', err);
		return false;
	}
}

/**
 * Regenerates the user's API key.
 * @returns The new raw API key string, or an empty string on failure.
 */
export async function regenerateApiKey(): Promise<string> {
	const token = getToken();
	if (!token) {
		console.error('Cannot regenerate API key, user is not authenticated.');
		return '';
	}
	try {
		const res = await fetch(`${API_URL}${API_BASE}/apikey/regenerate`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
				Authorization: `Bearer ${token}`
			}
		});
		if (!res.ok) {
			console.error('Failed to regenerate API key:', res.statusText);
			return '';
		}
		const data = await res.json();
		return data.rawApiKey || '';
	} catch (err) {
		console.error('Regenerate API key error:', err);
		return '';
	}
}

/**
 * Gets metadata about the user's API key, including the partial key.
 * @returns The partial key string, or an empty string on failure or if no key exists.
 */
export async function getPartialApiKey(): Promise<string> {
	const token = getToken();
	if (!token) {
		console.error('Cannot get API key, user is not authenticated.');
		return '';
	}
	try {
		const res = await fetch(`${API_URL}${API_BASE}/apikey`, {
			method: 'GET',
			headers: {
				Authorization: `Bearer ${token}`
			}
		});
		if (!res.ok) {
			if (res.status === 404) {
				// No API key exists yet
				return '';
			}
			console.error('Failed to fetch partial API key:', res.statusText);
			return '';
		}
		const data = await res.json();
		return data.partialKey || '';
	} catch (err) {
		console.error('Get partial API key error:', err);
		return '';
	}
}
