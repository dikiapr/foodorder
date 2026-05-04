using System.ComponentModel.DataAnnotations;

namespace FoodStoreIdentity.DTOs.Request;

public class CreateCategoryRequest
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}
