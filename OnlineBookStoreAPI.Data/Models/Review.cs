namespace OnlineBookStoreAPI.Data.Models;

public class Review
{
    public int Id { get; set; }
    
    public int BookId { get; set; }
    
    public int UserId { get; set; }
    
    public int Rating { get; set; }
    
    public string Text { get; set; } = string.Empty;

    // Navigational properties
    public Book Book { get; set; } = new();

    public User User { get; set; } = new();
}
