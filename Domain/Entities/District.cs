using Domain.Entities.Base;

namespace Domain.Entities
{
    public class District : BaseEntity
    {
        public string Name { get; private set; }
        public long DeliveryFees { get; private set; }

        // EF Core requires a private parameterless constructor for materialization
        private District() { }

        public District(
            string name,
            long deliveryFees
            )
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DeliveryFees = deliveryFees;
        }

        public void UpdateName(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
