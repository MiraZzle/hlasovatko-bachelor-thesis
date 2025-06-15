export type TimeFrame = '7d' | '30d' | '90d' | 'all';
export type ExportFormat = 'csv' | 'json' | 'xlsx';

export interface AnalyticsData {
	timeFrame: TimeFrame;
	exportFormat: ExportFormat;
}
