<script lang="ts">
	import AccordionItem from '$components/elements/typography/utils/AccordionItem.svelte';
</script>

<section class="features-section">
	<div class="page-container features-section__container">
		<h2 class="features-section__title">Designed for Every Type of Classroom Interaction</h2>
		<p class="features-section__description">
			Hlasovátko provides multiple interactive formats to engage students and enhance learning
			outcomes.
		</p>

		<div class="features-section__accordion-group">
			<AccordionItem isOpen={true}>
				<span slot="question">Quizzes</span>
				<div slot="answer">
					<p>
						Signing up is simple: just visit our homepage and click on the 'Sign Up' button. Fill in
						the required information, and you'll be ready to start using Hlasovátko.
					</p>
				</div>
			</AccordionItem>

			<AccordionItem>
				<span slot="question">Polls & Surveys</span>
				<div slot="answer">
					<p>Engage your audience with quick polls or gather detailed feedback through surveys.</p>
				</div>
			</AccordionItem>

			<AccordionItem>
				<span slot="question">Live Q&A</span>
				<div slot="answer">
					<p>
						Allow students to ask questions anonymously or identified, vote on questions, and manage
						discussions effectively.
					</p>
				</div>
			</AccordionItem>

			<AccordionItem>
				<span slot="question">Timed Challenges</span>
				<div slot="answer">
					<p>Add excitement with time-limited quizzes and activities to test quick thinking.</p>
				</div>
			</AccordionItem>

			<AccordionItem>
				<span slot="question">Pre-Lecture Questions</span>
				<div slot="answer">
					<p>Gauge prior knowledge or set the stage for a topic before the lecture begins.</p>
				</div>
			</AccordionItem>
		</div>
	</div>
</section>

<style lang="scss">
	@import '../../styles/variables.scss';

	.features-section {
		background-color: $color-surface;
		padding: $spacing-3xl 0;

		&__container {
			display: flex;
			flex-direction: column;
			align-items: center; // Center title/description
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
			max-width: 800px; // Limit width of the accordion section
			border-top: $border-width-thin solid $color-border-light; // Add top border
		}
	}
</style>
