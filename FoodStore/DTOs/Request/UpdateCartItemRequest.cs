using System.ComponentModel.DataAnnotations;

namespace FoodStore.DTOs.Request;

public class UpdateCartItemRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
}
