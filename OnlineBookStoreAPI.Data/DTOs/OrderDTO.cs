namespace OnlineBookStoreAPI.Data.DTOs;

public class OrderDTO
{
    public DateTime OrderDate { get; set; }

    public List<OrderDetailDTO> OrderDetailDTOs { get; set; } = new();
}
