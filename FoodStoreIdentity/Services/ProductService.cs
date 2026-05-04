using FoodStoreIdentity.DTOs.Request;
using FoodStoreIdentity.DTOs.Response;
using FoodStoreIdentity.Interfaces;
using FoodStoreIdentity.Models;

namespace FoodStoreIdentity.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductResponse>> GetAllAsync()
    {
        IEnumerable<Product> products = await _productRepository.GetAllAsync();
        return products.Select(ToResponse);
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

        Product product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            CategoryId = request.CategoryId
        };

        await _productRepository.AddAsync(product);

        Product? productWithCategory = await _productRepository.GetByIdAsync(product.Id);
        return ToResponse(productWithCategory!);
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
        product.CategoryId = request.CategoryId;

        await _productRepository.UpdateAsync(product);

        Product? updatedProduct = await _productRepository.GetByIdAsync(id);
        return ToResponse(updatedProduct!);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Product? product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        await _productRepository.DeleteAsync(product);
        return true;
    }

    private static ProductResponse ToResponse(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        Stock = product.Stock,
        CategoryId = product.CategoryId,
        CategoryName = product.Category?.Name
    };
}
