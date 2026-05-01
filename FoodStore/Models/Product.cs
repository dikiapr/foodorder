using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Precision(10, 2)]
    public decimal Price { get; set; }

    public int Stock { get; set; }
    public string? ImageUrl { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
