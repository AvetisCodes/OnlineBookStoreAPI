using Microsoft.AspNetCore.Identity;

namespace OnlineBookStoreAPI.Data.Models;

public class User : IdentityUser
{   
    // Navigation properties
    public List<Order> Orders { get; set; } = new();

    public List<Review> Reviews { get; set; } = new();
}
