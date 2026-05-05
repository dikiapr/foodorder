using FoodStoreIdentity.DTOs;
using FoodStoreIdentity.DTOs.Request;
using FoodStoreIdentity.DTOs.Response;

namespace FoodStoreIdentity.Interfaces;

public interface ICategoryService
{
    Task<ApiResponseDto<List<CategoryResponse>>> GetAllAsync();
    Task<ApiResponseDto<CategoryResponse>> GetByIdAsync(int id);
    Task<ApiResponseDto<CategoryResponse>> CreateAsync(CreateCategoryRequest request);
    Task<ApiResponseDto<CategoryResponse>> UpdateAsync(int id, UpdateCategoryRequest request);
    Task<ApiResponseDto<bool>> DeleteAsync(int id);
}
