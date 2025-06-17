type StaticActivityType = 'multiple_choice' | 'poll' | 'scale_rating' | 'open_ended';

export interface Activity {
	id: string;
	type: string;
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
	type: string;
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
