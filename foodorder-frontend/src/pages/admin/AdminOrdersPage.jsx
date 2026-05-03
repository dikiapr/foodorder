import { useEffect, useState, useCallback } from 'react';
import { X } from 'lucide-react';
import toast from 'react-hot-toast';
import { getOrders, updateOrderStatus } from '../../api/orderApi';
import StatusBadge from '../../components/StatusBadge';
import Pagination from '../../components/Pagination';

const STATUS_OPTIONS = ['', 'Pending', 'Paid', 'Completed', 'Cancelled'];
const STATUS_VALUES = ['Pending', 'Paid', 'Completed', 'Cancelled'];

export default function AdminOrdersPage() {
  const [orders, setOrders] = useState([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [total, setTotal] = useState(0);
  const [statusFilter, setStatusFilter] = useState('');
  const [loading, setLoading] = useState(true);

  const [showModal, setShowModal] = useState(false);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const [newStatus, setNewStatus] = useState('');
  const [updating, setUpdating] = useState(false);

  const fetchOrders = useCallback(async () => {
    setLoading(true);
    try {
      const params = {
        page, pageSize: 10,
        sortBy: 'orderdate', sortOrder: 'desc',
        ...(statusFilter && { status: statusFilter }),
      };
      const res = await getOrders(params);
      setOrders(res.data.data);
      setTotal(res.data.totalCount);
      setTotalPages(res.data.totalPages);
    } catch { toast.error('Failed to load orders'); }
    finally { setLoading(false); }
  }, [page, statusFilter]);

  useEffect(() => { fetchOrders(); }, [fetchOrders]);

  const openStatusModal = (order) => {
    setSelectedOrder(order);
    setNewStatus(order.status);
    setShowModal(true);
  };

  const handleUpdateStatus = async () => {
    setUpdating(true);
    try {
      await updateOrderStatus(selectedOrder.id, { status: newStatus });
      toast.success('Order status updated');
      setShowModal(false);
      fetchOrders();
    } catch (err) {
      toast.error(err.response?.data?.message || 'Update failed');
    } finally { setUpdating(false); }
  };

  return (
    <div>
      <div className="flex items-center justify-between mb-6">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Order Management</h1>
          <p className="text-sm text-gray-400 mt-0.5">Track, update, and manage incoming food orders. ({total} total)</p>
        </div>
        <select
          value={statusFilter} onChange={(e) => { setStatusFilter(e.target.value); setPage(1); }}
          className="text-sm border border-gray-200 bg-white rounded-xl px-3 py-2 focus:outline-none focus:ring-2 focus:ring-orange-400"
        >
          {STATUS_OPTIONS.map((s) => <option key={s} value={s}>{s || 'All Statuses'}</option>)}
        </select>
      </div>

      <div className="bg-white rounded-2xl shadow-sm overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-sm">
            <thead>
              <tr className="bg-orange-50">
                <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase">Order ID</th>
                <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase">Date & Time</th>
                <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase">Items</th>
                <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase">Total</th>
                <th className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase">Status</th>
                <th className="px-4 py-3 text-right text-xs font-semibold text-gray-500 uppercase">Action</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-50">
              {loading
                ? Array.from({ length: 5 }).map((_, i) => (
                    <tr key={i}>{Array.from({ length: 6 }).map((_, j) => (
                      <td key={j} className="px-4 py-4"><div className="h-4 bg-gray-100 rounded animate-pulse" /></td>
                    ))}</tr>
                  ))
                : orders.map((order) => (
                    <tr key={order.id} className="hover:bg-gray-50 transition">
                      <td className="px-4 py-4">
                        <span className="font-bold text-orange-500">#{order.id}</span>
                      </td>
                      <td className="px-4 py-4 text-gray-500">
                        {new Date(order.orderDate).toLocaleDateString()}<br />
                        <span className="text-xs">{new Date(order.orderDate).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</span>
                      </td>
                      <td className="px-4 py-4 text-gray-500">
                        {order.items.length} {order.items.length === 1 ? 'item' : 'items'}
                      </td>
                      <td className="px-4 py-4 font-semibold text-gray-900">
                        ${order.totalAmount.toFixed(2)}
                      </td>
                      <td className="px-4 py-4">
                        <StatusBadge status={order.status} />
                      </td>
                      <td className="px-4 py-4 text-right">
                        <button
                          onClick={() => openStatusModal(order)}
                          disabled={order.status === 'Completed' || order.status === 'Cancelled'}
                          className="px-3 py-1.5 border border-orange-400 text-orange-500 text-xs font-semibold rounded-lg hover:bg-orange-50 disabled:opacity-40 disabled:cursor-not-allowed transition"
                        >
                          Update Status
                        </button>
                      </td>
                    </tr>
                  ))
              }
            </tbody>
          </table>
        </div>
        <div className="px-4 py-3 border-t border-gray-100">
          <Pagination page={page} totalPages={totalPages} onChange={setPage} />
        </div>
      </div>

      {/* Status modal */}
      {showModal && selectedOrder && (
        <div className="fixed inset-0 bg-black/40 flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-2xl shadow-xl w-full max-w-sm">
            <div className="flex items-center justify-between px-6 py-4 border-b border-gray-100">
              <h2 className="font-bold text-gray-900">Update Order #{selectedOrder.id}</h2>
              <button onClick={() => setShowModal(false)} className="text-gray-400 hover:text-gray-600"><X size={20} /></button>
            </div>
            <div className="p-6 space-y-4">
              <div>
                <p className="text-sm text-gray-500 mb-1">Current status</p>
                <StatusBadge status={selectedOrder.status} />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1.5">New Status</label>
                <div className="space-y-2">
                  {STATUS_VALUES.map((s) => (
                    <label key={s} className={`flex items-center gap-3 p-3 rounded-xl border cursor-pointer transition ${
                      newStatus === s ? 'border-orange-400 bg-orange-50' : 'border-gray-200 hover:border-gray-300'
                    }`}>
                      <input type="radio" name="status" value={s} checked={newStatus === s} onChange={() => setNewStatus(s)} className="accent-orange-500" />
                      <StatusBadge status={s} />
                    </label>
                  ))}
                </div>
              </div>
              <div className="flex gap-3 pt-1">
                <button onClick={() => setShowModal(false)}
                  className="flex-1 px-4 py-2.5 border border-gray-200 rounded-xl text-sm font-medium text-gray-700 hover:bg-gray-50 transition">
                  Cancel
                </button>
                <button onClick={handleUpdateStatus} disabled={updating || newStatus === selectedOrder.status}
                  className="flex-1 bg-orange-500 hover:bg-orange-600 disabled:opacity-60 text-white font-semibold py-2.5 rounded-xl transition text-sm">
                  {updating ? 'Updating...' : 'Confirm'}
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
