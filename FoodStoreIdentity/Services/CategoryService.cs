using FoodStoreIdentity.DTOs.Request;
using FoodStoreIdentity.DTOs.Response;
using FoodStoreIdentity.Interfaces;
using FoodStoreIdentity.Models;

namespace FoodStoreIdentity.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
    {
        IEnumerable<Category> categories = await _categoryRepository.GetAllAsync();
        return categories.Select(ToResponse);
    }

    public async Task<CategoryResponse?> GetByIdAsync(int id)
    {
        Category? category = await _categoryRepository.GetByIdAsync(id);
        return category == null ? null : ToResponse(category);
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
    {
        bool nameExists = await _categoryRepository.NameExistsAsync(request.Name);
        if (nameExists)
        {
            throw new InvalidOperationException("Category name already exists.");
        }
        Category category = new Category { Name = request.Name };
        await _categoryRepository.AddAsync(category);
        return ToResponse(category);
    }

    public async Task<CategoryResponse?> UpdateAsync(int id, UpdateCategoryRequest request)
    {
        Category? category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return null;
        }

        bool nameExists = await _categoryRepository.NameExistsAsync(request.Name, excludeId: id);
        if (nameExists)
        {
            throw new InvalidOperationException("Category name already exists.");
        }

        category.Name = request.Name;
        await _categoryRepository.UpdateAsync(category);
        return ToResponse(category);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Category? category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return false;
        }

        bool hasProducts = await _categoryRepository.HasProductsAsync(id);
        if (hasProducts)
        {
            throw new InvalidOperationException("Cannot delete category that has products.");
        }

        await _categoryRepository.DeleteAsync(category);
        return true;
    }

    private static CategoryResponse ToResponse(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name
    };
}
