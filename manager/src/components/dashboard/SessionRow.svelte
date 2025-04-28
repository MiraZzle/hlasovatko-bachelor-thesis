<script lang="ts">
	// Define Session type (or import)
	interface Session {
		id: string;
		title: string;
		templateCode: string; // Assuming sessions relate to a template code
		created: string;
		status: 'Active' | 'Inactive' | 'Finished'; // Example statuses
		participants: number;
	}

	// Props
	let {
		session,
		onActionClick = (id: string) => {
			console.warn('onActionClick not provided for session', id);
		}
	}: {
		session: Session;
		onActionClick?: (id: string) => void;
	} = $props();

	// Format date
	const formattedDate = $derived(() => {
		try {
			return new Date(session.created).toLocaleDateString(undefined, {
				year: 'numeric',
				month: '2-digit',
				day: '2-digit'
			});
		} catch (e) {
			return 'Invalid Date';
		}
	});

	// Determine status class (similar to TemplateRow)
	const statusClass = $derived(() => `data-table__status--${session.status.toLowerCase()}`);
</script>

<tr>
	<td>
		<div class="data-table__cell-primary">{session.title}</div>
		<div class="data-table__cell-secondary">({session.templateCode})</div>
	</td>

	<td>{formattedDate}</td>

	<td>
		<span class="data-table__status {statusClass}">
			{session.status}
		</span>
	</td>

	<td>{session.participants}</td>

	<td>
		<button
			class="data-table__action-button"
			aria-label={`Actions for session ${session.title}`}
			onclick={() => onActionClick(session.id)}
		>
			<svg
				width="20"
				height="20"
				viewBox="0 0 24 24"
				fill="none"
				stroke="currentColor"
				stroke-width="1.5"
			>
				<path
					d="M12 5.25a.75.75 0 1 1 0-1.5.75.75 0 0 1 0 1.5Z"
					stroke-linecap="round"
					stroke-linejoin="round"
				/>
				<path
					d="M12 12.75a.75.75 0 1 1 0-1.5.75.75 0 0 1 0 1.5Z"
					stroke-linecap="round"
					stroke-linejoin="round"
				/>
				<path
					d="M12 20.25a.75.75 0 1 1 0-1.5.75.75 0 0 1 0 1.5Z"
					stroke-linecap="round"
					stroke-linejoin="round"
				/>
			</svg>
		</button>
	</td>
</tr>

<style lang="scss">
	// Import variables if needed for unique styles
	@import '../../styles/variables.scss';

	// Add status modifiers if not handled globally by .data-table__status--*
	// These should match the statuses defined in the Session interface
	:global(.data-table__status--inactive) {
		/* ... */
	}
	:global(.data-table__status--active) {
		/* ... */
	}
	:global(.data-table__status--finished) {
		background-color: darken($color-surface-alt, 5%);
		color: $color-text-disabled;
	}
</style>
