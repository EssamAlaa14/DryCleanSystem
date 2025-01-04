using Application.Common.Extensions;
using AutoMapper;
using Domain.Entities.Orders;
using Domain.Enums;

namespace Application.Common.DTOs.InfoDtos
{
    public class OrderDto(
        Guid id,
        Guid customerId,
        OrderType orderType,
        OrderStatus orderStatus,
        string? notes
        )
    {
        public Guid Id { get; set; } = id;
        public Guid CustomerId { get; set; } = customerId;
        public OrderType OrderType { get; set; } = orderType;
        public OrderStatus OrderStatus { get; set; } = orderStatus;
        public DateTime? DeliveryDate { get; set; }
        public Guid AddressId { get; set; }
        public string CustomerName { get; set; }
        public string OrderDate { get; set; }
        public long InvoiceNumber { get; set; }
        public string CustomerPhone { get; set; }
        public string? Notes { get; set; } = notes;
        public int DeliveryFees { get; set; }
        public decimal TotalOrderPrice { get; set; }
        public string AddressDetails { get; set; }
        public string? Reason { get; set; }
        public ICollection<OrderItemDto> Items { get; set; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Order, OrderDto>()
                    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                    .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.CreatedAt.ToString("dd-MM-yyyy HH:mm")))
                    .ForMember(dest => dest.DeliveryDate, opt => opt.MapFrom(src => src.DeliveryDate))
                    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : ""))
                    .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.PrimaryPhone.ToString() : ""));
            }
        }
    }
}
