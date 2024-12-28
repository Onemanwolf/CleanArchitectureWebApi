using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCleanArchitectureApp.Application.DTOs;
using MyCleanArchitectureApp.Application.Interfaces;
using MyCleanArchitectureApp.Domain.Entities;

namespace MyCleanArchitectureApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(Guid id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }
            var createdOrderId = await _orderService.CreateOrder(orderDto);
            return Ok(createdOrderId);

        }

         [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] OrderDto orderDto)
    {
        if (id != orderDto.OrderId)
        {
            return BadRequest();
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {

            await _orderService.UpdateOrder(orderDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            await _orderService.DeleteOrder(id);
            return NoContent();
        }
    }
}