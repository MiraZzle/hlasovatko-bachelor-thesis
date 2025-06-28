import type { StaticActivityType } from '$lib/activities/types';
export interface MultipleChoiceDefinition {
	options: { id: string; text: string }[];
	correctOptionId?: string | string[];
	allowMultipleAnswers?: boolean;
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

export type SubmitPayload = {
	activityId: string;
	value: string | string[] | number | null;
	activityType: StaticActivityType;
};
