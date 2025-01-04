using Domain.Entities.Base;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; private set; }
        public Phone PrimaryPhone { get; private set; }
        public Phone? SecondaryPhone { get; private set; }
        public ICollection<Address> Addresses { get; private set; } = [];

        // EF Core requires a private parameterless constructor for materialization
        private Customer() { }

        public Customer(
            string name,
            Phone primaryPhone,
            Phone? secondaryPhone = null
            )
        {
            Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
            PrimaryPhone = primaryPhone ?? throw new ArgumentNullException(nameof(primaryPhone));
            SecondaryPhone = secondaryPhone;
        }

        // Update Methods (Behavior)
        public void UpdateName(string name)
        {
            ArgumentNullException.ThrowIfNull(name);
            Name = name.Trim();
            UpdateTimestamp();
        }

        public void UpdatePrimaryPhone(Phone primaryPhone)
        {
            ArgumentNullException.ThrowIfNull(primaryPhone);
            PrimaryPhone = primaryPhone;
            UpdateTimestamp();
        }

        public void UpdateSecondaryPhone(Phone? seconaryPhone)
        {
            SecondaryPhone = seconaryPhone;
            UpdateTimestamp();
        }

        public void AddAddress(Address address)
        {
            ArgumentNullException.ThrowIfNull(address);
            Addresses.Add(address);
            UpdateTimestamp();
        }

        public void RemoveAddress(Address address)
        {
            ArgumentNullException.ThrowIfNull(address);
            Addresses.Remove(address);
            UpdateTimestamp();
        }
    }
}
