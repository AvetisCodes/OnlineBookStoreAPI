namespace OnlineBookStoreAPI.Data.Models;

public class OrderDetail
{
    public Guid Id { get; set; }
    
    public Guid OrderId { get; set; }
    
    public Guid BookId { get; set; }
    
    public int Quantity { get; set; }

    // Navigational properties
    public Book Book { get; set; } = new();

    public Order Order { get; set; } = new();
}
