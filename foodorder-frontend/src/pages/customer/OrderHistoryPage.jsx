import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { ClipboardList } from 'lucide-react';
import toast from 'react-hot-toast';
import { getOrders } from '../../api/orderApi';
import StatusBadge from '../../components/StatusBadge';
import Pagination from '../../components/Pagination';

const STATUS_OPTIONS = ['', 'Pending', 'Processing', 'Delivery', 'Completed', 'Cancelled'];

export default function OrderHistoryPage() {
  const navigate = useNavigate();
  const [orders, setOrders] = useState([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [statusFilter, setStatusFilter] = useState('');
  const [loading, setLoading] = useState(true);

  const fetchOrders = async () => {
    setLoading(true);
    try {
      const params = {
        page,
        pageSize: 10,
        sortBy: 'orderdate',
        sortOrder: 'desc',
        ...(statusFilter && { status: statusFilter }),
      };
      const res = await getOrders(params);
      setOrders(res.data.data);
      setTotalPages(res.data.totalPages);
    } catch {
      toast.error('Failed to load orders');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchOrders(); }, [page, statusFilter]);

  const handleStatusChange = (val) => {
    setStatusFilter(val);
    setPage(1);
  };

  return (
    <div>
      <div className="flex items-center justify-between mb-6">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Order History</h1>
          <p className="text-sm text-gray-400 mt-0.5">Review your past orders and track deliveries.</p>
        </div>
        <select
          value={statusFilter}
          onChange={(e) => handleStatusChange(e.target.value)}
          className="text-sm border border-gray-200 bg-white rounded-xl px-3 py-2 focus:outline-none focus:ring-2 focus:ring-orange-400"
        >
          {STATUS_OPTIONS.map((s) => (
            <option key={s} value={s}>{s || 'All Statuses'}</option>
          ))}
        </select>
      </div>

      {loading ? (
        <div className="space-y-3">
          {Array.from({ length: 4 }).map((_, i) => (
            <div key={i} className="bg-white rounded-2xl h-20 animate-pulse" />
          ))}
        </div>
      ) : orders.length === 0 ? (
        <div className="flex flex-col items-center py-24 text-center">
          <ClipboardList size={64} className="text-orange-200 mb-4" />
          <p className="text-lg font-semibold text-gray-700">No orders yet</p>
          <p className="text-sm text-gray-400 mt-1">Place your first order from our menu!</p>
        </div>
      ) : (
        <div className="space-y-3">
          {orders.map((order) => (
            <div key={order.id} className="bg-white rounded-2xl shadow-sm p-4 flex items-center gap-4">
              <div className="w-12 h-12 rounded-xl bg-orange-50 flex items-center justify-center shrink-0">
                <ClipboardList size={20} className="text-orange-400" />
              </div>
              <div className="flex-1 min-w-0">
                <div className="flex items-center gap-3">
                  <p className="font-bold text-gray-900 text-sm">#{order.id}</p>
                  <StatusBadge status={order.status} />
                </div>
                <p className="text-xs text-gray-400 mt-0.5">
                  {new Date(order.orderDate).toLocaleString()} • {order.items.length} {order.items.length === 1 ? 'item' : 'items'}
                </p>
              </div>
              <div className="text-right shrink-0">
                <p className="text-sm font-bold text-gray-900">TOTAL</p>
                <p className="text-orange-500 font-bold">${order.totalAmount.toFixed(2)}</p>
              </div>
              <button
                onClick={() => navigate(`/orders/${order.id}`)}
                className="shrink-0 px-4 py-2 border border-orange-400 text-orange-500 text-sm font-semibold rounded-xl hover:bg-orange-50 transition"
              >
                View Detail
              </button>
            </div>
          ))}
        </div>
      )}

      <Pagination page={page} totalPages={totalPages} onChange={setPage} />
    </div>
  );
}
