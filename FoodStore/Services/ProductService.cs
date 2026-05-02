using FoodStore.Data;
using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;
using FoodStore.Interfaces;
using FoodStore.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductResponse>> GetAllAsync(int? categoryId)
    {
        IQueryable<Product> query = _context.Products
            .Include(product => product.Category)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(product => product.CategoryId == categoryId.Value);
        }

        return await query.Select(product => ToResponse(product)).ToListAsync();
    }

    public async Task<ProductResponse?> GetByIdAsync(int id)
    {
        Product? product = await _context.Products
            .Include(product => product.Category)
            .FirstOrDefaultAsync(product => product.Id == id);

        return product == null ? null : ToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        bool categoryExists = await _context.Categories
            .AnyAsync(category => category.Id == request.CategoryId);

        if (!categoryExists)
        {
            throw new InvalidOperationException("Category not found.");
        }

        Product product = new Product()
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            ImageUrl = request.ImageUrl,
            CategoryId = request.CategoryId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        await _context.Entry(product).Reference(product => product.Category).LoadAsync();

        return ToResponse(product);
    }

    public async Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request)
    {
        Product? product = await _context.Products
            .Include(product => product.Category)
            .FirstOrDefaultAsync(product => product.Id == id);

        if (product == null)
        {
            return null;
        }

        bool categoryExists = await _context.Categories
            .AnyAsync(category => category.Id == request.CategoryId);

        if (!categoryExists)
        {
            throw new InvalidOperationException("Category not found.");
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.ImageUrl = request.ImageUrl;
        product.CategoryId = request.CategoryId;

        await _context.SaveChangesAsync();
        await _context.Entry(product).Reference(product => product.Category).LoadAsync();

        return ToResponse(product);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Product? product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return false;
        }

        bool hasOrderItems = await _context.OrderItems.AnyAsync(orderItem => orderItem.ProductId == id);

        if (hasOrderItems)
        {
            throw new InvalidOperationException("Cannot delete product that has order history.");
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }

    private static ProductResponse ToResponse(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name
        };
    }
}
