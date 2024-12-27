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
        string? deliveryDate,
        string? notes
        )
    {
        public Guid Id { get; set; } = id;
        public Guid CustomerId { get; set; } = customerId;
        public string CustomerName { get; set; }
        public string OrderDate { get; set; }
        public long InvoiceNumber { get; set; }
        public string CustomerPhone { get; set; }
        public OrderType OrderType { get; set; } = orderType;
        public OrderStatus OrderStatus { get; set; } = orderStatus;
        public string? DeliveryDate { get; set; } = deliveryDate;
        public string? Notes { get; set; } = notes;
        public int DeliveryFees { get; set; }
        public decimal TotalOrderPrice { get; set; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Order, OrderDto>()
                    .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.CreatedAt.ToString("dd-MM-yyyy HH:mm")))
                    .ForMember(dest => dest.DeliveryDate, opt => opt.MapFrom(src => src.DeliveryDate.HasValue ? src.DeliveryDate.Value.ToString("dd-MM-yyyy") : ""))
                    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : ""))
                    .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.PrimaryPhone.ToString() : ""));
            }
        }
    }
}
