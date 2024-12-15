namespace Application.Common.DTOs.CreateDtos
{
    public class CreateOrderItemDto
    {
        public Guid PieceId { get; set; }
        public int Quantity { get; set; }
        public decimal SingleQuantityPrice { get; set; }
        public string? Description { get; set; }
        public string? Comment { get; set; }
        public List<Guid> CleaningTypeIds { get; set; } = [];
    }
}
