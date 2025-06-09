/*
Signout user and clear local storage
*/
export function signout() {
	localStorage.removeItem('token');
	localStorage.removeItem('user');
	window.location.href = '/';
}

export function isAuthenticated(): boolean {
	const token = localStorage.getItem('token');
	return token !== null && token !== '';
}

export function login(username: string, password: string): boolean {
	// Simulate a login process
	if (username === 'admin' && password === 'password') {
		localStorage.setItem('token', 'fake-jwt-token');
		localStorage.setItem('user', JSON.stringify({ username: 'admin' }));
		return true;
	}
	return false;
}
