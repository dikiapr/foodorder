import { create } from 'zustand';
import { persist } from 'zustand/middleware';

const useAuthStore = create(
  persist(
    (set, get) => ({
      user: null,
      login: (user) => set({ user }),
      logout: () => set({ user: null }),
      isAdmin: () => get().user?.role === 'Admin',
    }),
    { name: 'auth-storage' }
  )
);

export default useAuthStore;
