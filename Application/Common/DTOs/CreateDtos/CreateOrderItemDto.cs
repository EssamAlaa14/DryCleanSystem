namespace Application.Common.DTOs.CreateDtos
{
    public class CreateOrderItemDto
    {
        public int Quantity { get; set; }
        public decimal SingleQuantityPrice { get; set; }
        public string Color { get; set; }
        public string? Comment { get; set; }
        public List<Guid> CleaningTypeIds { get; set; } = new(); // List of CleaningType IDs
    }
}
