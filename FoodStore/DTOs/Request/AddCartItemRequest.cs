using System.ComponentModel.DataAnnotations;

namespace FoodStore.DTOs.Request;

public class AddCartItemRequest
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
}
