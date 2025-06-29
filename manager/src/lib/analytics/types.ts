export type ExportFormat = 'csv' | 'json';

export interface Statistics {
	totalSessions: number;
	totalActivities: number;
	mostCommonActivityType: string;
}
