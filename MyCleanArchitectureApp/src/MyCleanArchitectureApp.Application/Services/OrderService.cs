using AutoMapper;
using MyCleanArchitectureApp.Domain.Entities;
using MyCleanArchitectureApp.Domain.Interfaces;
using MyCleanArchitectureApp.Application.DTOs;
using MyCleanArchitectureApp.Application.Interfaces;
using MyCleanArchitectureApp.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; // Add this for Encoding
using System.Threading.Tasks;

namespace MyCleanArchitectureApp.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IAzureQueueService _queueService;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IAzureQueueService queueService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _queueService = queueService;
        }

        public async Task<Guid> CreateOrder(OrderDto orderDto)
        {
            if (orderDto == null)
            {
                throw new ArgumentNullException(nameof(orderDto), "The orderDto field is required.");
            }

            var order = _mapper.Map<Order>(orderDto);
            order.OrderId = Guid.NewGuid();
            order.OrderDate = DateTime.UtcNow;

            var newOrder = await order.InitializeOrder(order);

            await _orderRepository.AddOrder(newOrder);

            var message = Convert.ToBase64String(Encoding.UTF8.GetBytes($"Order created: {newOrder.OrderId}"));
            await _queueService.SendMessageAsync(message);

            return newOrder.OrderId;
        }

        public async Task<OrderDto> GetOrderById(Guid orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                return null;
            }

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            if (orders == null)
            {
                return Enumerable.Empty<OrderDto>();
            }

            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task UpdateOrder(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            var updatedOrder = await order.UpdateOrder(order);
            await _orderRepository.UpdateOrder(updatedOrder);
        }

        public async Task DeleteOrder(Guid orderId)
        {
            await _orderRepository.DeleteOrder(orderId);
        }
    }
}