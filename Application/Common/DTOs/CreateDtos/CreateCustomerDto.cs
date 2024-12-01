namespace Application.Common.DTOs.CreateDtos
{
    public class CreateCustomerDto
    {
        public string Name { get; set; }
        public string PrimaryPhone { get; set; }
        public string? SecondaryPhone { get; set; }
        public string ApartmentNumber { get; set; }
        public string FloorNumber { get; set; }
        public string BuildingName { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string City { get; set; }
    }
}
