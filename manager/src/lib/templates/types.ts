import type { SessionMode } from '$lib/shared_types';
import type { Activity } from '$lib/activities/types';

export interface TemplateStub {
	id: string;
	title: string;
}

export interface Template {
	id: string;
	definition: Activity[];
	ownerId: string;
	version: number;
	dateCreated?: string;
	settings?: TemplateSettingsDTO;
}

export interface TemplateSettingsDTO {
	sessionPacing: SessionMode;
	resultsVisibleDefault: boolean;
	title: string;
	tags: string[];
}
