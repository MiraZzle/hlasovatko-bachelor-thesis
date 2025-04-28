// src/lib/types.ts (Example)
export interface Template {
	id: string;
	title: string;
	code: string;
	dateCreated: string; // Or Date object if you parse it earlier
	status: 'Active' | 'Inactive'; // Use specific types if known
	tags: string[];
}
