using Domain.Enums;

namespace Application.Common.DTOs.CreateDtos
{
    public class CreateOrderDto
    {
        public OrderType OrderType { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? Notes { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = [];
    }
}
