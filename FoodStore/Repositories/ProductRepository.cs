using FoodStore.Data;
using FoodStore.Interfaces;
using FoodStore.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(int? categoryId)
    {
        IQueryable<Product> query = _context.Products
            .Include(product => product.Category)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(product => product.CategoryId == categoryId.Value);
        }

        List<Product> products = await query.ToListAsync();
        return products;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        Product? product = await _context.Products
            .Include(product => product.Category)
            .FirstOrDefaultAsync(product => product.Id == id);
        return product;
    }

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        await _context.Entry(product).Reference(product => product.Category).LoadAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        await _context.SaveChangesAsync();
        await _context.Entry(product).Reference(product => product.Category).LoadAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CategoryExistsAsync(int categoryId)
    {
        bool categoryExists = await _context.Categories.AnyAsync(category => category.Id == categoryId);
        return categoryExists;
    }

    public async Task<bool> HasOrderItemsAsync(int productId)
    {
        bool hasOrderItems = await _context.OrderItems.AnyAsync(orderItem => orderItem.ProductId == productId);
        return hasOrderItems;
    }
}
