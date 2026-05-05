using Microsoft.AspNetCore.Identity;

namespace FoodStoreIdentity.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
