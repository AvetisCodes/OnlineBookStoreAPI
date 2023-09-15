namespace OnlineBookStoreAPI.Data.Models;

public class Order
{
    public Guid Id { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public decimal TotalPrice { get; set; }

    public User User { get; set; } = null!;

    public List<OrderDetail> OrderDetails { get; set; } = new();
}
