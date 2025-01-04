using Domain.Entities.Base;

namespace Domain.Entities.Orders
{
    public class CleaningType : BaseEntity
    {
        public string Name { get; private set; }
        public int DisplayOrder { get; private set; }

        // EF Core requires a private parameterless constructor for materialization
        private CleaningType() { }

        public CleaningType(
            string name,
            int displayOrder
            )
        {
            Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
            DisplayOrder = displayOrder;
        }

        // Methods to update state
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
