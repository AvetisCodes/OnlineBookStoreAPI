namespace OnlineBookStoreAPI.Data.Models;

public class Order
{
    public string Id { get; set; } = string.Empty;
    
    public string UserId { get; set; } = string.Empty;
    
    public DateTime OrderDate { get; set; }
    
    public double TotalPrice { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;

    public List<OrderDetail> OrderDetails { get; set; } = new();
}
