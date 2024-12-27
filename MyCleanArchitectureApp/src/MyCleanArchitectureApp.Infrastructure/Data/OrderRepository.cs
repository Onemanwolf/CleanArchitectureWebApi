using AutoMapper;
using MyCleanArchitectureApp.Domain.Entities;
using MyCleanArchitectureApp.Domain.Interfaces;
using MyCleanArchitectureApp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCleanArchitectureApp.Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddOrder(Order order)
        {
            var orderEntity = _mapper.Map<OrderEntity>(order);
            await _context.Orders.AddAsync(orderEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            var orderEntity = await _context.Orders.Include(o => o.OrderItems).AsNoTracking().FirstOrDefaultAsync(o => o.OrderId == orderId);
            return _mapper.Map<Order>(orderEntity);
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            var orderEntities = await _context.Orders.Include(o => o.OrderItems).AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<Order>>(orderEntities);
        }

        public async Task UpdateOrder(Order order)
        {
            var orderEntity = _mapper.Map<OrderEntity>(order);
            _context.Orders.Update(orderEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(Guid orderId)
        {
            var orderEntity = await _context.Orders.FindAsync(orderId);
            if (orderEntity != null)
            {
                _context.Orders.Remove(orderEntity);
                await _context.SaveChangesAsync();
            }
        }

        public void Detach(Order order)
        {
            var orderEntity = _mapper.Map<OrderEntity>(order);
            _context.Entry(orderEntity).State = EntityState.Detached;
        }
    }
}