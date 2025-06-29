/**
 * Retrieves the JWT from localStorage.
 * @returns The token string, or null if it doesn't exist.
 */
export function getToken(): string | null {
	return localStorage.getItem('token');
}
