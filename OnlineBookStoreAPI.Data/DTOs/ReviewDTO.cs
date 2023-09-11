using System.ComponentModel.DataAnnotations;

namespace OnlineBookStoreAPI.Data.DTOs;

public class ReviewDTO
{
    [Range(1, 5)]
    [Required]
    public int Rating { get; set; }

    public string Text { get; set; } = string.Empty;
}

