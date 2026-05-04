using FoodStoreIdentity.Data;
using FoodStoreIdentity.DTOs.Request;
using FoodStoreIdentity.DTOs.Response;
using FoodStoreIdentity.Interfaces;
using FoodStoreIdentity.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStoreIdentity.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductResponse>> GetAllAsync()
    {
        List<Product> products = await _context.Products.ToListAsync();
        return products.Select(ToResponse);
    }

    public async Task<ProductResponse?> GetByIdAsync(int id)
    {
        Product? product = await _context.Products.FindAsync(id);
        return product == null ? null : ToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        Product product = new Product()
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return ToResponse(product);
    }

    public async Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request)
    {
        Product? product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return null;
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;

        await _context.SaveChangesAsync();
        return ToResponse(product);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Product? product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    private static ProductResponse ToResponse(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        Stock = product.Stock
    };
}
