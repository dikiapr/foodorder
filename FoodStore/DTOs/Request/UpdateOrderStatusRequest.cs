using System.ComponentModel.DataAnnotations;
using FoodStore.Enums;

namespace FoodStore.DTOs.Request;

public class UpdateOrderStatusRequest
{
    [Required]
    public OrderStatus Status { get; set; }
}
