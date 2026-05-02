using FoodStore.DTOs.Request;
using FoodStore.Models;

namespace FoodStore.Interfaces;

public interface IOrderRepository
{
    Task<(IEnumerable<Order> Items, int TotalCount)> GetByUserIdAsync(int userId, OrderQueryParameters parameters);
    Task<Order?> GetByIdAsync(int id);
    Task<Order> CheckoutAsync(int userId, IEnumerable<CartItem> cartItems);
    Task UpdateStatusAsync(Order order);
}
