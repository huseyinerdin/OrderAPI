using OrderAPI.Application.Abstractions.IServices;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderAPI.Infrastructure.Services.Messaging
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqService(IConfiguration configuration)
        {
            _factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMq:HostName"],
                Port = int.Parse(configuration["RabbitMq:Port"]),
                UserName = configuration["RabbitMq:UserName"],
                Password = configuration["RabbitMq:Password"]
            };
        }

        public void PublishToQueue<T>(T message, string queueName)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var jsonBody = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonBody);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: properties,
                body: body
            );
        }
        public void Consume<T>(string queueName, Func<T, Task> onMessage)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                try
                {
                    var message = JsonSerializer.Deserialize<T>(messageJson);
                    if (message != null)
                    {
                        await onMessage(message);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"RabbitMQ tüketim hatası: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer
            );
        }
    }
}
