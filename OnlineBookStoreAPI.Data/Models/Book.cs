namespace OnlineBookStoreAPI.Data.Models;

public class Book
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    
    public string Author { get; set; } = string.Empty;

    public string ISBN { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public List<OrderDetail> OrderDetails { get; set; } = new();

    public List<Review> Reviews { get; set; } = new();
}
