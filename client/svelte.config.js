import adapter from '@sveltejs/adapter-auto';
import { vitePreprocess } from '@sveltejs/vite-plugin-svelte';
import { readFileSync } from 'fs';
import { resolve } from 'path';
import { fileURLToPath } from 'url';
import { dirname } from 'path';

const __dirname = dirname(fileURLToPath(import.meta.url));
const isProd = process.env.NODE_ENV === 'production';

/** @type {import('@sveltejs/kit').Config} */
const config = {
	// Consult https://svelte.dev/docs/kit/integrations
	// for more information about preprocessors
	preprocess: vitePreprocess({
		style: {
			// Read the SCSS variables file and prepend it to every style block
			prependData: readFileSync(resolve(__dirname, './src/styles/variables.scss'), 'utf-8')
		}
	}),

	kit: {
		// adapter-auto only supports some environments, see https://svelte.dev/docs/kit/adapter-auto for a list.
		// If your environment is not supported, or you settled on a specific environment, switch out the adapter.
		// See https://svelte.dev/docs/kit/adapters for more information about adapters.
		adapter: adapter(),
		alias: {
			$components: './src/components',
			$utils: './src/utils',
			$styles: './src/styles',
			$assets: './src/assets',
			$lib: './src/lib',
			$layouts: './src/layouts',
			$scripts: './src/scripts',
			$types: './src/lib/types'
		},
		paths: {
			base: isProd ? '/engage' : ''
		}
	}
};

export default config;
