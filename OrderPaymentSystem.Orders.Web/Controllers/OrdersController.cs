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
}

