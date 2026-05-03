import api from './axiosInstance';

export const getCart = () => api.get('/api/cartitem');
export const addToCart = (data) => api.post('/api/cartitem', data);
export const updateCartItem = (id, data) => api.put(`/api/cartitem/${id}`, data);
export const removeCartItem = (id) => api.delete(`/api/cartitem/${id}`);
export const clearCart = () => api.delete('/api/cartitem/clear');
