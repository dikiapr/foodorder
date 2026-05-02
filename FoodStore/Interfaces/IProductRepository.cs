using FoodStore.DTOs.Request;
using FoodStore.Models;

namespace FoodStore.Interfaces;

public interface IProductRepository
{
    Task<(IEnumerable<Product> Items, int TotalCount)> GetAllAsync(ProductQueryParameters parameters);
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
    Task<bool> CategoryExistsAsync(int categoryId);
    Task<bool> HasOrderItemsAsync(int productId);
}
