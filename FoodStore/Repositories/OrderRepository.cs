using FoodStore.Data;
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

    public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
    {
        List<Order> orders = await _context.Orders
            .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.Product)
            .Where(order => order.UserId == userId)
            .OrderByDescending(order => order.OrderDate)
            .ToListAsync();
        return orders;
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
