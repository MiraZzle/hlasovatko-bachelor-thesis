<script lang="ts">
	export let value: string = '';
	export let placeholder: string = '';
	export let rows: number = 3;
	export let cols: number = 50;
	export let disabled: boolean = false;
	export let textAlign: 'left' | 'center' | 'right' = 'left';
	export let borderColor: string = 'gray';

	$: styleAttr = `text-align: ${textAlign}; border-color: ${borderColor};`;
</script>

<textarea
	class="text-area__input"
	bind:value
	{placeholder}
	{rows}
	{cols}
	{disabled}
	style={styleAttr}
>
</textarea>

<style lang="scss">
	.text-area__input {
		width: 100%;
		padding: 0.5rem;
		border: 1px solid;
		border-radius: 4px;
		font-size: 1rem;
		resize: vertical;
		transition:
			border-color 0.2s ease,
			box-shadow 0.2s ease;

		&:focus {
			outline: none;
			box-shadow: 0 0 0 3px rgba(#007bff, 0.2);
		}
	}
</style>
