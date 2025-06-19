import type { SessionMode } from '$lib/shared_types';
import type { Template } from '$lib/templates/types';

export interface Session {
	id: string;
	title: string;
	templateID: string;
	templateVersion?: string;
	status: 'Active' | 'Planned' | 'Finished' | 'Inactive';
	created: string;
	joinCode?: string;
	participants?: number;
	activationDate?: string;
	mode?: SessionMode;
	currentActivity?: number;
	templateDefinition?: Template;
}

export interface SessionMetrics {
	participants: number;
	activitiesRun: number;
	answersReceived: number;
}
