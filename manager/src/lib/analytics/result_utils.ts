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
