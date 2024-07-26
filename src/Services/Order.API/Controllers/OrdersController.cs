using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.API.Context;
using Order.API.DTOs;
using SagaOrche.Shared.OrderEvents;
using SagaOrche.Shared.Settings;

namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(OrderDbContext _context, ISendEndpointProvider _sendEndpointProvider) : ControllerBase
{

    [HttpPost("create-order")]
    public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
    {
        Models.Order order = new()
        {
            BuyerId = createOrderDto.BuyerId,
            CreatedDate = DateTime.Now,
            OrderStatus = Enums.OrderStatus.Suspend,
            TotalPrice = createOrderDto.OrderItems.Sum(oi => oi.Count * oi.Price),
            OrderItems = createOrderDto.OrderItems.Select(oi => new Models.OrderItem
            {
                Price = oi.Price,
                Count = oi.Count,
                ProductId = oi.ProductId,
            }).ToList(),
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        OrderStartedEvent orderStartedEvent = new()
        {
            BuyerId = order.BuyerId,
            OrderId = order.Id,
            OrderItems = order.OrderItems.Select(oi => new SagaOrche.Shared.Messages.OrderItemMessage
            {
                Count = oi.Count,
                Price = oi.Price,
                ProductId = oi.ProductId,
            }).ToList(),
        };

        var sendPoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));
        await sendPoint.Send<OrderStartedEvent>(orderStartedEvent);
        return Ok();
    }
}
