using FoodStoreIdentity.Models;

namespace FoodStoreIdentity.Interfaces;

public interface IJwtService
{
    string GenerateToken(ApplicationUser user, IList<string> roles);
}
