using Domain.Enums;

namespace Application.Common.DTOs.UpdateDtos
{
    public class UpdateOrderDto
    {
        public Guid OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
