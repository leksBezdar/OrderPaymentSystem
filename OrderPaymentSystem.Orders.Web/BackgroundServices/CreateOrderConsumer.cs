
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrderPaymentSystem.Orders.Application.Abstractions;
using OrderPaymentSystem.Orders.Application.Models.Orders;
using OrderPaymentSystem.Orders.Application.Services;
using OrderPaymentSystem.Orders.Domain.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace OrderPaymentSystem.Orders.Web.BackgroundServices;

public class CreateOrderConsumer : BackgroundService
{
    private readonly RabbitMqOptions _rabbitMqOptions;
    private readonly IChannel _channel;
    private readonly IServiceProvider _serviceProvider;

    public CreateOrderConsumer(IOptions<RabbitMqOptions> options, IServiceProvider serviceProvider)
    {
        _rabbitMqOptions = options.Value;
        _serviceProvider = serviceProvider;

        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqOptions.HostName,
            Port = _rabbitMqOptions.Port,
            UserName = _rabbitMqOptions.UserName,
            Password = _rabbitMqOptions.Password,
            VirtualHost = _rabbitMqOptions.VirtualHost,
        };

        var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());

            var createOrderDTO = JsonSerializer.Deserialize<CreateOrderDTO>(message, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            })!;

            using var scope = _serviceProvider.CreateScope();
            var _orderService = scope.ServiceProvider.GetRequiredService<IOrdersService>();

            await _orderService.Create(createOrderDTO);

            await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        };

        await _channel.BasicConsumeAsync(_rabbitMqOptions.CreateOrderQueueName, autoAck: false, consumer, cancellationToken: stoppingToken);
    }
}
