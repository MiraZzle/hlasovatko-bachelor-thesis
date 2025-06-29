import type { Template, TemplateSettingsDTO } from '$lib/templates/types';
import type { Activity } from '$lib/activities/types';
import { getToken } from '$lib/auth/auth';
import { API_URL } from '$lib/config';
import type { SessionMode } from '$lib/shared_types';
import type { StaticActivityType } from '$lib/activities/types';

/**
 * Defines the shape of the Template object as it comes from the backend API.
 */
interface TemplateResponse {
	id: string;
	title: string;
	tags: string[];
	sessionPacing: SessionMode;
	resultsVisibleDefault: boolean;
	dateCreated: string;
	definition: ActivityResponse[];
}

/**
 * Defines the shape of an Activity within a Template response.
 */
interface ActivityResponse {
	id: string;
	title: string;
	activityType: string;
	definition: object;
	tags: string[];
}

/**
 * Maps a TemplateResponse from the backend to the frontend Template type.
 * @param t - The template response object from the API.
 * @returns A frontend-compatible Template object.
 */
function mapResponseToTemplate(t: TemplateResponse): Template {
	return {
		id: t.id,
		ownerId: '', // Not provided by the API, can be added if needed
		version: 1, // Not provided by the API, can be added if needed
		dateCreated: t.dateCreated,
		settings: {
			title: t.title,
			tags: t.tags,
			sessionPacing: t.sessionPacing,
			resultsVisibleDefault: t.resultsVisibleDefault
		},
		definition: t.definition
			? t.definition.map((act) => ({
					id: act.id,
					title: act.title,
					type: act.activityType as StaticActivityType,
					definition: act.definition,
					tags: act.tags
				}))
			: []
	};
}

/**
 * Updates an entire template, including its settings and full definition.
 * @param templateId The ID of the template to update.
 * @param template The full template object with the new data.
 * @returns The updated template object, or null on failure.
 */
export async function updateTemplate(
	templateId: string,
	template: Template
): Promise<Template | null> {
	const token = getToken();
	if (!token) return null;

	// Map the frontend Template object to the backend's UpdateTemplateDto shape
	const updateDto = {
		settings: template.settings,
		definition: template.definition.map((act) => ({
			title: act.title,
			activityType: act.type,
			definition: JSON.stringify(act.definition),
			tags: act.tags || []
		}))
	};

	try {
		const res = await fetch(`${API_URL}/api/v1/template/${templateId}`, {
			method: 'PUT',
			headers: {
				'Content-Type': 'application/json',
				Authorization: `Bearer ${token}`
			},
			body: JSON.stringify(updateDto)
		});

		if (!res.ok) {
			console.error(`Failed to update template ${templateId}:`, res.statusText);
			return null;
		}

		const updatedTemplate: TemplateResponse = await res.json();
		return mapResponseToTemplate(updatedTemplate);
	} catch (err) {
		console.error('Update template API error:', err);
		return null;
	}
}

/**
 * Fetches all templates created by the logged-in user.
 * @returns A promise that resolves to an array of simplified template objects.
 */
export async function getAllTemplates(): Promise<Template[]> {
	const token = getToken();
	if (!token) return [];

	try {
		const res = await fetch(`${API_URL}/api/v1/template`, {
			method: 'GET',
			headers: {
				Authorization: `Bearer ${token}`
			}
		});

		if (!res.ok) {
			console.error('Failed to fetch templates:', res.statusText);
			return [];
		}

		const templates: TemplateResponse[] = await res.json();
		return templates.map(mapResponseToTemplate);
	} catch (err) {
		console.error('Get all templates API error:', err);
		return [];
	}
}

/**
 * Fetches a single, complete template by its ID.
 * @param templateId The ID of the template to fetch.
 * @returns A promise that resolves to the template object, or null if not found.
 */
export async function getTemplateById(templateId: string): Promise<Template | null> {
	const token = getToken();
	if (!token) return null;

	try {
		const res = await fetch(`${API_URL}/api/v1/template/${templateId}`, {
			method: 'GET',
			headers: {
				Authorization: `Bearer ${token}`
			}
		});

		if (!res.ok) {
			console.error(`Failed to fetch template ${templateId}:`, res.statusText);
			return null;
		}

		const templateData: TemplateResponse = await res.json();
		return mapResponseToTemplate(templateData);
	} catch (err) {
		console.error(`Get template by ID API error:`, err);
		return null;
	}
}

/**
 * Creates a new template.
 * @param settings The settings for the new template.
 * @param activities A list of full activity objects to be included in the template.
 * @returns The newly created template object, or null on failure.
 */
export async function createNewTemplate(
	settings: TemplateSettingsDTO,
	activities: Activity[]
): Promise<Template | null> {
	const token = getToken();
	if (!token) return null;

	const activityDtos = activities.map((act) => ({
		title: act.title,
		activityType: act.type,
		definition: JSON.stringify(act.definition),
		tags: act.tags || []
	}));

	try {
		const res = await fetch(`${API_URL}/api/v1/template`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
				Authorization: `Bearer ${token}`
			},
			body: JSON.stringify({
				title: settings.title,
				tags: settings.tags,
				sessionPacing: settings.sessionPacing,
				resultsVisibleDefault: settings.resultsVisibleDefault,
				activities: activityDtos
			})
		});

		if (!res.ok) {
			console.error('Failed to create new template:', res.statusText);
			return null;
		}

		const newTemplate: TemplateResponse = await res.json();
		return mapResponseToTemplate(newTemplate);
	} catch (err) {
		console.error('Create template API error:', err);
		return null;
	}
}

/**
 * Updates the settings of an existing template.
 * @param templateId The ID of the template to update.
 * @param settings The new settings for the template.
 * @returns The updated template object, or null on failure.
 */
export async function updateTemplateSettings(
	templateId: string,
	settings: TemplateSettingsDTO
): Promise<Template | null> {
	const token = getToken();
	if (!token) return null;

	try {
		const res = await fetch(`${API_URL}/api/v1/template/${templateId}/settings`, {
			method: 'PUT',
			headers: {
				'Content-Type': 'application/json',
				Authorization: `Bearer ${token}`
			},
			body: JSON.stringify(settings)
		});

		if (!res.ok) {
			console.error(`Failed to update settings for template ${templateId}:`, res.statusText);
			return null;
		}

		const updatedTemplate: TemplateResponse = await res.json();
		return mapResponseToTemplate(updatedTemplate);
	} catch (err) {
		console.error('Update template settings API error:', err);
		return null;
	}
}

/**
 * Deletes a template by its ID.
 * @param templateId The ID of the template to delete.
 * @returns True on success, false otherwise.
 */
export async function deleteTemplate(templateId: string): Promise<boolean> {
	const token = getToken();
	if (!token) return false;

	try {
		const res = await fetch(`${API_URL}/api/v1/template/${templateId}`, {
			method: 'DELETE',
			headers: {
				Authorization: `Bearer ${token}`
			}
		});

		return res.ok;
	} catch (err) {
		console.error('Delete template API error:', err);
		return false;
	}
}
