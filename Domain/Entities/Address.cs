using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Address : BaseEntity
    {
        public string AppartmentNumber { get; private set; }
        public ushort FloorNumber { get; private set; }
        public string BuildingName { get; private set; }
        public string Street { get; private set; }
        public Guid DistrictId { get; private set; }
        public District District { get; private set; }
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; }

        // EF Core requires a private parameterless constructor for materialization
        private Address() { }

        public Address(
            string appartmentNumber,
            ushort floorNumber,
            string buildingName,
            string street,
            Guid districtId //,
            //Guid customerId
            )
        {
            AppartmentNumber = appartmentNumber ?? throw new ArgumentNullException(nameof(appartmentNumber));
            FloorNumber = floorNumber;
            BuildingName = buildingName ?? throw new ArgumentNullException(nameof(buildingName));
            Street = street ?? throw new ArgumentNullException(nameof(street));
            DistrictId = districtId;
            //CustomerId = customerId;
        }

        public void Update(
            string appartmentNumber,
            ushort floorNumber,
            string buildingName,
            string street,
            Guid districtId
            )
        {
            AppartmentNumber = appartmentNumber ?? throw new ArgumentNullException(nameof(appartmentNumber));
            FloorNumber = floorNumber;
            BuildingName = buildingName ?? throw new ArgumentNullException(nameof(buildingName));
            Street = street ?? throw new ArgumentNullException(nameof(street));
            DistrictId = districtId;
        }

        public override string ToString()
        {
            return $"{AppartmentNumber}, {FloorNumber}, {BuildingName}, {Street}, {District.Name}";
        }
    }
}
