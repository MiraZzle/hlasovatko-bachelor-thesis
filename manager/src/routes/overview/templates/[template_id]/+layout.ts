import { getTemplateById } from '$lib/templates/template_utils';
import type { LayoutLoad } from './$types';

export const load: LayoutLoad = async ({ params, fetch }) => {
	const templateData = getTemplateById(params.template_id);

	if (!templateData) {
		return {
			status: 404,
			error: new Error('Template not found')
		};
	}

	return {
		templateDataForTopBar: {
			id: templateData.id,
			title: templateData.settings?.title
		}
	};
};
