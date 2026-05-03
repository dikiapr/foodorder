using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;
using FoodStore.Interfaces;
using FoodStore.Models;

namespace FoodStore.Services;

public class CartItemService : ICartItemService
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;

    public CartItemService(ICartItemRepository cartItemRepository, IProductRepository productRepository)
    {
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<CartItemResponse>> GetCartAsync(int userId)
    {
        IEnumerable<CartItem> cartItems = await _cartItemRepository.GetByUserIdAsync(userId);
        IEnumerable<CartItemResponse> responses = cartItems.Select(ToResponse);
        return responses;
    }

    public async Task<CartItemResponse> AddOrUpdateAsync(int userId, AddCartItemRequest request)
    {
        Product? product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        CartItem? existing = await _cartItemRepository.GetByUserAndProductAsync(userId, request.ProductId);

        int totalQuantity = (existing?.Quantity ?? 0) + request.Quantity;
        if (totalQuantity > product.Stock)
        {
            throw new InvalidOperationException($"Insufficient stock. Available: {product.Stock}.");
        }

        if (existing != null)
        {
            existing.Quantity += request.Quantity;
            await _cartItemRepository.UpdateAsync(existing);
            CartItemResponse updatedResponse = ToResponse(existing);
            return updatedResponse;
        }

        CartItem cartItem = new CartItem()
        {
            UserId = userId,
            ProductId = request.ProductId,
            Quantity = request.Quantity
        };

        await _cartItemRepository.AddAsync(cartItem);
        CartItemResponse response = ToResponse(cartItem);
        return response;
    }

    public async Task<CartItemResponse?> UpdateQuantityAsync(int id, UpdateCartItemRequest request)
    {
        CartItem? cartItem = await _cartItemRepository.GetByIdAsync(id);
        if (cartItem == null)
        {
            return null;
        }

        Product? product = await _productRepository.GetByIdAsync(cartItem.ProductId);
        if (request.Quantity > product!.Stock)
        {
            throw new InvalidOperationException($"Insufficient stock. Available: {product.Stock}.");
        }

        cartItem.Quantity = request.Quantity;
        await _cartItemRepository.UpdateAsync(cartItem);
        CartItemResponse? response = ToResponse(cartItem);
        return response;
    }

    public async Task<bool> RemoveAsync(int id)
    {
        CartItem? cartItem = await _cartItemRepository.GetByIdAsync(id);
        if (cartItem == null)
        {
            return false;
        }

        await _cartItemRepository.DeleteAsync(cartItem);
        return true;
    }

    public async Task ClearCartAsync(int userId)
    {
        await _cartItemRepository.ClearByUserIdAsync(userId);
    }

    private static CartItemResponse ToResponse(CartItem cartItem)
    {
        return new CartItemResponse
        {
            Id = cartItem.Id,
            UserId = cartItem.UserId,
            ProductId = cartItem.ProductId,
            ProductName = cartItem.Product?.Name ?? string.Empty,
            ProductPrice = cartItem.Product?.Price ?? 0,
            ProductImageUrl = cartItem.Product?.ImageUrl,
            Quantity = cartItem.Quantity,
            Subtotal = (cartItem.Product?.Price ?? 0) * cartItem.Quantity
        };
    }
}
