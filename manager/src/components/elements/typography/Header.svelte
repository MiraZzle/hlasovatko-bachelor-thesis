<script lang="ts">
	export let type: 1 | 2 | 3 = 1;
	export let textAlign: 'left' | 'center' | 'right' = 'center';
</script>

<svelte:element this={'h' + type} class="header header--{type}" style={`text-align: ${textAlign}`}>
	<slot />
</svelte:element>

<style lang="scss">
	@import '../../../styles/variables.scss';

	.header {
		margin: 0;
		&--1 {
			font-size: 72px;
			font-weight: 700;
			line-height: 120%;
			letter-spacing: -2px;
		}

		&--2 {
			font-size: 24px;
			font-weight: 600;
			line-height: 120%;
		}

		&--3 {
			font-size: 48px;
			font-weight: 700;
			line-height: 120%;
		}
	}
</style>
