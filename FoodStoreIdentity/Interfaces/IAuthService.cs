using FoodStoreIdentity.DTOs;
using FoodStoreIdentity.DTOs.Request;
using FoodStoreIdentity.DTOs.Response;

namespace FoodStoreIdentity.Interfaces;

public interface IAuthService
{
    Task<ApiResponseDto<UserResponse>> RegisterAsync(RegisterRequest request);
    Task<ApiResponseDto<LoginResponse>> LoginAsync(LoginRequest request);
}
