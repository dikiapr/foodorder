using FoodStoreIdentity.DTOs;
using FoodStoreIdentity.DTOs.Request;
using FoodStoreIdentity.DTOs.Response;

namespace FoodStoreIdentity.Interfaces;

public interface IProductService
{
    Task<ApiResponseDto<List<ProductResponse>>> GetAllAsync();
    Task<ApiResponseDto<ProductResponse>> GetByIdAsync(int id);
    Task<ApiResponseDto<ProductResponse>> CreateAsync(CreateProductRequest request);
    Task<ApiResponseDto<ProductResponse>> UpdateAsync(int id, UpdateProductRequest request);
    Task<ApiResponseDto<bool>> DeleteAsync(int id);
}
