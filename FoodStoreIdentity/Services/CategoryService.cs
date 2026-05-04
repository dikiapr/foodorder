using FoodStoreIdentity.Data;
using FoodStoreIdentity.DTOs.Request;
using FoodStoreIdentity.DTOs.Response;
using FoodStoreIdentity.Interfaces;
using FoodStoreIdentity.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStoreIdentity.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
    {
        List<Category> categories = await _context.Categories.ToListAsync();
        return categories.Select(ToResponse);
    }

    public async Task<CategoryResponse?> GetByIdAsync(int id)
    {
        Category? category = await _context.Categories.FindAsync(id);
        return category == null ? null : ToResponse(category);
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
    {
        Category category = new Category { Name = request.Name };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return ToResponse(category);
    }

    public async Task<CategoryResponse?> UpdateAsync(int id, UpdateCategoryRequest request)
    {
        Category? category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return null;
        }

        category.Name = request.Name;
        await _context.SaveChangesAsync();
        return ToResponse(category);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Category? category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return false;
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }

    private static CategoryResponse ToResponse(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name
    };
}
