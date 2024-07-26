using MassTransit;
using SagaOrche.Shared.Messages;

namespace SagaOrche.Shared.PaymentEvents;

public class PaymentStartedEvent : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; }
    public PaymentStartedEvent(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public decimal TotalPrice { get; set; }
    public List<OrderItemMessage> OrderItems { get; set; } = default!;
}