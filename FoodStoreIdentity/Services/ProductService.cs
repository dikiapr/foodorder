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
        try
        {
            List<Product> products = await _productRepository.GetAllAsync();
            List<ProductResponse> data = _mapper.Map<List<ProductResponse>>(products);
            return ApiResponseDto<List<ProductResponse>>.SuccessResult(data, "Products retrieved successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<List<ProductResponse>>.ErrorResult($"An unexpected error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<ProductResponse>> GetByIdAsync(int id)
    {
        try
        {
            Product? product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return ApiResponseDto<ProductResponse>.ErrorResult("Product not found.");
            }

            ProductResponse data = _mapper.Map<ProductResponse>(product);
            return ApiResponseDto<ProductResponse>.SuccessResult(data, "Product retrieved successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<ProductResponse>.ErrorResult($"An unexpected error occurred: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<ProductResponse>> CreateAsync(CreateProductRequest request, string? userId)
    {
        try
        {
            bool categoryExists = await _productRepository.CategoryExistsAsync(request.CategoryId);
            if (!categoryExists)
            {
                return ApiResponseDto<ProductResponse>.ErrorResult("Category not found.");
            }

            Product product = _mapper.Map<Product>(request);
            product.CreatedByUserId = userId;
            await _productRepository.AddAsync(product);

            ProductResponse data = _mapper.Map<ProductResponse>(product);
            return ApiResponseDto<ProductResponse>.SuccessResult(data, "Product created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<ProductResponse>.ErrorResult($"An unexpected error occurred during product creation: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<ProductResponse>> UpdateAsync(int id, UpdateProductRequest request)
    {
        try
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
        catch (Exception ex)
        {
            return ApiResponseDto<ProductResponse>.ErrorResult($"An unexpected error occurred during product update: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<bool>> DeleteAsync(int id)
    {
        try
        {
            Product? product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return ApiResponseDto<bool>.ErrorResult("Product not found.");
            }

            await _productRepository.DeleteAsync(product);
            return ApiResponseDto<bool>.SuccessResult(true, "Product deleted successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<bool>.ErrorResult($"An unexpected error occurred during product deletion: {ex.Message}");
        }
    }
}
