using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyCleanArchitectureApp.Application.DTOs;

namespace MyCleanArchitectureApp.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> CreateOrder(OrderDto orderDto);
        Task<OrderDto> GetOrderById(Guid orderId);
        Task<IEnumerable<OrderDto>> GetAllOrders();
        Task UpdateOrder(OrderDto orderDto);
        Task DeleteOrder(Guid orderId);
        // Define other methods as needed
    }


}