using FoodStoreIdentity.DTOs.Request;
using FoodStoreIdentity.DTOs.Response;

namespace FoodStoreIdentity.Interfaces;

public interface IAuthService
{
    Task<(bool Success, IEnumerable<string> Errors, UserResponse? User)> RegisterAsync(RegisterRequest request);
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}
