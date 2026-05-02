using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;

namespace FoodStore.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderResponse>> GetByUserIdAsync(int userId);
    Task<OrderResponse?> GetByIdAsync(int id);
    Task<OrderResponse> CheckoutAsync(CheckoutRequest request);
    Task<OrderResponse?> UpdateStatusAsync(int id, UpdateOrderStatusRequest request);
}
