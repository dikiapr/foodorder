using FoodStore.Common;
using FoodStore.Enums;

namespace FoodStore.DTOs.Request;

public class OrderQueryParameters : QueryParameters
{
    public OrderStatus? Status { get; set; }
    public string SortBy { get; set; } = "orderdate";
}
