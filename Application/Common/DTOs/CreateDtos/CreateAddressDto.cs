namespace Application.Common.DTOs.CreateDtos
{
    public class CreateAddressDto
    {
        public Guid DistrictId { get; set; }
        public Guid CustomerId { get; set; }
        public string ApartmentNumber { get; set; }
        public ushort FloorNumber { get; set; }
        public string BuildingName { get; set; }
        public string Street { get; set; }
    }
}
