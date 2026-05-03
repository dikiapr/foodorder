using System.Security.Claims;
using FoodStore.Common;
using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;
using FoodStore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<OrderResponse>>> GetAll([FromQuery] OrderQueryParameters parameters, [FromQuery] int? userId = null)
    {
        bool isAdmin = User.IsInRole("Admin");
        int claimsUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        int? filterUserId = isAdmin ? userId : claimsUserId;

        PagedResponse<OrderResponse> orders = await _orderService.GetAllAsync(filterUserId, parameters);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetById([FromRoute] int id)
    {
        OrderResponse? order = await _orderService.GetByIdAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<OrderResponse>> Checkout()
    {
        try
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            OrderResponse order = await _orderService.CheckoutAsync(userId);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<OrderResponse>> UpdateStatus([FromRoute] int id, [FromBody] UpdateOrderStatusRequest request)
    {
        OrderResponse? order = await _orderService.UpdateStatusAsync(id, request);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }
}
