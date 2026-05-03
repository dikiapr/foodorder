using FoodStore.Data;
using FoodStore.DTOs.Request;
using FoodStore.Enums;
using FoodStore.Interfaces;
using FoodStore.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Order> Items, int TotalCount)> GetAllAsync(int? userId, OrderQueryParameters parameters)
    {
        IQueryable<Order> query = _context.Orders
            .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.Product)
            .AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(order => order.UserId == userId.Value);
        }

        if (parameters.Status.HasValue)
        {
            query = query.Where(order => order.Status == parameters.Status.Value);
        }

        query = (parameters.SortBy.ToLower(), parameters.SortOrder.ToLower()) switch
        {
            ("totalamount", "asc")  => query.OrderBy(order => order.TotalAmount),
            ("totalamount", _)      => query.OrderByDescending(order => order.TotalAmount),
            (_, "asc")              => query.OrderBy(order => order.OrderDate),
            _                       => query.OrderByDescending(order => order.OrderDate)
        };

        int totalCount = await query.CountAsync();

        List<Order> items = await query
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        Order? order = await _context.Orders
            .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.Product)
            .FirstOrDefaultAsync(order => order.Id == id);
        return order;
    }

    public async Task<Order> CheckoutAsync(int userId, IEnumerable<CartItem> cartItems)
    {
        decimal totalAmount = cartItems.Sum(item => item.Product!.Price * item.Quantity);

        Order order = new Order()
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            TotalAmount = totalAmount,
            OrderItems = cartItems.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.Product!.Price
            }).ToList()
        };

        _context.Orders.Add(order);

        foreach (CartItem item in cartItems)
        {
            item.Product!.Stock -= item.Quantity;
        }

        _context.CartItems.RemoveRange(cartItems);

        await _context.SaveChangesAsync();

        Order? createdOrder = await GetByIdAsync(order.Id);
        return createdOrder!;
    }

    public async Task UpdateStatusAsync(Order order)
    {
        await _context.SaveChangesAsync();
    }
}
