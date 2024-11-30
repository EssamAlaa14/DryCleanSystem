using Domain.ValueObjects.Base;

namespace Domain.ValueObjects
{
    public class Address(
        string appartmentNumber,
        string floorNumber,
        string buildingName,
        string street,
        string district,
        string city
        ) : ValueObject
    {
        public string AppartmentNumber { get; } = appartmentNumber ?? throw new ArgumentNullException(nameof(appartmentNumber));
        public string FloorNumber { get; } = floorNumber ?? throw new ArgumentNullException(nameof(floorNumber));
        public string BuildingName { get; } = buildingName ?? throw new ArgumentNullException(nameof(buildingName));
        public string Street { get; } = street ?? throw new ArgumentNullException(nameof(street));
        public string District { get; } = district ?? throw new ArgumentNullException(nameof(district));
        public string City { get; } = city ?? throw new ArgumentNullException(nameof(city));

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AppartmentNumber;
            yield return FloorNumber;
            yield return BuildingName;
            yield return Street;
            yield return District;
            yield return City;
        }

        public override string ToString()
        {
            return $"{AppartmentNumber}, {FloorNumber}, {BuildingName}, {Street}, {District}, {City}";
        }
    }
}
