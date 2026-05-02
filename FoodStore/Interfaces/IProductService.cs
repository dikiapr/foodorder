using FoodStore.Common;
using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;

namespace FoodStore.Interfaces;

public interface IProductService
{
    Task<PagedResponse<ProductResponse>> GetAllAsync(ProductQueryParameters parameters);
    Task<ProductResponse?> GetByIdAsync(int id);
    Task<ProductResponse> CreateAsync(CreateProductRequest request);
    Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request);
    Task<bool> DeleteAsync(int id);
}
