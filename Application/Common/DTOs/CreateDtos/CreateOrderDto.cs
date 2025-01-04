using Domain.Enums;

namespace Application.Common.DTOs.CreateDtos
{
    public class CreateOrderDto
    {
        public OrderType OrderType { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.New;
        public DateTime? DeliveryDate { get; set; }
        public short? DeliveryFees { get; set; }
        public string? Notes { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = [];
    }
}
