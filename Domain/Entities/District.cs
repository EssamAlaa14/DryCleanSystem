using Domain.Entities.Base;

namespace Domain.Entities
{
    public class District : BaseEntity
    {
        public string Name { get; private set; }
        public long DeliveryFees { get; private set; }
        public int DisplayOrder { get; private set; }

        // EF Core requires a private parameterless constructor for materialization
        private District() { }

        public District(
            string name,
            long deliveryFees,
            int displayOrder
            )
        {
            Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
            DeliveryFees = deliveryFees;
            DisplayOrder = displayOrder;
        }

        public void UpdateName(string name)
        {
            ArgumentNullException.ThrowIfNull(name);
            Name = name.Trim();
            UpdateTimestamp();
        }

        public void UpdateDisplayOrder(int displayOrder)
        {
            DisplayOrder = displayOrder;
            UpdateTimestamp();
        }
    }
}
