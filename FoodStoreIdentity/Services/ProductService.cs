using AutoMapper;
using FoodStoreIdentity.DTOs;
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

    public async Task<ApiResponseDto<List<ProductResponse>>> GetAllAsync()
    {
        List<Product> products = await _productRepository.GetAllAsync();
        List<ProductResponse> data = _mapper.Map<List<ProductResponse>>(products);
        return ApiResponseDto<List<ProductResponse>>.SuccessResult(data, "Products retrieved successfully.");
    }

    public async Task<ApiResponseDto<ProductResponse>> GetByIdAsync(int id)
    {
        Product? product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return ApiResponseDto<ProductResponse>.ErrorResult("Product not found.");
        }

        ProductResponse data = _mapper.Map<ProductResponse>(product);
        return ApiResponseDto<ProductResponse>.SuccessResult(data, "Product retrieved successfully.");
    }

    public async Task<ApiResponseDto<ProductResponse>> CreateAsync(CreateProductRequest request)
    {
        bool categoryExists = await _productRepository.CategoryExistsAsync(request.CategoryId);
        if (!categoryExists)
        {
            return ApiResponseDto<ProductResponse>.ErrorResult("Category not found.");
        }

        Product product = _mapper.Map<Product>(request);
        await _productRepository.AddAsync(product);

        ProductResponse data = _mapper.Map<ProductResponse>(product);
        return ApiResponseDto<ProductResponse>.SuccessResult(data, "Product created successfully.");
    }

    public async Task<ApiResponseDto<ProductResponse>> UpdateAsync(int id, UpdateProductRequest request)
    {
        Product? product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return ApiResponseDto<ProductResponse>.ErrorResult("Product not found.");
        }

        bool categoryExists = await _productRepository.CategoryExistsAsync(request.CategoryId);
        if (!categoryExists)
        {
            return ApiResponseDto<ProductResponse>.ErrorResult("Category not found.");
        }

        _mapper.Map(request, product);
        await _productRepository.UpdateAsync(product);

        ProductResponse data = _mapper.Map<ProductResponse>(product);
        return ApiResponseDto<ProductResponse>.SuccessResult(data, "Product updated successfully.");
    }

    public async Task<ApiResponseDto<bool>> DeleteAsync(int id)
    {
        Product? product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return ApiResponseDto<bool>.ErrorResult("Product not found.");
        }

        await _productRepository.DeleteAsync(product);
        return ApiResponseDto<bool>.SuccessResult(true, "Product deleted successfully.");
    }
}
