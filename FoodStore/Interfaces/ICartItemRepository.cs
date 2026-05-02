using FoodStore.Models;

namespace FoodStore.Interfaces;

public interface ICartItemRepository
{
    Task<IEnumerable<CartItem>> GetByUserIdAsync(int userId);
    Task<CartItem?> GetByIdAsync(int id);
    Task<CartItem?> GetByUserAndProductAsync(int userId, int productId);
    Task<bool> UserExistsAsync(int userId);
    Task<bool> ProductExistsAsync(int productId);
    Task AddAsync(CartItem cartItem);
    Task UpdateAsync(CartItem cartItem);
    Task DeleteAsync(CartItem cartItem);
    Task ClearByUserIdAsync(int userId);
}
