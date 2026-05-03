using FoodStore.Data;
using FoodStore.Interfaces;
using FoodStore.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Repositories;

public class CartItemRepository : ICartItemRepository
{
    private readonly AppDbContext _context;

    public CartItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CartItem>> GetByUserIdAsync(int userId)
    {
        List<CartItem> cartItems = await _context.CartItems
            .Include(cartItem => cartItem.Product)
            .Where(cartItem => cartItem.UserId == userId)
            .ToListAsync();
        return cartItems;
    }

    public async Task<CartItem?> GetByIdAsync(int id)
    {
        CartItem? cartItem = await _context.CartItems
            .Include(cartItem => cartItem.Product)
            .FirstOrDefaultAsync(cartItem => cartItem.Id == id);
        return cartItem;
    }

    public async Task<CartItem?> GetByUserAndProductAsync(int userId, int productId)
    {
        CartItem? cartItem = await _context.CartItems
            .FirstOrDefaultAsync(cartItem => cartItem.UserId == userId && cartItem.ProductId == productId);
        return cartItem;
    }

    public async Task AddAsync(CartItem cartItem)
    {
        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();
        await _context.Entry(cartItem).Reference(cartItem => cartItem.Product).LoadAsync();
    }

    public async Task UpdateAsync(CartItem cartItem)
    {
        await _context.SaveChangesAsync();
        await _context.Entry(cartItem).Reference(cartItem => cartItem.Product).LoadAsync();
    }

    public async Task DeleteAsync(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
    }

    public async Task ClearByUserIdAsync(int userId)
    {
        List<CartItem> cartItems = await _context.CartItems
            .Where(cartItem => cartItem.UserId == userId)
            .ToListAsync();
        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();
    }
}
