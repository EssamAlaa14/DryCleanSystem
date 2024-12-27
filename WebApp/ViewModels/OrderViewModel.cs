using Application.Common.DTOs.InfoDtos;
using AutoMapper;
using Domain.Entities.Orders;
using Domain.Enums;

namespace WebApp.ViewModels
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public long InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string OrderDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public OrderType OrderType { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string? DeliveryDate { get; set; }
        public int DeliveryFees { get; set; }
        public decimal TotalOrderPrice { get; set; }
        public string? Notes { get; set; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Order, OrderViewModel>()
                    .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.CreatedAt.ToString("dd-MM-yyyy HH:mm")))
                    .ForMember(dest => dest.DeliveryDate, opt => opt.MapFrom(src => src.DeliveryDate.HasValue ? src.DeliveryDate.Value.ToString("dd-MM-yyyy") : ""))
                    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : ""))
                    .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.PrimaryPhone.ToString() : ""));
            }
        }
    }

    public class OrderItemViewModel
    {

    }
}
