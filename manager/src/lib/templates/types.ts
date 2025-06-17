export interface TemplateStub {
	id: string;
	title: string;
}

export interface Template {
	id: string;
	title: string;
	code: string;
	dateCreated: string;
	status: string;
	tags: string[];
}
