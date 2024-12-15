namespace Application.Common.DTOs.CreateDtos
{
    public class CreateCustomerDto
    {
        public string Name { get; set; }
        public string PrimaryPhone { get; set; }
        public string? SecondaryPhone { get; set; }
        public Guid AddressId { get; set; }
    }
}
