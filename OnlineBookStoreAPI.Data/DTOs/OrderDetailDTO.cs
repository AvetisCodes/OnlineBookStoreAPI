using System.ComponentModel.DataAnnotations;

namespace OnlineBookStoreAPI.Data.DTOs;

public class OrderDetailDTO
{
    [Required]
    public Guid BookId { get; set; }

    public int Quantity { get; set; }
}
