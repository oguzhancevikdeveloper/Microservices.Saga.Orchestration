using MassTransit;
using SagaOrche.Shared.PaymentEvents;
using SagaOrche.Shared.Settings;

namespace Payment.API.Consumers;

public class PaymentStartedEventConsumer(ISendEndpointProvider _sendEndpointProvider) : IConsumer<PaymentStartedEvent>
{
    public async Task Consume(ConsumeContext<PaymentStartedEvent> context)
    {
        Random random = new Random();
        int number = random.Next(0, 100);


        var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));
        if (number % 2 == 0)
        {
            PaymentCompletedEvent paymentCompletedEvent = new(context.Message.CorrelationId) { };
            await sendEndpoint.Send(paymentCompletedEvent);
        }
        else
        {
            PaymentFailedEvent paymentFailedEvent = new(context.Message.CorrelationId)
            {
                Message = "Yetersiz bakiye...",
                OrderItems = context.Message.OrderItems
            };

            await sendEndpoint.Send(paymentFailedEvent);
        }
    }
}
