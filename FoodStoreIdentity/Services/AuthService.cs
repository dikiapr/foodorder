using AutoMapper;
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

    public async Task<(bool Success, IEnumerable<string> Errors, UserResponse? User)> RegisterAsync(RegisterRequest request)
    {
        ApplicationUser user = _mapper.Map<ApplicationUser>(request);

        IdentityResult result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description), null);
        }

        await _userManager.AddToRoleAsync(user, "Customer");

        UserResponse userResponse = _mapper.Map<UserResponse>(user);
        userResponse.Role = "Customer";

        return (true, [], userResponse);
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return null;
        }

        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return null;
        }

        IList<string> roles = await _userManager.GetRolesAsync(user);
        string token = _jwtService.GenerateToken(user, roles);

        LoginResponse loginResponse = _mapper.Map<LoginResponse>(user);
        loginResponse.Role = roles.FirstOrDefault() ?? string.Empty;
        loginResponse.Token = token;

        return loginResponse;
    }
}
