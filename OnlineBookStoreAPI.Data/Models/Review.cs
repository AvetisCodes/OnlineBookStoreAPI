namespace OnlineBookStoreAPI.Data.Models;

public class Review
{
    public string Id { get; set; } = string.Empty;
    
    public string BookId { get; set; } = string.Empty;
    
    public string UserId { get; set; } = string.Empty;
    
    public int Rating { get; set; }
    
    public string Text { get; set; } = string.Empty;

    // Navigational properties
    public Book Book { get; set; } = new();

    public User User { get; set; } = new();
}
