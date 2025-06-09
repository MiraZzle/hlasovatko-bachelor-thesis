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
