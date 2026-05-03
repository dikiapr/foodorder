import { useEffect, useState } from 'react';
import { Package, Tag, ClipboardList, Clock } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { getProducts } from '../../api/productApi';
import { getCategories } from '../../api/categoryApi';
import { getOrders } from '../../api/orderApi';
import StatusBadge from '../../components/StatusBadge';

export default function AdminDashboardPage() {
  const navigate = useNavigate();
  const [stats, setStats] = useState({ products: 0, categories: 0, totalOrders: 0, pendingOrders: 0 });
  const [recentOrders, setRecentOrders] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const load = async () => {
      try {
        const [prodRes, catRes, ordersRes, pendingRes] = await Promise.all([
          getProducts({ page: 1, pageSize: 1 }),
          getCategories(),
          getOrders({ page: 1, pageSize: 5, sortBy: 'orderdate', sortOrder: 'desc' }),
          getOrders({ page: 1, pageSize: 1, status: 'Pending' }),
        ]);
        setStats({
          products: prodRes.data.totalCount,
          categories: catRes.data.length,
          totalOrders: ordersRes.data.totalCount,
          pendingOrders: pendingRes.data.totalCount,
        });
        setRecentOrders(ordersRes.data.data);
      } catch {}
      finally { setLoading(false); }
    };
    load();
  }, []);

  const cards = [
    { label: 'Total Products', value: stats.products, icon: Package, color: 'bg-orange-50 text-orange-500' },
    { label: 'Total Categories', value: stats.categories, icon: Tag, color: 'bg-blue-50 text-blue-500' },
    { label: 'Total Orders', value: stats.totalOrders, icon: ClipboardList, color: 'bg-green-50 text-green-500' },
    { label: 'Pending Orders', value: stats.pendingOrders, icon: Clock, color: 'bg-amber-50 text-amber-500' },
  ];

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-gray-900">Dashboard Overview</h1>
        <p className="text-sm text-gray-400 mt-1">Welcome back, Admin. Here's what's happening today.</p>
      </div>

      {/* Stats cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-4 mb-8">
        {cards.map(({ label, value, icon: Icon, color }) => (
          <div key={label} className="bg-white rounded-2xl shadow-sm p-5 flex items-center justify-between">
            <div>
              <p className="text-xs font-semibold text-gray-400 uppercase tracking-wide">{label}</p>
              <p className="text-3xl font-bold text-gray-900 mt-1">
                {loading ? <span className="inline-block w-12 h-8 bg-gray-100 rounded animate-pulse" /> : value.toLocaleString()}
              </p>
            </div>
            <div className={`w-12 h-12 rounded-xl flex items-center justify-center ${color}`}>
              <Icon size={22} />
            </div>
          </div>
        ))}
      </div>

      {/* Recent orders */}
      <div className="bg-white rounded-2xl shadow-sm overflow-hidden">
        <div className="px-6 py-4 border-b border-gray-100 flex items-center justify-between">
          <h2 className="font-bold text-gray-900">Recent Orders</h2>
          <button onClick={() => navigate('/admin/orders')} className="text-sm text-orange-500 hover:text-orange-600 font-medium transition">
            View All →
          </button>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full text-sm">
            <thead>
              <tr className="bg-orange-50 text-left">
                <th className="px-6 py-3 text-xs font-semibold text-gray-500 uppercase tracking-wide">Order ID</th>
                <th className="px-6 py-3 text-xs font-semibold text-gray-500 uppercase tracking-wide">Date</th>
                <th className="px-6 py-3 text-xs font-semibold text-gray-500 uppercase tracking-wide">Status</th>
                <th className="px-6 py-3 text-xs font-semibold text-gray-500 uppercase tracking-wide text-right">Amount</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-50">
              {loading
                ? Array.from({ length: 4 }).map((_, i) => (
                    <tr key={i}>
                      {Array.from({ length: 4 }).map((_, j) => (
                        <td key={j} className="px-6 py-4">
                          <div className="h-4 bg-gray-100 rounded animate-pulse" />
                        </td>
                      ))}
                    </tr>
                  ))
                : recentOrders.map((order) => (
                    <tr key={order.id} className="hover:bg-gray-50 transition">
                      <td className="px-6 py-4">
                        <span className="font-semibold text-orange-500">#{order.id}</span>
                      </td>
                      <td className="px-6 py-4 text-gray-500">
                        {new Date(order.orderDate).toLocaleDateString()}
                      </td>
                      <td className="px-6 py-4">
                        <StatusBadge status={order.status} />
                      </td>
                      <td className="px-6 py-4 text-right font-semibold text-gray-900">
                        ${order.totalAmount.toFixed(2)}
                      </td>
                    </tr>
                  ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
