// src/lib/types.ts (or similar)

// --- Specific Definition Interfaces ---

// Interface for options used in MultipleChoice and Poll
export interface ActivityOption {
	id: string;
	text: string;
}

export interface MultipleChoiceDefinition {
	type: 'MultipleChoice'; // Discriminant property
	options: ActivityOption[];
	correctOptionId?: string | string[]; // Optional: single ID for single choice, array for multiple choice
	allowMultiple?: boolean;
}

export interface PollDefinition {
	type: 'Poll'; // Discriminant property
	options: ActivityOption[];
}

export interface ScaleRatingDefinition {
	type: 'ScaleRating'; // Discriminant property
	min: number;
	max: number;
	minLabel?: string;
	maxLabel?: string;
}

export interface OpenEndedDefinition {
	type: 'OpenEnded'; // Discriminant property
	// Potentially add max length, etc.
	// No other specific fields needed for now.
}

// Union type for all possible known activity definitions
export type KnownActivityDefinition =
	| MultipleChoiceDefinition
	| PollDefinition
	| ScaleRatingDefinition
	| OpenEndedDefinition;

// Type for unknown or custom definitions (can be refined further if needed)
export interface UnknownDefinition {
	type: string; // The unknown type string
}

// --- Main Session Activity Interface ---

// Define possible statuses explicitly
export type SessionActivityStatus = 'Pending' | 'Active' | 'Closed';

// Define possible known types explicitly
export type KnownActivityType = KnownActivityDefinition['type']; // 'MultipleChoice' | 'Poll' | 'ScaleRating' | 'OpenEnded'

export interface SessionActivity {
	id: string; // Unique ID for this instance of the activity within the session
	templateActivityId?: string; // ID of the original template activity, if derived
	type: KnownActivityType | string; // Allow known types or any other string
	title: string; // The question or main prompt
	// Definition can be one of the known types or a generic object for unknown types
	definition: KnownActivityDefinition | UnknownDefinition | object; // Use object as a fallback if parsing fails or type is truly unknown
	status?: SessionActivityStatus; // Status within the session
	order?: number; // Display order
	// resultsSummary?: any; // Define results structure later if needed
}

// --- Updated Helper Type Guard Functions ---

// Note: These now accept 'unknown' which is safer than 'any'
// They check the discriminant 'type' property first for efficiency

export function isMultipleChoice(def: unknown): def is MultipleChoiceDefinition {
	return (
		typeof def === 'object' &&
		def !== null &&
		(def as MultipleChoiceDefinition).type === 'MultipleChoice' && // Check type first
		Array.isArray((def as MultipleChoiceDefinition).options) &&
		(def as MultipleChoiceDefinition).options.every(
			(o) =>
				typeof o === 'object' &&
				o !== null &&
				typeof o.id === 'string' &&
				typeof o.text === 'string'
		)
		// Correctness/allowMultiple are optional, no need to check for type guard
	);
}

export function isPoll(def: unknown): def is PollDefinition {
	return (
		typeof def === 'object' &&
		def !== null &&
		(def as PollDefinition).type === 'Poll' && // Check type first
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
		(def as ScaleRatingDefinition).type === 'ScaleRating' && // Check type first
		typeof (def as ScaleRatingDefinition).min === 'number' &&
		typeof (def as ScaleRatingDefinition).max === 'number'
		// Optional labels don't need checking for the type guard
	);
}

export function isOpenEnded(def: unknown): def is OpenEndedDefinition {
	return (
		typeof def === 'object' && def !== null && (def as OpenEndedDefinition).type === 'OpenEnded'
		// No other fields to check currently
	);
}

// Function to safely get a known definition type or return null/undefined
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
			return null; // Type is not one of the known ones
	}
}
