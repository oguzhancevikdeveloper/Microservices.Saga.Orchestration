namespace SagaOrche.Shared.Messages;

public class StockRollbackMessage
{
    public List<OrderItemMessage> OrderItems { get; set; } = default!;
}
