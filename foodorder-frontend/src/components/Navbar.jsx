import { Link, useNavigate } from 'react-router-dom';
import { ShoppingCart, Bell, LogOut, ChevronDown } from 'lucide-react';
import { useState } from 'react';
import useAuthStore from '../store/authStore';
import useCartStore from '../store/cartStore';

export default function Navbar() {
  const navigate = useNavigate();
  const { user, logout } = useAuthStore();
  const itemCount = useCartStore((s) => s.itemCount);
  const resetCart = useCartStore((s) => s.reset);
  const [dropdownOpen, setDropdownOpen] = useState(false);

  const handleLogout = () => {
    logout();
    resetCart();
    navigate('/login');
  };

  return (
    <nav className="sticky top-0 z-40 bg-white shadow-sm border-b border-orange-100">
      <div className="max-w-7xl mx-auto px-4 h-16 flex items-center justify-between">
        {/* Logo */}
        <Link to="/home" className="text-xl font-bold text-orange-500 tracking-tight">
          FoodStore
        </Link>

        {/* Nav links */}
        <div className="hidden md:flex items-center gap-6">
          <Link to="/home" className="text-sm font-medium text-gray-700 hover:text-orange-500 transition">
            Menu
          </Link>
          <Link to="/orders" className="text-sm font-medium text-gray-700 hover:text-orange-500 transition">
            My Orders
          </Link>
        </div>

        {/* Right side */}
        <div className="flex items-center gap-2">
          <Link to="/cart" className="relative p-2 rounded-full hover:bg-orange-50 transition">
            <ShoppingCart size={20} className="text-gray-700" />
            {itemCount > 0 && (
              <span className="absolute -top-0.5 -right-0.5 bg-orange-500 text-white text-[10px] font-bold w-4 h-4 flex items-center justify-center rounded-full">
                {itemCount > 9 ? '9+' : itemCount}
              </span>
            )}
          </Link>

          <button className="p-2 rounded-full hover:bg-orange-50 transition">
            <Bell size={20} className="text-gray-700" />
          </button>

          <div className="relative">
            <button
              onClick={() => setDropdownOpen((v) => !v)}
              className="flex items-center gap-2 pl-2 pr-1 py-1 rounded-full hover:bg-orange-50 transition"
            >
              <div className="w-8 h-8 rounded-full bg-orange-500 flex items-center justify-center text-white text-sm font-semibold">
                {user?.username?.[0]?.toUpperCase()}
              </div>
              <ChevronDown size={14} className="text-gray-500" />
            </button>

            {dropdownOpen && (
              <div className="absolute right-0 mt-2 w-44 bg-white rounded-xl shadow-lg border border-gray-100 py-1 z-50">
                <div className="px-4 py-2 border-b border-gray-100">
                  <p className="text-sm font-semibold text-gray-900 truncate">{user?.username}</p>
                  <p className="text-xs text-gray-400 truncate">{user?.email}</p>
                </div>
                <button
                  onClick={handleLogout}
                  className="flex items-center gap-2 w-full px-4 py-2 text-sm text-red-600 hover:bg-red-50 transition"
                >
                  <LogOut size={14} />
                  Logout
                </button>
              </div>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
}
