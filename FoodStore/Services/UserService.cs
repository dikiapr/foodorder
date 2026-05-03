using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;
using FoodStore.Interfaces;
using FoodStore.Models;

namespace FoodStore.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<UserResponse> RegisterAsync(RegisterRequest request)
    {
        bool usernameExists = await _userRepository.UsernameExistsAsync(request.Username);
        if (usernameExists)
        {
            throw new InvalidOperationException("Username already taken.");
        }

        bool emailExists = await _userRepository.EmailExistsAsync(request.Email);
        if (emailExists)
        {
            throw new InvalidOperationException("Email already registered.");
        }

        User user = new User()
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _userRepository.AddAsync(user);
        UserResponse response = ToUserResponse(user);
        return response;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        User? user = await _userRepository.GetByUsernameOrEmailAsync(request.UsernameOrEmail);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        string token = _jwtService.GenerateToken(user);
        LoginResponse response = ToLoginResponse(user, token);
        return response;
    }

    private static UserResponse ToUserResponse(User user)
    {
        return new UserResponse()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };
    }

    private static LoginResponse ToLoginResponse(User user, string token)
    {
        return new LoginResponse()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = token
        };
    }
}
