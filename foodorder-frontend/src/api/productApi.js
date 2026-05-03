import api from './axiosInstance';

export const getProducts = (params) => api.get('/api/product', { params });
export const getProduct = (id) => api.get(`/api/product/${id}`);
export const createProduct = (data) => api.post('/api/product', data);
export const updateProduct = (id, data) => api.put(`/api/product/${id}`, data);
export const deleteProduct = (id) => api.delete(`/api/product/${id}`);
