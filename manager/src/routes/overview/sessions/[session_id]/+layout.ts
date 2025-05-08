import type { LayoutLoad } from './$types';

export const load: LayoutLoad = async ({ params, fetch }) => {
	// Get the dynamic session ID from the route parameters
	const sessionId = params.session_id;

	// dummy fetch usage

	// --- Fetch actual session data based on sessionId ---
	// In a real application, you would make an API call here
	// using the 'fetch' provided by SvelteKit for universal fetching.
	// Example:
	// const response = await fetch(`/api/sessions/${sessionId}`); // Using SvelteKit fetch
	// if (!response.ok) {
	//     // Handle error, maybe redirect or show error page using SvelteKit's error helper
	//     throw error(404, 'Session not found');
	// }
	// const sessionDetails = await response.json();

	// --- Using Dummy Data for Demonstration ---
	console.log(`Load function running for session layout: ${sessionId}`);
	await new Promise((resolve) => setTimeout(resolve, 50)); // Simulate network delay
	const dummySessionData = {
		// Generate dummy title based on ID
		title: `Session ${sessionId}`,
		// Generate a random status for demonstration
		status: Math.random() > 0.5 ? ('Active' as const) : ('Inactive' as const)
		// Add other necessary fields fetched from your API
	};
	// Type assertion for status (ensures it matches expected literal types)
	const status: 'Active' | 'Inactive' | 'Finished' = dummySessionData.status;
	// -------------------------------------------

	// Return data to be merged into the $page.data store.
	// The parent layout (overview/+layout.svelte) will check for this specific key.
	return {
		sessionDataForTopbar: {
			// Data needed by the SessionTopbar component
			id: sessionId,
			title: dummySessionData.title, // Use fetched/dummy data
			status: status // Use fetched/dummy data
		}
	};
};
