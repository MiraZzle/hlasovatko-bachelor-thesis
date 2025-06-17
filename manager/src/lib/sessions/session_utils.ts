import type { Session, SessionMetrics } from './types';

export function getAllSessions() {}

/*
Returns basic info about session
*/
export function getSessionById(sessionID: string): Session | null {
	console.log(`Fetching session with ID: ${sessionID}`);

	return {
		id: sessionID,
		title: 'Sample Session',
		templateID: 'template123',
		templateVersion: '1',
		status: 'Active',
		created: new Date().toISOString(),
		joinCode: 'ABC123'
	};
}

/*
Returns metrics about session
*/
export function getSessionMetrics(sessionID: string): SessionMetrics {
	console.log(`Fetching metrics for session with ID: ${sessionID}`);

	return {
		participants: 28,
		activitiesRun: 3,
		answersReceived: 15
	};
}
