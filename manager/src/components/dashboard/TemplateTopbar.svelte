<script lang="ts">
	/**
	 * @file Reusable component for displaying a template header in the dashboard.
	 * Renders the title of the template with appropriate styling.
	 */
	let {
		templateId,
		templateTitle
	}: {
		templateId: string;
		templateTitle: string;
	} = $props();
</script>

<h1 class="template-header__title">{templateTitle}</h1>

<style lang="scss">
	.template-header__title {
		font-size: $font-size-lg;
		font-weight: $font-weight-semibold;
		color: $color-text-primary;
		margin: 0;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
		flex-shrink: 1;
		margin-right: $spacing-sm;
	}
</style>
