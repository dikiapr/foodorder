using FoodStore.Models;

namespace FoodStore.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
