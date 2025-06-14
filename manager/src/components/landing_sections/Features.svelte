<script lang="ts">
	import AccordionItem from '$components/elements/typography/utils/AccordionItem.svelte';
</script>

<section class="features-section" id="features">
	<div class="page-container features-section__container">
		<h2 class="features-section__title">Designed for Every Type of Classroom Interaction</h2>
		<p class="features-section__description">
			EngaGenie provides multiple interactive formats to engage students and enhance learning
			outcomes.
		</p>

		<div class="features-section__accordion-group">
			<AccordionItem isOpen={true}>
				<span slot="question">Multiple Choice</span>
				<div slot="answer">
					<p>
						Ask single- or multiple-answer questions to check understanding in real-time. Ideal for
						concept checks, quick assessments, or sparking discussion.
					</p>
				</div>
			</AccordionItem>

			<AccordionItem>
				<span slot="question">Polls and Opinions</span>
				<div slot="answer">
					<p>
						Quickly collect opinions or check understanding using simple polls. Choose from
						predefined options or allow open-ended voting.
					</p>
				</div>
			</AccordionItem>

			<AccordionItem>
				<span slot="question">Live Rating Scales</span>
				<div slot="answer">
					<p>
						Let students rate topics, confidence, or preferences on a numeric or Likert scale. Ideal
						for feedback or gauging topic difficulty.
					</p>
				</div>
			</AccordionItem>

			<AccordionItem>
				<span slot="question">Open-ended Reflections</span>
				<div slot="answer">
					<p>
						Collect qualitative feedback or critical thinking responses from students. Support
						pre-lecture prompts or follow-up reflections.
					</p>
				</div>
			</AccordionItem>

			<AccordionItem>
				<span slot="question">Session-based Control for Teachers</span>
				<div slot="answer">
					<p>
						Manage a sequence of activities in real-time. Control pacing, activate or skip
						questions, and monitor participation live without needing to log in again.
					</p>
				</div>
			</AccordionItem>
		</div>
	</div>
</section>

<style lang="scss">
	.features-section {
		background-color: $color-surface;
		padding: $spacing-3xl 0;

		&__container {
			display: flex;
			flex-direction: column;
			align-items: center;
		}

		&__title {
			text-align: center;
			font-size: $font-size-3xl;
			margin-bottom: $spacing-sm;
		}

		&__description {
			text-align: center;
			font-size: $font-size-lg;
			color: $color-text-secondary;
			max-width: 700px;
			margin-bottom: $spacing-2xl;
		}

		&__accordion-group {
			width: 100%;
			max-width: 800px;
			border-top: $border-width-thin solid $color-border-light;
		}
	}
</style>
