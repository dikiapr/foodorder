import { create } from 'zustand';
import { getCart } from '../api/cartApi';

const useCartStore = create((set) => ({
  itemCount: 0,
  setItemCount: (count) => set({ itemCount: count }),
  reset: () => set({ itemCount: 0 }),
  fetchCount: async () => {
    try {
      const res = await getCart();
      set({ itemCount: res.data.length });
    } catch {
      set({ itemCount: 0 });
    }
  },
}));

export default useCartStore;
