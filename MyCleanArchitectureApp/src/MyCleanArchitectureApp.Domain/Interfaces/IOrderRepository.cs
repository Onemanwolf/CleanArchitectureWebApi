using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyCleanArchitectureApp.Domain.Entities;

namespace MyCleanArchitectureApp.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrder(Order order);
        Task<Order> GetOrderById(Guid orderId);
        Task<IEnumerable<Order>> GetAllOrders();
        Task UpdateOrder(Order order);
        Task DeleteOrder(Guid orderId);
        void Detach(Order order); // Add this line
    }
}