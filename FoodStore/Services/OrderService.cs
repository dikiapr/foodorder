using AutoMapper;
using FoodStore.Common;
using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;
using FoodStore.Enums;
using FoodStore.Interfaces;
using FoodStore.Models;

namespace FoodStore.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, ICartItemRepository cartItemRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _cartItemRepository = cartItemRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<OrderResponse>> GetAllAsync(int? userId, OrderQueryParameters parameters)
    {
        (IEnumerable<Order> Items, int TotalCount) paginatedOrders = await _orderRepository.GetAllAsync(userId, parameters);

        IEnumerable<OrderResponse> data = _mapper.Map<IEnumerable<OrderResponse>>(paginatedOrders.Items);
        int totalPages = (int)Math.Ceiling(paginatedOrders.TotalCount / (double)parameters.PageSize);

        return new PagedResponse<OrderResponse>
        {
            Data = data,
            Page = parameters.Page,
            PageSize = parameters.PageSize,
            TotalCount = paginatedOrders.TotalCount,
            TotalPages = totalPages
        };
    }

    public async Task<OrderResponse?> GetByIdAsync(int id)
    {
        Order? order = await _orderRepository.GetByIdAsync(id);
        return order == null ? null : _mapper.Map<OrderResponse>(order);
    }

    public async Task<OrderResponse> CheckoutAsync(int userId)
    {
        IEnumerable<CartItem> cartItems = await _cartItemRepository.GetByUserIdAsync(userId);

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

        Order order = await _orderRepository.CheckoutAsync(userId, cartItems);
        return _mapper.Map<OrderResponse>(order);
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
        return _mapper.Map<OrderResponse>(order);
    }

    public async Task<(OrderResponse? Order, string? Error)> ConfirmReceivedAsync(int orderId, int userId)
    {
        Order? order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            return (null, null);
        }

        if (order.UserId != userId)
        {
            return (null, "Forbidden");
        }

        if (order.Status != OrderStatus.Delivery)
        {
            return (null, "Order must be in Delivery status before it can be completed.");
        }

        order.Status = OrderStatus.Completed;
        await _orderRepository.UpdateStatusAsync(order);
        return (_mapper.Map<OrderResponse>(order), null);
    }
}
