using FoodStore.Models;

namespace FoodStore.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
    Task<Order?> GetByIdAsync(int id);
    Task<Order> CheckoutAsync(int userId, IEnumerable<CartItem> cartItems);
    Task UpdateStatusAsync(Order order);
}
