using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;
using FoodStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartItemController : ControllerBase
{
    private readonly ICartItemService _cartItemService;

    public CartItemController(ICartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CartItemResponse>>> GetCart([FromQuery] int userId)
    {
        IEnumerable<CartItemResponse> items = await _cartItemService.GetCartAsync(userId);
        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<CartItemResponse>> AddOrUpdate([FromBody] AddCartItemRequest request)
    {
        try
        {
            CartItemResponse item = await _cartItemService.AddOrUpdateAsync(request);
            return Ok(item);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CartItemResponse>> UpdateQuantity([FromRoute] int id, [FromBody] UpdateCartItemRequest request)
    {
        CartItemResponse? item = await _cartItemService.UpdateQuantityAsync(id, request);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove([FromRoute] int id)
    {
        bool removed = await _cartItemService.RemoveAsync(id);

        if (!removed)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearCart([FromQuery] int userId)
    {
        await _cartItemService.ClearCartAsync(userId);
        return NoContent();
    }
}
