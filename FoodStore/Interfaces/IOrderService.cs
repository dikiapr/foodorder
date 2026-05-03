using FoodStore.Common;
using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;

namespace FoodStore.Interfaces;

public interface IOrderService
{
    Task<PagedResponse<OrderResponse>> GetAllAsync(int? userId, OrderQueryParameters parameters);
    Task<OrderResponse?> GetByIdAsync(int id);
    Task<OrderResponse> CheckoutAsync(int userId);
    Task<OrderResponse?> UpdateStatusAsync(int id, UpdateOrderStatusRequest request);
    Task<(OrderResponse? Order, string? Error)> ConfirmReceivedAsync(int orderId, int userId);
}
