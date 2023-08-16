namespace OnlineBookStoreAPI.Data.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;

    // Navigation properties
    public List<Order> Orders { get; set; } = new();

    public List<Review> Reviews { get; set; } = new();
}
