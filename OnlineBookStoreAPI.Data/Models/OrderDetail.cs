namespace OnlineBookStoreAPI.Data.Models;

public class OrderDetail
{
    public string Id { get; set; } = string.Empty;
    
    public string OrderId { get; set; } = string.Empty;
    
    public string BookId { get; set; } = string.Empty;
    
    public int Quantity { get; set; }

    // Navigational properties
    public Book Book { get; set; } = new();

    public Order Order { get; set; } = new();
}
