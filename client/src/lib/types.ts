export type QuizAnswer = {
	sessionId: string;
	activityId: string;
	questionId: string;
	selectedOption: string;
};

export type Question = {
	questionId: string;
	question: string;
	options: string[];
	correctOptionIndex: number;
};

export type Activity = {
	activityId: string;
	activityType: string;
	title: string;
	questions: Question[]; // Only available if it's a quiz
};

export type Session = {
	sessionId: string;
	title: string;
	activities: Activity[];
};
