import type { SessionMode } from '$lib/shared_types';

export interface TemplateStub {
	id: string;
	title: string;
}

export interface Template {
	id: string;
	title: string;
	definition: object;
	ownerId: string;
	version: number;
	tags?: string[];
	dateCreated?: string;
	defaultPace?: SessionMode;
	resultsVisible?: boolean;
}

export interface TemplateSettingsDTO {
	sessionPacing: SessionMode;
	resultsVisibleDefault: boolean;
	title: string;
	tags: string[];
}
