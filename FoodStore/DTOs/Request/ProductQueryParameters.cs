using FoodStore.Common;

namespace FoodStore.DTOs.Request;

public class ProductQueryParameters : QueryParameters
{
    public string? Search { get; set; }
    public int? CategoryId { get; set; }
    public string SortBy { get; set; } = "name";
}
