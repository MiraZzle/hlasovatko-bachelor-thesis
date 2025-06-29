import type { SessionMode } from '$lib/shared_types';
import type { Template } from '$lib/templates/types';

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

export type SessionStatus = 'Active' | 'Planned' | 'Finished' | 'Inactive';

export interface SessionSettings {
	mode: SessionMode;
	allowParticipantAnswers: boolean;
	allowAnswerReveal: boolean;
	allowNextActivity: boolean;
}

export interface SessionJoinInfo {
	id: string;
	title: string;
	mode: SessionMode;
}
