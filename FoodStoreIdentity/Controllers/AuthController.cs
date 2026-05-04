using FoodStoreIdentity.DTOs.Request;
using FoodStoreIdentity.DTOs.Response;
using FoodStoreIdentity.Interfaces;
using FoodStoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodStoreIdentity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest request)
    {
        ApplicationUser user = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email
        };

        // UserManager.CreateAsync otomatis hash password — tidak perlu BCrypt manual
        IdentityResult result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(error => error.Description));
        }

        await _userManager.AddToRoleAsync(user, "Customer");

        return Ok(new UserResponse
        {
            Id = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            Role = "Customer"
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Unauthorized("Invalid credentials.");
        }

        // SignInManager.CheckPasswordSignInAsync memvalidasi password tanpa membuat session cookie
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return Unauthorized("Invalid credentials.");
        }

        // GetRolesAsync mengambil roles dari tabel AspNetUserRoles
        IList<string> roles = await _userManager.GetRolesAsync(user);
        string token = _jwtService.GenerateToken(user, roles);

        return Ok(new LoginResponse
        {
            Id = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            Role = roles.FirstOrDefault() ?? string.Empty,
            Token = token
        });
    }
}
