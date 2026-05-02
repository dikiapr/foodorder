using FoodStore.Common;
using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;
using FoodStore.Interfaces;
using FoodStore.Models;

namespace FoodStore.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartItemRepository _cartItemRepository;

    public OrderService(IOrderRepository orderRepository, ICartItemRepository cartItemRepository)
    {
        _orderRepository = orderRepository;
        _cartItemRepository = cartItemRepository;
    }

    public async Task<PagedResponse<OrderResponse>> GetByUserIdAsync(int userId, OrderQueryParameters parameters)
    {
        (IEnumerable<Order> Items, int TotalCount) paginatedOrders = await _orderRepository.GetByUserIdAsync(userId, parameters);

        IEnumerable<OrderResponse> data = paginatedOrders.Items.Select(ToResponse);
        int totalPages = (int)Math.Ceiling(paginatedOrders.TotalCount / (double)parameters.PageSize);

        PagedResponse<OrderResponse> response = new PagedResponse<OrderResponse>()
        {
            Data = data,
            Page = parameters.Page,
            PageSize = parameters.PageSize,
            TotalCount = paginatedOrders.TotalCount,
            TotalPages = totalPages
        };

        return response;
    }

    public async Task<OrderResponse?> GetByIdAsync(int id)
    {
        Order? order = await _orderRepository.GetByIdAsync(id);
        OrderResponse? response = order == null ? null : ToResponse(order);
        return response;
    }

    public async Task<OrderResponse> CheckoutAsync(CheckoutRequest request)
    {
        IEnumerable<CartItem> cartItems = await _cartItemRepository.GetByUserIdAsync(request.UserId);

        if (!cartItems.Any())
        {
            throw new InvalidOperationException("Cart is empty.");
        }

        foreach (CartItem item in cartItems)
        {
            if (item.Product!.Stock < item.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for \"{item.Product.Name}\".");
            }
        }

        Order order = await _orderRepository.CheckoutAsync(request.UserId, cartItems);
        OrderResponse response = ToResponse(order);
        return response;
    }

    public async Task<OrderResponse?> UpdateStatusAsync(int id, UpdateOrderStatusRequest request)
    {
        Order? order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            return null;
        }

        order.Status = request.Status;
        await _orderRepository.UpdateStatusAsync(order);

        OrderResponse? response = ToResponse(order);
        return response;
    }

    private static OrderResponse ToResponse(Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            UserId = order.UserId,
            OrderDate = order.OrderDate,
            Status = order.Status.ToString(),
            TotalAmount = order.TotalAmount,
            Items = order.OrderItems.Select(orderItem => new OrderItemResponse
            {
                Id = orderItem.Id,
                ProductId = orderItem.ProductId,
                ProductName = orderItem.Product?.Name ?? string.Empty,
                UnitPrice = orderItem.UnitPrice,
                Quantity = orderItem.Quantity,
                Subtotal = orderItem.UnitPrice * orderItem.Quantity
            }).ToList()
        };
    }
}
