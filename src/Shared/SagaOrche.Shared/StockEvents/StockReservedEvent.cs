﻿using MassTransit;
using SagaOrche.Shared.Messages;

namespace SagaOrche.Shared.StockEvents;

public class StockReservedEvent : CorrelatedBy<Guid>
{
    public StockReservedEvent(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CorrelationId { get; }
    public List<OrderItemMessage> OrderItems { get; set; }
}