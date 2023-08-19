namespace OnlineBookStoreAPI.Data.DTOs;

public class OrderDetailDTO
{
    public Guid BookId { get; private set; }

    public int Quantity { get; set; }
}
