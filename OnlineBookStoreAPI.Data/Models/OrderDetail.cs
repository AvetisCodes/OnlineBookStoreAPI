namespace OnlineBookStoreAPI.Data.Models;

public class OrderDetail
{
    public Guid Id { get; set; }
    
    public Guid OrderId { get; private set; }
    
    public Guid BookId { get; private set; }
    
    public int Quantity { get; set; }

    public Book Book { get; set; } = new();

    public Order Order { get; set; } = new();
}
