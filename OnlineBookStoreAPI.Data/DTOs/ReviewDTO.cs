namespace OnlineBookStoreAPI.Data.DTOs;

public class ReviewDTO
{
    public Guid BookId { get; set; }

    public Guid UserId { get; set; }

    public int Rating { get; set; }

    public string Text { get; set; } = string.Empty;
}

