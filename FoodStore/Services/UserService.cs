using AutoMapper;
using FoodStore.DTOs.Request;
using FoodStore.DTOs.Response;
using FoodStore.Interfaces;
using FoodStore.Models;

namespace FoodStore.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IJwtService jwtService, IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _mapper = mapper;
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

        User user = _mapper.Map<User>(request);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        await _userRepository.AddAsync(user);
        return _mapper.Map<UserResponse>(user);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        User? user = await _userRepository.GetByUsernameOrEmailAsync(request.UsernameOrEmail);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        string token = _jwtService.GenerateToken(user);
        LoginResponse response = _mapper.Map<LoginResponse>(user);
        response.Token = token;
        return response;
    }
}
