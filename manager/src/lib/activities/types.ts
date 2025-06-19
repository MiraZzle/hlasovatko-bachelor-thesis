export type StaticActivityType =
	| 'multiple_choice'
	| 'poll'
	| 'scale_rating'
	| 'open_ended'
	| 'custom_activity';

export interface Activity {
	id: string;
	type: StaticActivityType;
	title: string;
	definition: object;
}

export interface PredefinedActivity {
	id: string;
	refActivity: Activity;
	categories: string[];
}

export interface NewActivityData {
	title: string;
	type: StaticActivityType;
	definition: object;
	categories: string[];
}

export interface ActivityType {
	name: string;
}
export interface ActivityResult {
	activityRef: Activity;
	baseActivityType?: string;
	results: unknown[];
}
