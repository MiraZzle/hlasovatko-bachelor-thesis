export interface Activity {
	id: string;
	type: string;
	title: string;
	definition: string;
}

export interface PredefinedActivity {
	id: string;
	refActivity: Activity;
	categories: string[];
}

export interface NewActivityData {
	title: string;
	type: string;
	definition: string;
	categories: string[];
}

export interface ActivityType {
	name: string;
}
