<script lang="ts">
	import Textarea from '$components/elements/typography/utils/TextArea.svelte';

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
		maxLength = 500
	}: Props = $props();
</script>

<div class="open-ended-input">
	<Textarea bind:value {placeholder} {disabled} rows={5} ariaLabel="Your answer" />
	{#if maxLength}
		<div class="open-ended-input__char-count">
			{value.length} / {maxLength}
		</div>
	{/if}
</div>

<style lang="scss">
	@import '../../styles/variables.scss'; // Adjust path

	.open-ended-input {
		width: 100%;
		position: relative; // For char count

		:global(.textarea-wrapper__textarea) {
			// Style the textarea from imported component
			font-size: $font-size-lg; // Make text larger for answers
			line-height: 1.5;
		}

		&__char-count {
			text-align: right;
			font-size: $font-size-xs;
			color: $color-text-secondary;
			margin-top: $spacing-xs;
		}
	}
</style>
