using AutoMapper;
using Domain.Entities;

namespace Application.Common.DTOs.InfoDtos
{
    public class AddressDto(
        Guid id,
        string appartmentNumber,
        ushort floorNumber,
        string buildingName,
        string street,
        Guid districtId,
        Guid customerId
        )
    {
        public Guid Id { get; set; } = id;
        public Guid DistrictId { get; set; } = districtId;
        public Guid CustomerId { get; set; } = customerId;
        public string ApartmentNumber { get; set; } = appartmentNumber;
        public ushort FloorNumber { get; set; } = floorNumber;
        public string BuildingName { get; set; } = buildingName;
        public string Street { get; set; } = street;

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Address, AddressDto>();
            }
        }
    }
}
