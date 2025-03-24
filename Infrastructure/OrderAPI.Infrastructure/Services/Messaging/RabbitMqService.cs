using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrderAPI.Application.Abstractions.IServices;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace OrderAPI.Infrastructure.Services.Messaging
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqService> _logger;

        public RabbitMqService(IConfiguration configuration, ILogger<RabbitMqService> logger)
        {
            _logger = logger;
            _factory = new()
            {
                HostName = configuration["RabbitMq:HostName"],
                Port = int.Parse(configuration["RabbitMq:Port"]),
                UserName = configuration["RabbitMq:UserName"],
                Password = configuration["RabbitMq:Password"]
            };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void PublishToQueue<T>(T message, string queueName)
        {

            _channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var jsonBody = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonBody);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: properties,
                body: body
            );
        }
        public void Consume<T>(string queueName, Func<T, Task> onMessage)
        {

            _channel.QueueDeclare(queue: queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null
            );

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                try
                {
                    var message = JsonSerializer.Deserialize<T>(messageJson);
                    if (message is not null)
                    {
                        await onMessage(message);
                        await Task.Delay(TimeSpan.FromSeconds(10));
                        _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "RabbitMQ consumer error: {Message}", ex.Message);
                }
            };

            _channel.BasicConsume(queue: queueName,
                                     autoAck: false,
                                     consumer: consumer
            );

        }
    }
}
