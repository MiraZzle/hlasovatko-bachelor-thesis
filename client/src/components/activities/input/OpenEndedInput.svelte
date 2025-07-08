<script lang="ts">
	/**
	 * @file A reusable component for open-ended text input.
	 * It provides a textarea with a character counter.
	 */

	import TextArea from '$components/elements/typography/utils/TextArea.svelte';

	/**
	 * The properties accepted by the component.
	 */
	type Props = {
		value?: string;
		placeholder?: string;
		disabled?: boolean;
		maxLength?: number;
	};

	let {
		value = $bindable(''),
		placeholder = 'Type your answer here...',
		disabled = false,
		maxLength = 300
	}: Props = $props();

	const currentAnswerLength = $derived(value?.length ?? 0);
</script>

<div class="open-ended-input">
	<TextArea bind:value {placeholder} {disabled} rows={6} />
	{#if maxLength}
		<div class="open-ended-input__char-count" class:over-limit={currentAnswerLength > maxLength}>
			{currentAnswerLength} / {maxLength}
		</div>
	{/if}
</div>

<style lang="scss">
	.open-ended-input {
		width: 100%;
		position: relative;

		&__char-count {
			text-align: right;
			font-size: $font-size-xs;
			color: $color-text-secondary;
			margin-top: $spacing-sm;
			padding-right: $spacing-xs;

			&.over-limit {
				color: $color-danger;
				font-weight: 500;
			}
		}
	}
</style>
