export interface MultipleChoiceDefinition {
	options: { id: string; text: string }[];
	correctOptionId?: string | string[];
}

export interface PollDefinition {
	options: { id: string; text: string }[];
}

export interface ScaleRatingDefinition {
	min: number;
	max: number;
	minLabel?: string;
	maxLabel?: string;
}

export type OpenEndedActivityResult = string[];
