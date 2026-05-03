namespace FoodStore.Enums;

public enum OrderStatus
{
    Pending = 0,
    Processing = 1, // was Paid — admin confirms & prepares
    Completed = 2,
    Cancelled = 3,
    Delivery = 4    // admin dispatches, customer confirms receipt
}
