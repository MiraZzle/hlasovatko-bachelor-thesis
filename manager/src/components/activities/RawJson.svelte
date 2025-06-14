<script lang="ts">
	//!TODO: Add type definitions for the JSON structure
	let { definition }: { definition: any } = $props();

	/*
	 * Formats the JSON string to a more readable format with indentation.
	 * If the JSON is invalid, it returns an error message.
	 *
	 * @param json - The JSON string to format.
	 * @returns Formatted JSON string or an error message if invalid.
	 */
	function formatJson(json: string): string {
		try {
			return JSON.stringify(JSON.parse(json), null, 2);
		} catch (e) {
			return 'Invalid JSON format.';
		}
	}
	let formattedJson = $derived(formatJson(definition));
</script>

<div class="activity-display activity-display--raw-json">
	<pre class="activity-display__pre"><code class="activity-display__code">{formattedJson}</code
		></pre>
</div>

<style lang="scss">
	.activity-display {
		margin-top: $spacing-sm;

		&__pre {
			background-color: $color-surface-alt;
			padding: $spacing-md;
			border-radius: $border-radius-md;
			overflow-x: auto;
			max-height: 200px;
		}

		&__code {
			font-family: monospace;
			font-size: $font-size-xs;
			color: $color-text-primary;
		}
	}
</style>
