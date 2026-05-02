namespace FoodStore.DTOs.Response;

public class OrderResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public List<OrderItemResponse> Items { get; set; } = new();
}
