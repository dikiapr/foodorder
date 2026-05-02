using FoodStore.Data;
using FoodStore.DTOs.Request;
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

    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetAllAsync(ProductQueryParameters parameters)
    {
        IQueryable<Product> query = _context.Products
            .Include(product => product.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            query = query.Where(product => product.Name.Contains(parameters.Search));
        }

        if (parameters.CategoryId.HasValue)
        {
            query = query.Where(product => product.CategoryId == parameters.CategoryId.Value);
        }

        query = (parameters.SortBy.ToLower(), parameters.SortOrder.ToLower()) switch
        {
            ("price", "desc") => query.OrderByDescending(product => product.Price),
            ("price", _)      => query.OrderBy(product => product.Price),
            ("stock", "desc") => query.OrderByDescending(product => product.Stock),
            ("stock", _)      => query.OrderBy(product => product.Stock),
            (_, "desc")       => query.OrderByDescending(product => product.Name),
            _                 => query.OrderBy(product => product.Name)
        };

        int totalCount = await query.CountAsync();

        List<Product> items = await query
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return (items, totalCount);
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
