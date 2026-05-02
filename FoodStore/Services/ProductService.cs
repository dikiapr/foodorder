using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;
using FoodStore.Interfaces;
using FoodStore.Models;

namespace FoodStore.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductResponse>> GetAllAsync(int? categoryId)
    {
        IEnumerable<Product> products = await _productRepository.GetAllAsync(categoryId);
        return products.Select(product => ToResponse(product));
    }

    public async Task<ProductResponse?> GetByIdAsync(int id)
    {
        Product? product = await _productRepository.GetByIdAsync(id);
        return product == null ? null : ToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        bool categoryExists = await _productRepository.CategoryExistsAsync(request.CategoryId);
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

        await _productRepository.AddAsync(product);
        return ToResponse(product);
    }

    public async Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request)
    {
        Product? product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return null;
        }

        bool categoryExists = await _productRepository.CategoryExistsAsync(request.CategoryId);
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

        await _productRepository.UpdateAsync(product);
        return ToResponse(product);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Product? product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        bool hasOrderItems = await _productRepository.HasOrderItemsAsync(id);
        if (hasOrderItems)
        {
            throw new InvalidOperationException("Cannot delete product that has order history.");
        }

        await _productRepository.DeleteAsync(product);
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
