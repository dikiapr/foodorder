using FoodStoreIdentity.Data;
using FoodStoreIdentity.Interfaces;
using FoodStoreIdentity.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStoreIdentity.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        List<Category> categories = await _context.Categories
            .OrderBy(category => category.Name)
            .ToListAsync();
        return categories;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        Category? category = await _context.Categories.FindAsync(id);
        return category;
    }

    public async Task AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
    {
        bool exists = await _context.Categories
            .AnyAsync(category => category.Name == name && category.Id != excludeId);
        return exists;
    }

    public async Task<bool> HasProductsAsync(int categoryId)
    {
        bool hasProducts = await _context.Products
            .AnyAsync(product => product.CategoryId == categoryId);
        return hasProducts;
    }
}
