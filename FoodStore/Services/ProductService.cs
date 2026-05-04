using AutoMapper;
using FoodStore.Common;
using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;
using FoodStore.Interfaces;
using FoodStore.Models;

namespace FoodStore.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<ProductResponse>> GetAllAsync(ProductQueryParameters parameters)
    {
        (IEnumerable<Product> Items, int TotalCount) paginatedProducts = await _productRepository.GetAllAsync(parameters);

        IEnumerable<ProductResponse> data = _mapper.Map<IEnumerable<ProductResponse>>(paginatedProducts.Items);
        int totalPages = (int)Math.Ceiling(paginatedProducts.TotalCount / (double)parameters.PageSize);

        return new PagedResponse<ProductResponse>
        {
            Data = data,
            Page = parameters.Page,
            PageSize = parameters.PageSize,
            TotalCount = paginatedProducts.TotalCount,
            TotalPages = totalPages
        };
    }

    public async Task<ProductResponse?> GetByIdAsync(int id)
    {
        Product? product = await _productRepository.GetByIdAsync(id);
        return product == null ? null : _mapper.Map<ProductResponse>(product);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        bool categoryExists = await _productRepository.CategoryExistsAsync(request.CategoryId);
        if (!categoryExists)
        {
            throw new InvalidOperationException("Category not found.");
        }

        Product product = _mapper.Map<Product>(request);
        await _productRepository.AddAsync(product);
        return _mapper.Map<ProductResponse>(product);
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

        _mapper.Map(request, product);
        await _productRepository.UpdateAsync(product);
        return _mapper.Map<ProductResponse>(product);
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
}
