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
	tags?: string[];
}

export interface ActivityResponse {
	id: string;
	title: string;
	activityType: string;
	definition: object;
	tags: string[];
}

export interface NewActivityData {
	title: string;
	type: StaticActivityType;
	definition: object;
	tags: string[];
}

export interface ActivityType {
	name: string;
}
export interface ActivityResult {
	activityRef: Activity;
	baseActivityType?: string;
	results: unknown[];
}
