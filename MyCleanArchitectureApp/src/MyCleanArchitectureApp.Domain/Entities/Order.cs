using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCleanArchitectureApp.Domain.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public decimal TotalAmount {get; set;}

        public async Task<Order> InitializeOrder(Order order)
        {
            order.OrderId = Guid.NewGuid();
            order.CustomerId = order.CustomerId;
            order.OrderDate = DateTime.UtcNow;
            order.OrderItems = await Task.WhenAll(order.OrderItems.Select(async item => new OrderItem
            {
                OrderItemId = Guid.NewGuid(),
                OrderId = order.OrderId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                Price = item.Price,

            }).ToList());

            order.TotalAmount = order.OrderItems.Sum(item => item.Price * item.Quantity);
            order.TotalAmount = await AddTax(order.TotalAmount);

            var isValidOrder = await IsValid(order);
            if (!isValidOrder)
            {
                throw new InvalidOperationException("Invalid order.");
            }
            return order;
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            var _order = new Order();
            _order.OrderId = order.OrderId;
            _order.CustomerId = order.CustomerId;
            _order.OrderDate = order.OrderDate;

            _order.OrderItems = await Task.WhenAll(order.OrderItems.Select(async item => new OrderItem
            {
                OrderItemId = item.OrderItemId,
                OrderId = order.OrderId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                Price = item.Price,

            }).ToList());
            _order.TotalAmount = _order.OrderItems.Sum(item => item.Price * item.Quantity);
            _order.TotalAmount = await AddTax(_order.TotalAmount);

            var isValidOrder = await IsValid(_order);
            if (!isValidOrder)
            {
                throw new InvalidOperationException("Invalid order.");
            }
            return _order;
        }

        public async Task<bool> IsValid(Order order)
        {
            if (order.CustomerId == Guid.Empty)
            {
                return await Task.Run(() => { return false;});
            }
            if (order.OrderItems == null || order.OrderItems.Count == 0)
            {
                return await Task.Run(() => {return false;});
            }
            return await Task.Run(() =>{ return true;});
        }

        public async Task<decimal> AddTax(decimal price)
        {
            // Simulate an asynchronous operation

            return await Task.Run(() => { return price * 1.1m;}); // Assuming a 10% tax rate
        }
    }
}
