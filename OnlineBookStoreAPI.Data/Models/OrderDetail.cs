namespace OnlineBookStoreAPI.Data.Models;

public class OrderDetail
{
    public Guid Id { get; set; }
    
    public int Quantity { get; set; }

    public Book Book { get; set; } = new();

    public Order Order { get; set; } = new();
}
