using FoodStoreIdentity.Data;
using FoodStoreIdentity.Interfaces;
using FoodStoreIdentity.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStoreIdentity.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(product => product.Category)
            .Include(product => product.CreatedBy)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        Product? product = await _context.Products
            .Include(product => product.Category)
            .Include(product => product.CreatedBy)
            .FirstOrDefaultAsync(product => product.Id == id);
        return product;
    }

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        await _context.Entry(product).Reference(product => product.Category).LoadAsync();
        await _context.Entry(product).Reference(product => product.CreatedBy).LoadAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        await _context.SaveChangesAsync();
        await _context.Entry(product).Reference(product => product.Category).LoadAsync();
        await _context.Entry(product).Reference(product => product.CreatedBy).LoadAsync();
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
}
