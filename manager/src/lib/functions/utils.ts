/*
 * Formats date string to 'MM/DD/YYYY'.
 * Returns 'Invalid Date' if the date string is not valid.
 */
export function formatDate(dateString: string): string {
	try {
		return new Date(dateString).toLocaleDateString(undefined, {
			year: 'numeric',
			month: '2-digit',
			day: '2-digit'
		});
	} catch (e) {
		console.error('Error formatting date:', dateString, e);
		return 'Invalid Date';
	}
}

/**
 * Generates initials from a given name.
 * Takes the first two parts of the name and returns their uppercase initials.
 * If the name is empty or has no parts, returns an empty string.
 */
export function getInitials(name: string): string {
	const parts = name.trim().split(' ');
	return parts
		.slice(0, 2)
		.map((p) => p[0].toUpperCase())
		.join('');
}
