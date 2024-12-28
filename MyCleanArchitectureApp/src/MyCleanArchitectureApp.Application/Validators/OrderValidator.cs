using FluentValidation;
using MyCleanArchitectureApp.Domain.Entities;

namespace MyCleanArchitectureApp.Application.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(order => order.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
            RuleFor(order => order.OrderDate).NotEmpty().WithMessage("Order date is required.");
            RuleFor(order => order.TotalAmount).GreaterThan(0).WithMessage("Total amount must be greater than zero.");
            RuleForEach(order => order.OrderItems).SetValidator(new OrderItemValidator());
        }
    }

    public class OrderItemValidator : AbstractValidator<OrderItem>
    {
        public OrderItemValidator()
        {
            RuleFor(orderItem => orderItem.OrderId).NotEmpty().WithMessage("OrderId is required."); // Ensure ProductId exists in OrderItem
            RuleFor(orderItem => orderItem.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            RuleFor(orderItem => orderItem.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
}