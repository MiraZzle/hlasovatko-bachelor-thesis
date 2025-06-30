import type { SessionMode } from '$lib/shared_types';
import type { Template } from '$lib/templates/types';

export type SessionStatus = 'Inactive' | 'Active' | 'Finished' | 'Planned';

export interface Session {
	id: string;
	title: string;
	templateID: string;
	templateVersion?: string;
	status: SessionStatus;
	created: string;
	joinCode?: string;
	participants?: number;
	activationDate?: string;
	mode?: SessionMode;
	currentActivity?: number;
	templateDefinition?: Template;
}
