using FoodStore.Common;
using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;

namespace FoodStore.Interfaces;

public interface IOrderService
{
    Task<PagedResponse<OrderResponse>> GetByUserIdAsync(int userId, OrderQueryParameters parameters);
    Task<OrderResponse?> GetByIdAsync(int id);
    Task<OrderResponse> CheckoutAsync(CheckoutRequest request);
    Task<OrderResponse?> UpdateStatusAsync(int id, UpdateOrderStatusRequest request);
}
