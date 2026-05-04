using AutoMapper;
using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;
using FoodStore.Interfaces;
using FoodStore.Models;

namespace FoodStore.Services;

public class CartItemService : ICartItemService
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CartItemService(ICartItemRepository cartItemRepository, IProductRepository productRepository, IMapper mapper)
    {
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CartItemResponse>> GetCartAsync(int userId)
    {
        IEnumerable<CartItem> cartItems = await _cartItemRepository.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<CartItemResponse>>(cartItems);
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
            return _mapper.Map<CartItemResponse>(existing);
        }

        CartItem cartItem = _mapper.Map<CartItem>(request);
        cartItem.UserId = userId;
        await _cartItemRepository.AddAsync(cartItem);
        return _mapper.Map<CartItemResponse>(cartItem);
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
        return _mapper.Map<CartItemResponse>(cartItem);
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
}
