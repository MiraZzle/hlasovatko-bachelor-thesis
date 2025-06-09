<script lang="ts">
	interface Activity {
		id: string;
		type: string;
		question: string;
		tags: string[];
	}

	let {
		activity,
		onclick = (id: string) => {
			console.log('Activity card clicked:', id);
		}
	}: {
		activity: Activity;
		onclick?: (id: string) => void;
	} = $props();
</script>

<button type="button" class="activity-card" onclick={() => onclick(activity.id)}>
	<div class="activity-card__type">{activity.type}</div>
	<p class="activity-card__question">{activity.question}</p>
	<div class="activity-card__footer">
		<div class="activity-card__tags">
			{#each activity.tags as tag (tag)}
				<span class="activity-card__tag">{tag}</span>
			{/each}
		</div>
	</div>
</button>

<style lang="scss">
	@import '../../styles/variables.scss';

	.activity-card {
		background: none;
		border: none;
		padding: 0;
		margin: 0;
		font: inherit;
		color: inherit;
		cursor: pointer;
		text-align: left;
		width: 100%;
		display: flex;
		flex-direction: column;
		justify-content: space-between;
		background-color: $color-surface;
		border-radius: $border-radius-md;
		padding: $spacing-md;
		box-shadow: $box-shadow-sm;
		border: $border-width-thin solid $color-border-light;
		min-height: 150px;
		transition:
			box-shadow $transition-duration-fast,
			transform $transition-duration-fast;

		&:hover {
			box-shadow: $box-shadow-md;
			transform: translateY(-2px);
		}
		&:focus-visible {
			outline: 2px solid $color-primary-light;
			outline-offset: 2px;
			box-shadow: $box-shadow-md;
		}

		&__type {
			font-size: $font-size-xs;
			font-weight: $font-weight-medium;
			color: $color-text-secondary;
			text-transform: uppercase;
			margin-bottom: $spacing-sm;
		}

		&__question {
			font-size: $font-size-md;
			color: $color-text-primary;
			line-height: 1.4;
			margin-bottom: $spacing-lg;
			white-space: normal;
		}

		&__footer {
			display: flex;
			justify-content: flex-end;
			align-items: center;
			margin-top: auto;
		}

		&__tags {
			display: flex;
			flex-wrap: wrap;
			gap: $spacing-xs;
		}

		&__tag {
			background-color: $color-surface-alt;
			color: $color-text-secondary;
			padding: $spacing-xs * 0.5 $spacing-sm;
			border-radius: $border-radius-sm;
			font-size: $font-size-xs;
			font-weight: $font-weight-medium;
			white-space: nowrap;
		}
	}
</style>
