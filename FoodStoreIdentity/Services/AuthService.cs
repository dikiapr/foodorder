using AutoMapper;
using FoodStoreIdentity.DTOs;
using FoodStoreIdentity.DTOs.Request;
using FoodStoreIdentity.DTOs.Response;
using FoodStoreIdentity.Interfaces;
using FoodStoreIdentity.Models;
using Microsoft.AspNetCore.Identity;

namespace FoodStoreIdentity.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtService jwtService,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    public async Task<ApiResponseDto<UserResponse>> RegisterAsync(RegisterRequest request)
    {
        try
        {
            ApplicationUser? existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return ApiResponseDto<UserResponse>.ErrorResult("User with this email already exists.");
            }

            ApplicationUser user = _mapper.Map<ApplicationUser>(request);

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                List<string> errors = result.Errors.Select(e => e.Description).ToList();
                return ApiResponseDto<UserResponse>.ErrorResult("Registration failed.", errors);
            }

            await _userManager.AddToRoleAsync(user, "Customer");

            UserResponse userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Role = "Customer";

            return ApiResponseDto<UserResponse>.SuccessResult(userResponse, "Registration successful.");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<UserResponse>.ErrorResult($"An unexpected error occurred during registration: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<LoginResponse>> LoginAsync(LoginRequest request)
    {
        try
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return ApiResponseDto<LoginResponse>.ErrorResult("Invalid credentials.");
            }

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return ApiResponseDto<LoginResponse>.ErrorResult("Invalid credentials.");
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);
            string token = _jwtService.GenerateToken(user, roles);

            LoginResponse loginResponse = _mapper.Map<LoginResponse>(user);
            loginResponse.Role = roles.FirstOrDefault() ?? string.Empty;
            loginResponse.Token = token;

            return ApiResponseDto<LoginResponse>.SuccessResult(loginResponse, "Login successful.");
        }
        catch (Exception ex)
        {
            return ApiResponseDto<LoginResponse>.ErrorResult($"An unexpected error occurred during login: {ex.Message}");
        }
    }

    public async Task<ApiResponseDto<UserResponse>> GetCurrentUserAsync(string userId)
    {
        try
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ApiResponseDto<UserResponse>.ErrorResult("User not found.");
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);

            UserResponse userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Role = roles.FirstOrDefault() ?? string.Empty;

            return ApiResponseDto<UserResponse>.SuccessResult(userResponse);
        }
        catch (Exception ex)
        {
            return ApiResponseDto<UserResponse>.ErrorResult($"An unexpected error occurred: {ex.Message}");
        }
    }
}
