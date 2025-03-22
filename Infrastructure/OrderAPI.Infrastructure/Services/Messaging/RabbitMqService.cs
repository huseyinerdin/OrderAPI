using OrderAPI.Application.Abstractions.IServices;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;

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
    }
}
