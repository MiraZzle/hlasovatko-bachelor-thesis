import type { Session } from './types';
import type { SessionSettings } from './types';

export function stopSession(sessionId: string): void {
	console.log(`Stopping session ${sessionId}`);
}

export function getSessionById(sessionID: string): Session | null {
	console.log(`Fetching session with ID: ${sessionID}`);

	return {
		id: sessionID,
		title: 'Sample Session',
		templateID: 'template123',
		templateVersion: '1',
		status: 'Active',
		created: new Date().toISOString(),
		joinCode: 'aaaa'
	};
}

export function getSessionIdByJoinCode(joinCode: string): string | null {
	console.log(`Fetching session ID for join code: ${joinCode}`);
	return 'session123';
}

export function getSessionSettings(sessionId: string): SessionSettings {
	console.log(`Fetching settings for session ${sessionId}`);
	return {
		mode: 'teacher-paced',
		allowParticipantAnswers: true,
		allowAnswerReveal: true,
		allowNextActivity: true
	};
}

export function getCurrentActivity(sessionId: string) {
	console.log(`Fetching current activity for session ${sessionId}`);
	return {
		id: 'sact1',
		type: 'poll',
		title: 'Sample Activity',
		definition: { type: 'Poll', options: [{ id: 'o1', text: 'Option 1' }] }
	};
}
