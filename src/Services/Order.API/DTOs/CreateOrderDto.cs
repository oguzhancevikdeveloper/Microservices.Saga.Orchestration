using Order.API.Models;

namespace Order.API.DTOs;

public class CreateOrderDto
{
    public int BuyerId { get; set; }
    public List<OrderItem> OrderItems { get; set; } = default!;
}
