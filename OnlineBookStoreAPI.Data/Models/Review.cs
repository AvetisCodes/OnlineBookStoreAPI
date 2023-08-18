namespace OnlineBookStoreAPI.Data.Models;

public class Review
{
    public Guid Id { get; set; }
    
    public Guid BookId { get; set; }
    
    public Guid UserId { get; set; }
    
    public int Rating { get; set; }
    
    public string Text { get; set; } = string.Empty;

    // Navigational properties
    public Book Book { get; set; } = new();

    public User User { get; set; } = new();
}
