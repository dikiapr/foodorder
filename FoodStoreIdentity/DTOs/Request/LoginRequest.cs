using System;
using System.ComponentModel.DataAnnotations;

namespace FoodStoreIdentity.DTOs.Request;

public class LoginRequest
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
