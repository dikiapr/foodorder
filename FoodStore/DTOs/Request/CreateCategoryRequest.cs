using System.ComponentModel.DataAnnotations;

namespace FoodStore.DTOs.Request;

public class CreateCategoryRequest
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}
