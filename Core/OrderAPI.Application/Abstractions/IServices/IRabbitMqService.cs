namespace OrderAPI.Application.Abstractions.IServices
{
    public interface IRabbitMqService
    {
        void PublishToQueue<T>(T message, string queueName);
    }
}
