using System.ComponentModel.DataAnnotations;

namespace FoodStore.DTOs.Request;

public class CheckoutRequest
{
    [Required]
    public int UserId { get; set; }
}
