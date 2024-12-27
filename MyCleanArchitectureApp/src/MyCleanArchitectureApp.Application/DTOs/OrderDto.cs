using System;
using System.Collections.Generic;


namespace MyCleanArchitectureApp.Application.DTOs
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Application.DTOs.OrderItemDto> OrderItemsDto { get; set; }
        public decimal TotalAmount { get; set; }
    }
}