import type { TimeFrame } from './types';

export function getSupportedTimeFrames() {}

export function getSupportedExportFormats() {}

export function exportAnalyticsByFormat() {}

export function getAnalyticsData() {}

export function getTotalResponses(timeFrame: TimeFrame): number {
	return 123;
}
export function getMostPopularActivityType(timeFrame: TimeFrame): string {
	return 'Poll';
}
export function getTotalNumberOfSessions(timeFrame: TimeFrame): number {
	return 10;
}
