import { type Session, type SessionMetrics } from './types';
import type { SessionMode } from '$lib/shared_types';

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
		status: 'Inactive',
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

/*
Returns all sessions
*/
export function getAllSessions(): Session[] {
	return [
		{
			id: 's1',
			title: 'Quiz 1 - NDBI046 Spring',
			templateID: `#t41585`,
			created: '2025-04-01T10:00:00Z',
			status: 'Finished',
			participants: 12
		},
		{
			id: 's2',
			title: 'Quiz 1 - NDBI046 Fall',
			templateID: `#t41585`,
			created: '2024-10-15T11:00:00Z',
			status: 'Finished',
			participants: 15
		},
		{
			id: 's3',
			title: 'Poll - Lecture 3 Feedback',
			templateID: `#t41587`,
			created: '2025-04-03T14:30:00Z',
			status: 'Active',
			participants: 28
		},
		{
			id: 's4',
			title: 'Midterm Review Session',
			templateID: `#t41588`,
			created: '2025-04-10T09:00:00Z',
			status: 'Inactive',
			participants: 0
		},
		{
			id: 's5',
			title: 'Quiz 2 - NDBI046 Spring',
			templateID: `#t41586`,
			created: '2025-04-08T10:00:00Z',
			status: 'Finished',
			participants: 11
		}
	];
}

export function getSessionsByTemplate(templateID: string): Session[] {
	console.log(`Fetching sessions for template ID: ${templateID}`);

	return [
		{
			id: 's1',
			title: 'Quiz 1 - NDBI046 Spring',
			templateID: `#t41585`,
			created: '2025-04-01T10:00:00Z',
			status: 'Finished',
			participants: 12
		},
		{
			id: 's2',
			title: 'Quiz 1 - NDBI046 Fall',
			templateID: `#t41585`,
			created: '2024-10-15T11:00:00Z',
			status: 'Finished',
			participants: 15
		},
		{
			id: 's3',
			title: 'Poll - Lecture 3 Feedback',
			templateID: `#t41587`,
			created: '2025-04-03T14:30:00Z',
			status: 'Active',
			participants: 28
		},
		{
			id: 's4',
			title: 'Midterm Review Session',
			templateID: `#t41588`,
			created: '2025-04-10T09:00:00Z',
			status: 'Inactive',
			participants: 0
		},
		{
			id: 's5',
			title: 'Quiz 2 - NDBI046 Spring',
			templateID: `#t41586`,
			created: '2025-04-08T10:00:00Z',
			status: 'Finished',
			participants: 11
		}
	];
}
// src/lib/api/sessions.ts
export async function createNewSession(
	templateId: string,
	title: string,
	activationDate?: string,
	mode?: SessionMode
): Promise<Session> {
	// mock response for testing
	return {
		id: 'new-session-id',
		title,
		templateID: templateId,
		templateVersion: '1.0',
		status: 'Planned',
		created: new Date().toISOString(),
		joinCode: 'NEW123',
		activationDate: activationDate,
		mode: mode
	};
	/*const response = await fetch('/api/sessions', {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json'
		},
		body: JSON.stringify({
			templateId,
			title,
			activationDate
		})
	});

	if (!response.ok) {
		throw new Error('Failed to create session');
	}

	const session: Session = await response.json();
	return session;
	*/
}
