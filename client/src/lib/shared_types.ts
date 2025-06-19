export interface SelectOption {
	value: string | number;
	label: string;
}

export type SessionMode = 'teacher-paced' | 'student-paced';

export interface ActivityOption {
	id: string;
	text: string;
}
