import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Minus, Plus, Trash2, ShoppingBag } from 'lucide-react';
import toast from 'react-hot-toast';
import { getCart, updateCartItem, removeCartItem } from '../../api/cartApi';
import { checkout } from '../../api/orderApi';
import useCartStore from '../../store/cartStore';

export default function CartPage() {
  const navigate = useNavigate();
  const { setItemCount, reset } = useCartStore();

  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [checkingOut, setCheckingOut] = useState(false);

  const fetchCart = async () => {
    try {
      const res = await getCart();
      setItems(res.data);
      setItemCount(res.data.length);
    } catch {
      toast.error('Failed to load cart');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchCart(); }, []);

  const handleQtyChange = async (item, delta) => {
    const newQty = item.quantity + delta;
    if (newQty < 1) return;
    try {
      await updateCartItem(item.id, { quantity: newQty });
      setItems((prev) => prev.map((i) => i.id === item.id ? { ...i, quantity: newQty, subtotal: i.productPrice * newQty } : i));
    } catch (err) {
      toast.error(err.response?.data?.message || 'Failed to update quantity');
    }
  };

  const handleRemove = async (id) => {
    try {
      await removeCartItem(id);
      const updated = items.filter((i) => i.id !== id);
      setItems(updated);
      setItemCount(updated.length);
      toast.success('Item removed');
    } catch {
      toast.error('Failed to remove item');
    }
  };

  const handleCheckout = async () => {
    setCheckingOut(true);
    try {
      const res = await checkout();
      reset();
      toast.success('Order placed successfully!');
      navigate(`/orders/${res.data.id}`);
    } catch (err) {
      toast.error(err.response?.data?.message || 'Checkout failed');
    } finally {
      setCheckingOut(false);
    }
  };

  const total = items.reduce((sum, i) => sum + i.subtotal, 0);

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="w-8 h-8 border-4 border-orange-400 border-t-transparent rounded-full animate-spin" />
      </div>
    );
  }

  if (items.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-24 text-center">
        <ShoppingBag size={64} className="text-orange-200 mb-4" />
        <h2 className="text-xl font-bold text-gray-700 mb-2">Your cart is empty</h2>
        <p className="text-gray-400 text-sm mb-6">Add some delicious items from our menu!</p>
        <Link to="/home" className="bg-orange-500 hover:bg-orange-600 text-white font-semibold px-6 py-2.5 rounded-xl transition text-sm">
          Browse Menu
        </Link>
      </div>
    );
  }

  return (
    <div>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-bold text-gray-900">Your Cart</h1>
        <Link to="/home" className="text-sm text-orange-500 hover:text-orange-600 font-medium transition">
          ← Continue Shopping
        </Link>
      </div>

      <div className="flex flex-col lg:flex-row gap-6">
        {/* Items */}
        <div className="flex-1 space-y-3">
          {items.map((item) => (
            <div key={item.id} className="bg-white rounded-2xl p-4 shadow-sm flex items-center gap-4">
              {/* Image */}
              <div className="w-16 h-16 rounded-xl bg-orange-50 overflow-hidden shrink-0">
                {item.productImageUrl ? (
                  <img src={item.productImageUrl} alt={item.productName} className="w-full h-full object-cover" />
                ) : (
                  <div className="w-full h-full bg-gradient-to-br from-orange-100 to-orange-200 flex items-center justify-center">
                    <span className="text-xl font-bold text-orange-300">{item.productName[0]}</span>
                  </div>
                )}
              </div>

              {/* Name + Price */}
              <div className="flex-1 min-w-0">
                <p className="font-semibold text-gray-900 text-sm truncate">{item.productName}</p>
                <p className="text-orange-500 font-medium text-sm">${item.productPrice.toFixed(2)}</p>
              </div>

              {/* Qty stepper */}
              <div className="flex items-center gap-2 bg-gray-50 rounded-xl px-3 py-1.5">
                <button onClick={() => handleQtyChange(item, -1)} className="w-5 h-5 flex items-center justify-center rounded-full hover:bg-gray-200 transition">
                  <Minus size={12} />
                </button>
                <span className="w-5 text-center text-sm font-semibold">{item.quantity}</span>
                <button onClick={() => handleQtyChange(item, 1)} className="w-5 h-5 flex items-center justify-center rounded-full hover:bg-gray-200 transition">
                  <Plus size={12} />
                </button>
              </div>

              {/* Subtotal + Remove */}
              <div className="text-right shrink-0">
                <p className="text-sm font-bold text-gray-900">${item.subtotal.toFixed(2)}</p>
                <button onClick={() => handleRemove(item.id)} className="mt-1 text-red-400 hover:text-red-600 transition">
                  <Trash2 size={14} />
                </button>
              </div>
            </div>
          ))}
        </div>

        {/* Order summary */}
        <div className="lg:w-80 shrink-0">
          <div className="bg-white rounded-2xl shadow-sm p-6 sticky top-24">
            <h2 className="font-bold text-gray-900 text-lg mb-4">Order Summary</h2>
            <div className="space-y-2 text-sm mb-4">
              {items.map((item) => (
                <div key={item.id} className="flex justify-between text-gray-500">
                  <span className="truncate mr-2">{item.productName} × {item.quantity}</span>
                  <span className="shrink-0">${item.subtotal.toFixed(2)}</span>
                </div>
              ))}
            </div>
            <div className="border-t border-gray-100 pt-4 flex justify-between font-bold text-gray-900">
              <span>Total</span>
              <span className="text-orange-500 text-xl">${total.toFixed(2)}</span>
            </div>
            <button
              onClick={handleCheckout}
              disabled={checkingOut}
              className="w-full mt-5 bg-orange-500 hover:bg-orange-600 disabled:opacity-60 text-white font-semibold py-3 rounded-xl transition text-sm"
            >
              {checkingOut ? 'Processing...' : 'Proceed to Checkout →'}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
