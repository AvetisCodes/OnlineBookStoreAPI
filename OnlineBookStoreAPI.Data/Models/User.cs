using Microsoft.AspNetCore.Identity;

namespace OnlineBookStoreAPI.Data.Models;

public class User : IdentityUser<Guid>
{   
    public List<Order> Orders { get; set; } = new();

    public List<Review> Reviews { get; set; } = new();
}
