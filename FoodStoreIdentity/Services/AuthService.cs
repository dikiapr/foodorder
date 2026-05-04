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

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    public async Task<(bool Success, IEnumerable<string> Errors, UserResponse? User)> RegisterAsync(RegisterRequest request)
    {
        ApplicationUser user = new ApplicationUser()
        {
            UserName = request.Username,
            Email = request.Email
        };

        IdentityResult result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description), null);
        }

        await _userManager.AddToRoleAsync(user, "Customer");

        return (true, [], new UserResponse()
        {
            Id = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            Role = "Customer"
        });
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return null;

        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded) return null;

        IList<string> roles = await _userManager.GetRolesAsync(user);
        string token = _jwtService.GenerateToken(user, roles);

        return new LoginResponse()
        {
            Id = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            Role = roles.FirstOrDefault() ?? string.Empty,
            Token = token
        };
    }
}
