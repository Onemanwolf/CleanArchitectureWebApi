using AutoMapper;
using MyCleanArchitectureApp.Domain.Entities;
using MyCleanArchitectureApp.Application.DTOs;
using MyCleanArchitectureApp.Infrastructure.Entities;

namespace MyCleanArchitectureApp.Application.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderItemsDto, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, OrderItemDto>();

            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItemsDto));

            CreateMap<OrderItemDto, OrderItem>();

              // Domain to EF Core Entity
            CreateMap<Order, OrderEntity>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<OrderItem, OrderItemEntity>();

            // EF Core Entity to Domain
            CreateMap<OrderEntity, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<OrderItemEntity, OrderItem>();
        }
    }
}