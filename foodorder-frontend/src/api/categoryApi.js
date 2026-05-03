import api from './axiosInstance';

export const getCategories = () => api.get('/api/category');
export const createCategory = (data) => api.post('/api/category', data);
export const updateCategory = (id, data) => api.put(`/api/category/${id}`, data);
export const deleteCategory = (id) => api.delete(`/api/category/${id}`);
