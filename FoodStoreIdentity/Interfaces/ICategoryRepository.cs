using FoodStoreIdentity.Models;

namespace FoodStoreIdentity.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Category category);
    Task<bool> NameExistsAsync(string name, int? excludeId = null);
    Task<bool> HasProductsAsync(int categoryId);
}
