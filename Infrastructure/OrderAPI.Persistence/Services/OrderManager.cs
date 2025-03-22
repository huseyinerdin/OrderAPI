using AutoMapper;
using OrderAPI.Application.Abstractions.IRepositories;
using OrderAPI.Application.Abstractions.IServices;
using OrderAPI.Application.DTOs;
using OrderAPI.Domain.Entities;
using OrderAPI.Infrastructure.Services.Messaging;

namespace OrderAPI.Persistence.Services
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderRepository _repo;
        private readonly IMapper _mapper;
        private readonly IRabbitMqService _rabbitMQ;

        public OrderManager(IOrderRepository repo, IMapper mapper, IRabbitMqService rabbitMQ)
        {
            _repo = repo;
            _mapper = mapper;
            _rabbitMQ = rabbitMQ;
        }

        public async Task<int> CreateOrderAsync(CreateOrderRequest request)
        {
            var order = _mapper.Map<Order>(request);

            await _repo.CreateAsync(order);
            await _repo.SaveChangesAsync();

            var mail = new SendMailMessage
            {
                To = request.CustomerEmail,
                Subject = "Order Confirmation",
                Body = $"Dear {request.CustomerName}, your order has been successfully received. Amount: {order.TotalAmount} ₺"
            };

            _rabbitMQ.PublishToQueue(mail, "SendMail");

            return order.Id;
        }
    }
}
