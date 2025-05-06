<script lang="ts">
	let { definition }: { definition: any } = $props();

	// Pretty-print the JSON

	function formatJson(json: string) {
		try {
			return JSON.stringify(JSON.parse(json), null, 2); // Pretty-print JSON
		} catch (e) {
			return 'Invalid JSON format.';
		}
	}
	let formattedJson = $derived(formatJson(definition));
</script>

<div class="activity-display activity-display--raw-json">
	<pre><code>{formattedJson}</code></pre>
</div>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

	.activity-display {
		margin-top: $spacing-sm;
		pre {
			background-color: $color-surface-alt;
			padding: $spacing-md;
			border-radius: $border-radius-md;
			overflow-x: auto; // Allow horizontal scroll for long lines
			max-height: 200px; // Limit height
			code {
				font-family: monospace;
				font-size: $font-size-xs;
				color: $color-text-primary;
			}
		}
	}
</style>
