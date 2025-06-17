export interface Session {
	id: string;
	title: string;
	templateID: string;
	templateVersion?: string;
	status: 'Active' | 'Planned' | 'Finished' | 'Inactive';
	created: string;
	joinCode?: string;
	participants?: number;
}

export interface SessionMetrics {
	participants: number;
	activitiesRun: number;
	answersReceived: number;
}
