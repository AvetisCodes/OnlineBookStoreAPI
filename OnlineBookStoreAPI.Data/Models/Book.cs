namespace OnlineBookStoreAPI.Data.Models;

public class Book
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
    
    public string Author { get; set; } = string.Empty;

    public string ISBN { get; set; } = string.Empty;

    public double Price { get; set; }

    // Navigational properties
    public List<OrderDetail> OrderDetails { get; set; } = new();

    public List<Review> Reviews { get; set; } = new();
}
