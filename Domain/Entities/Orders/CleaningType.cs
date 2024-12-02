using Domain.Entities.Base;

namespace Domain.Entities.Orders
{
    public class CleaningType : BaseEntity
    {
        public string Name { get; private set; }

        // EF Core requires a private parameterless constructor for materialization
        private CleaningType() { }

        public CleaningType(
            string name
            )
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required.", nameof(name));
            }

            Name = name.Trim();
        }

        // Methods to update state
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required.", nameof(name));
            }

            Name = name.Trim();
        }
    }
}
