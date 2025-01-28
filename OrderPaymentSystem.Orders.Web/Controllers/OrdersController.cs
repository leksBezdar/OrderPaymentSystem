using Microsoft.AspNetCore.Mvc;
using OrderPaymentSystem.Orders.Application.Abstractions;
using OrderPaymentSystem.Orders.Application.Models.Orders;
using System.Text.Json;

namespace OrderPaymentSystem.Orders.Web.Controllers;

[Route("api/orders")]
public class OrdersController(IOrdersService ordersService, ILogger<OrdersController> logger) : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDTO request)
    {
        logger.LogInformation($"Method api/orders Create started. Reqest: {JsonSerializer.Serialize(request)}");

        var result = await ordersService.Create(request);

        logger.LogInformation($"Method api/orders Create finished. Response: {JsonSerializer.Serialize(result)}");


        return Ok(result);
    }

    [HttpGet("{orderId:long}")]
    public async Task<IActionResult> GetById(long orderId)
    {
        logger.LogInformation($"Method api/orders/{orderId} started.");

        var result = await ordersService.GetById(orderId);

        logger.LogInformation($"Method api/orders/{orderId} finished. Response: {JsonSerializer.Serialize(result)}");

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        logger.LogInformation($"Method api/orders/ started.");

        var result = await ordersService.GetAll();

        logger.LogInformation($"Method api/orders/ finished. Result count: {result.Count}");

        return Ok(new
        {
            TotalCount = result.Count,
            Orders = result
        });
    }

    [HttpGet("customers/{customerId:long}")]
    public async Task<IActionResult> GetByUser(long customerId)
    {
        logger.LogInformation($"Method api/orders/customers/{customerId} started.");

        var result = await ordersService.GetByUser(customerId);

        logger.LogInformation($"Method api/orders/customers/{customerId} finished. Result count: {result.Count}");

        return Ok(new
        {
            TotalCount = result.Count,
            Orders = result
        });
    }

    [HttpPatch("{orderId:long}/reject")]
    public async Task<IActionResult> Reject(long orderId)
    {
        await ordersService.Reject(orderId);
        return Ok();
    }
}

