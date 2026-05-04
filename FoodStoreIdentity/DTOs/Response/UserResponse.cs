using System;

namespace FoodStoreIdentity.DTOs.Response;

public class UserResponse
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? Address { get; set; }
    public string Role { get; set; } = string.Empty;
}
