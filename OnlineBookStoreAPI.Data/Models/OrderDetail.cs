namespace OnlineBookStoreAPI.Data.Models;

public class OrderDetail
{
    public int Id { get; set; }
    
    public int OrderId { get; set; }
    
    public int BookId { get; set; }
    
    public int Quantity { get; set; }

    // Navigational properties
    public Book Book { get; set; } = new();

    public Order Order { get; set; } = new();
}
