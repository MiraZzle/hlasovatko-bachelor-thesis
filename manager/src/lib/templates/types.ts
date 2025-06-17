export interface TemplateStub {
	id: string;
	title: string;
}

export interface Template {
	id: string;
	title: string;
	definition: object;
	ownerId: string;
	version: number;
	tags?: string[];
	dateCreated?: string;
}
