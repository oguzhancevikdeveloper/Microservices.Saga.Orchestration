using MassTransit;
using SagaOrche.Shared.Messages;

namespace SagaOrche.Shared.PaymentEvents;

public class PaymentFailedEvent : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; }

    public PaymentFailedEvent(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public string Message { get; set; }
    public List<OrderItemMessage> OrderItems { get; set; } = default!;
}
