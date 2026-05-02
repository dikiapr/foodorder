using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;
using FoodStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetByUserId([FromQuery] int userId)
    {
        IEnumerable<OrderResponse> orders = await _orderService.GetByUserIdAsync(userId);
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
    public async Task<ActionResult<OrderResponse>> Checkout([FromBody] CheckoutRequest request)
    {
        try
        {
            OrderResponse order = await _orderService.CheckoutAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/status")]
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
