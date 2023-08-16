namespace OnlineBookStoreAPI.Data.Models;

public class Order
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public double TotalPrice { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;

    public List<OrderDetail> OrderDetails { get; set; } = new();
}
