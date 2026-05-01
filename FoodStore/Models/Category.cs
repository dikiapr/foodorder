using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Models;

[Index(nameof(Name), IsUnique = true)]
public class Category
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
