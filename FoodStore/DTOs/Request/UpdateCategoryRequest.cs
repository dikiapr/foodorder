using System.ComponentModel.DataAnnotations;

namespace FoodStore.DTOs.Request;

public class UpdateCategoryRequest
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}
