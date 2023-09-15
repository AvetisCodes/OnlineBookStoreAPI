namespace OnlineBookStoreAPI.Data.Models;

public class Review
{
    public Guid Id { get; set; }
    
    public int Rating { get; set; }
    
    public string Text { get; set; } = string.Empty;

    public Book Book { get; set; } = new();

    public User User { get; set; } = new();
}
