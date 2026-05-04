using FoodStoreIdentity.DTOs.Request;
using FoodStoreIdentity.DTOs.Response;
using FoodStoreIdentity.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodStoreIdentity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest request)
    {
        (bool success, IEnumerable<string> errors, UserResponse? user) = await _authService.RegisterAsync(request);
        if (!success)
        {
            return BadRequest(errors);            
        }
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        LoginResponse? response = await _authService.LoginAsync(request);
        if (response == null)
        {
            return Unauthorized("Invalid credentials.");            
        }
        return Ok(response);
    }
}
