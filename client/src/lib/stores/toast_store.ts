import { writable } from 'svelte/store';

export type ToastType = 'success' | 'error' | 'info';

export interface ToastMessage {
	id: string;
	type: ToastType;
	message: string;
	duration: number;
}

const createToastStore = () => {
	const { subscribe, update } = writable<ToastMessage[]>([]);

	function show(message: string, type: ToastType = 'info', duration: number = 3000) {
		const id = crypto.randomUUID();
		const newToast: ToastMessage = { id, type, message, duration };

		console.log(`Showing toast: ${message} (Type: ${type}, Duration: ${duration}ms)`);

		update((toasts) => [...toasts, newToast]);

		setTimeout(() => {
			remove(id);
		}, duration);
	}

	function remove(id: string) {
		update((toasts) => toasts.filter((toast) => toast.id !== id));
	}

	return {
		subscribe,
		show,
		remove
	};
};

export const toast = createToastStore();
