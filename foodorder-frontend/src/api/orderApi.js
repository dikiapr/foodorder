import api from './axiosInstance';

export const getOrders = (params) => api.get('/api/order', { params });
export const getOrder = (id) => api.get(`/api/order/${id}`);
export const checkout = () => api.post('/api/order/checkout');
export const updateOrderStatus = (id, data) => api.put(`/api/order/${id}/status`, data);
export const confirmReceived = (id) => api.put(`/api/order/${id}/confirm-received`);
