import { Outlet } from 'react-router-dom';
import { useEffect } from 'react';
import Navbar from './Navbar';
import useAuthStore from '../store/authStore';
import useCartStore from '../store/cartStore';

export default function CustomerLayout() {
  const user = useAuthStore((s) => s.user);
  const fetchCount = useCartStore((s) => s.fetchCount);

  useEffect(() => {
    if (user) fetchCount();
  }, [user]);

  return (
    <div className="min-h-screen bg-[#fff8f6]">
      <Navbar />
      <main className="max-w-7xl mx-auto px-4 py-8">
        <Outlet />
      </main>
    </div>
  );
}
