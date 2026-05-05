using AutoMapper;
using FoodStoreIdentity.DTOs;
using FoodStoreIdentity.DTOs.Request;
using FoodStoreIdentity.DTOs.Response;
using FoodStoreIdentity.Interfaces;
using FoodStoreIdentity.Models;

namespace FoodStoreIdentity.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponseDto<List<CategoryResponse>>> GetAllAsync()
    {
        List<Category> categories = await _categoryRepository.GetAllAsync();
        List<CategoryResponse> data = _mapper.Map<List<CategoryResponse>>(categories);
        return ApiResponseDto<List<CategoryResponse>>.SuccessResult(data, "Categories retrieved successfully.");
    }

    public async Task<ApiResponseDto<CategoryResponse>> GetByIdAsync(int id)
    {
        Category? category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return ApiResponseDto<CategoryResponse>.ErrorResult("Category not found.");
        }

        CategoryResponse data = _mapper.Map<CategoryResponse>(category);
        return ApiResponseDto<CategoryResponse>.SuccessResult(data, "Category retrieved successfully.");
    }

    public async Task<ApiResponseDto<CategoryResponse>> CreateAsync(CreateCategoryRequest request)
    {
        bool nameExists = await _categoryRepository.NameExistsAsync(request.Name);
        if (nameExists)
        {
            return ApiResponseDto<CategoryResponse>.ErrorResult("Category name already exists.");
        }

        Category category = _mapper.Map<Category>(request);
        await _categoryRepository.AddAsync(category);

        CategoryResponse data = _mapper.Map<CategoryResponse>(category);
        return ApiResponseDto<CategoryResponse>.SuccessResult(data, "Category created successfully.");
    }

    public async Task<ApiResponseDto<CategoryResponse>> UpdateAsync(int id, UpdateCategoryRequest request)
    {
        Category? category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return ApiResponseDto<CategoryResponse>.ErrorResult("Category not found.");
        }

        bool nameExists = await _categoryRepository.NameExistsAsync(request.Name, excludeId: id);
        if (nameExists)
        {
            return ApiResponseDto<CategoryResponse>.ErrorResult("Category name already exists.");
        }

        _mapper.Map(request, category);
        await _categoryRepository.UpdateAsync(category);

        CategoryResponse data = _mapper.Map<CategoryResponse>(category);
        return ApiResponseDto<CategoryResponse>.SuccessResult(data, "Category updated successfully.");
    }

    public async Task<ApiResponseDto<bool>> DeleteAsync(int id)
    {
        Category? category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return ApiResponseDto<bool>.ErrorResult("Category not found.");
        }

        bool hasProducts = await _categoryRepository.HasProductsAsync(id);
        if (hasProducts)
        {
            return ApiResponseDto<bool>.ErrorResult("Cannot delete category that has products.");
        }

        await _categoryRepository.DeleteAsync(category);
        return ApiResponseDto<bool>.SuccessResult(true, "Category deleted successfully.");
    }
}
