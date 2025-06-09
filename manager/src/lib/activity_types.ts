export interface ActivityOption {
	id: string;
	text: string;
}

export interface MultipleChoiceDefinition {
	type: 'MultipleChoice';
	options: ActivityOption[];
	correctOptionId?: string | string[];
	allowMultiple?: boolean;
}

export interface PollDefinition {
	type: 'Poll';
	options: ActivityOption[];
}

export interface ScaleRatingDefinition {
	type: 'ScaleRating';
	min: number;
	max: number;
	minLabel?: string;
	maxLabel?: string;
}

export interface OpenEndedDefinition {
	type: 'OpenEnded';
}

export type KnownActivityDefinition =
	| MultipleChoiceDefinition
	| PollDefinition
	| ScaleRatingDefinition
	| OpenEndedDefinition;

export interface UnknownDefinition {
	type: string;
}

export interface Session {
	id: string;
	title: string;
	templateCode: string;
	created: string;
	status: 'Active' | 'Inactive' | 'Finished';
	participants: number;
}

export interface Template {
	id: string;
	title: string;
	code: string;
	dateCreated: string;
	status: string; // Keep as string if statuses vary widely
	tags: string[];
}

export type SessionActivityStatus = 'Pending' | 'Active' | 'Closed';
export type KnownActivityType = KnownActivityDefinition['type'];

export interface SessionActivity {
	id: string;
	templateActivityId?: string;
	type: KnownActivityType | string;
	title: string;
	definition: KnownActivityDefinition | UnknownDefinition | object;
	status?: SessionActivityStatus;
	order?: number;
}

export function isMultipleChoice(def: unknown): def is MultipleChoiceDefinition {
	return (
		typeof def === 'object' &&
		def !== null &&
		(def as MultipleChoiceDefinition).type === 'MultipleChoice' &&
		Array.isArray((def as MultipleChoiceDefinition).options) &&
		(def as MultipleChoiceDefinition).options.every(
			(o) =>
				typeof o === 'object' &&
				o !== null &&
				typeof o.id === 'string' &&
				typeof o.text === 'string'
		)
	);
}

export function isPoll(def: unknown): def is PollDefinition {
	return (
		typeof def === 'object' &&
		def !== null &&
		(def as PollDefinition).type === 'Poll' &&
		Array.isArray((def as PollDefinition).options) &&
		(def as PollDefinition).options.every(
			(o) =>
				typeof o === 'object' &&
				o !== null &&
				typeof o.id === 'string' &&
				typeof o.text === 'string'
		)
	);
}

export function isScaleRating(def: unknown): def is ScaleRatingDefinition {
	return (
		typeof def === 'object' &&
		def !== null &&
		(def as ScaleRatingDefinition).type === 'ScaleRating' &&
		typeof (def as ScaleRatingDefinition).min === 'number' &&
		typeof (def as ScaleRatingDefinition).max === 'number'
	);
}

export function isOpenEnded(def: unknown): def is OpenEndedDefinition {
	return (
		typeof def === 'object' && def !== null && (def as OpenEndedDefinition).type === 'OpenEnded'
	);
}

export function getKnownDefinition(activity: SessionActivity): KnownActivityDefinition | null {
	const def = activity.definition;
	if (typeof def !== 'object' || def === null) return null;

	switch (activity.type) {
		case 'MultipleChoice':
			return isMultipleChoice(def) ? def : null;
		case 'Poll':
			return isPoll(def) ? def : null;
		case 'ScaleRating':
			return isScaleRating(def) ? def : null;
		case 'OpenEnded':
			return isOpenEnded(def) ? def : null;
		default:
			return null;
	}
}
export interface OptionResult {
	id: string;
	text: string;
	count: number;
	percentage?: number;
	isCorrect?: boolean;
}
export type ChoiceActivityResult = OptionResult[];

export interface RatingResult {
	rating: number;
	count: number;
	percentage?: number;
}
export type ScaleRatingActivityResult = RatingResult[];
export type OpenEndedActivityResult = string[];

export interface SessionActivity {
	id: string;
	templateActivityId?: string;
	type: KnownActivityType | string;
	title: string;
	definition: KnownActivityDefinition | UnknownDefinition | object;
	status?: SessionActivityStatus;
	order?: number;
	results?: ChoiceActivityResult | ScaleRatingActivityResult | OpenEndedActivityResult | unknown;
	participantCount?: number;
	responseCount?: number;
}

export function isChoiceResult(results: unknown): results is ChoiceActivityResult {
	return (
		Array.isArray(results) &&
		results.every(
			(r) =>
				typeof r === 'object' &&
				r !== null &&
				typeof r.id === 'string' &&
				typeof r.text === 'string' &&
				typeof r.count === 'number'
		)
	);
}

export function isScaleRatingResult(results: unknown): results is ScaleRatingActivityResult {
	return (
		Array.isArray(results) &&
		results.every(
			(r) =>
				typeof r === 'object' &&
				r !== null &&
				typeof r.rating === 'number' &&
				typeof r.count === 'number'
		)
	);
}

export function isOpenEndedResult(results: unknown): results is OpenEndedActivityResult {
	return Array.isArray(results) && results.every((r) => typeof r === 'string');
}
