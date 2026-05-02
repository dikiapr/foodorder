using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;

namespace FoodStore.Interfaces;

public interface ICartItemService
{
    Task<IEnumerable<CartItemResponse>> GetCartAsync(int userId);
    Task<CartItemResponse> AddOrUpdateAsync(AddCartItemRequest request);
    Task<CartItemResponse?> UpdateQuantityAsync(int id, UpdateCartItemRequest request);
    Task<bool> RemoveAsync(int id);
    Task ClearCartAsync(int userId);
}
