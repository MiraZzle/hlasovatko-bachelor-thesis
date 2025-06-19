<script lang="ts">
	let {
		title = 'Benefit Title',
		description = 'This is a brief description of the benefit.',
		icon = '⭐'
	}: {
		title?: string;
		description?: string;
		icon?: string;
	} = $props();
</script>

<div class="benefit-card">
	<div class="benefit-card__icon">
		<span class="benefit-card__icon">{icon}</span>
	</div>
	<h3 class="benefit-card__title">{title}</h3>
	<p class="benefit-card__description">{description}</p>
</div>

<style lang="scss">
	.benefit-card {
		background-color: $color-surface;
		padding: $spacing-xl;
		border-radius: $border-radius-lg;
		box-shadow: $box-shadow-sm;
		text-align: center;
		transition:
			transform 0.2s ease,
			box-shadow 0.2s ease;

		&:hover {
			transform: translateY(-4px);
			box-shadow: $box-shadow-md;
		}

		&__icon {
			font-size: $font-size-3xl;
			margin-bottom: $spacing-sm;
			color: $color-primary;
			display: inline-block;
		}

		&__title {
			font-size: $font-size-xl;
			font-weight: $font-weight-semibold;
			margin-bottom: $spacing-sm;
			color: $color-text-primary;
		}

		&__description {
			font-size: $font-size-md;
			color: $color-text-secondary;
			line-height: $line-height-base;
			margin-bottom: 0;
		}
	}
</style>
