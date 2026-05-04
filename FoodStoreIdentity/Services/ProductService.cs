using AutoMapper;
using FoodStoreIdentity.DTOs.Request;
using FoodStoreIdentity.DTOs.Response;
using FoodStoreIdentity.Interfaces;
using FoodStoreIdentity.Models;

namespace FoodStoreIdentity.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductResponse>> GetAllAsync()
    {
        IEnumerable<Product> products = await _productRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductResponse>>(products);
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

        await _productRepository.DeleteAsync(product);
        return true;
    }
}
