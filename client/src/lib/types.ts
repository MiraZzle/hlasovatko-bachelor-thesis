export type Session = {
	sessionId: string;
	title: string;
	activities: Activity[];
};

export type Activity = {
	activityId: string;
	title: string;
	type: 'quiz' | 'poll' | 'qa'; // allowed activity types
	questions: Question[];
};

export type Question = {
	questionId: string;
	question: string;
	options: string[];
};
