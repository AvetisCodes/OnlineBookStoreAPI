namespace OnlineBookStoreAPI.Data.Models;

public class Order
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; private set; }
    
    public DateTime OrderDate { get; set; }
    
    public double TotalPrice { get; set; }

    public User User { get; set; } = null!;

    public List<OrderDetail> OrderDetails { get; set; } = new();
}
