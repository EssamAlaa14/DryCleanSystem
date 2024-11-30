using Domain.Entities.Base;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; private set; }
        public Email? Email { get; private set; }
        public Phone PrimaryPhone { get; private set; }
        public Phone? SecondaryPhone { get; private set; }
        public Address Address { get; private set; }

        // EF Core requires a private parameterless constructor for materialization
        private Customer() { }

        public Customer(string name, Phone primaryPhone, Address address, Email? email = null, Phone? secondaryPhone = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            PrimaryPhone = primaryPhone ?? throw new ArgumentNullException(nameof(primaryPhone));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Email = email; // Optional
            SecondaryPhone = secondaryPhone; // Optional
        }

        // Update Methods (Behavior)
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(newName));

            Name = newName;
            UpdateTimestamp();
        }

        public void UpdatePrimaryPhone(Phone newPrimaryPhone)
        {
            ArgumentNullException.ThrowIfNull(newPrimaryPhone);

            PrimaryPhone = newPrimaryPhone;
            UpdateTimestamp();
        }

        public void UpdateAddress(Address newAddress)
        {
            ArgumentNullException.ThrowIfNull(newAddress);

            Address = newAddress;
            UpdateTimestamp();
        }

        public void UpdateEmail(Email? newEmail)
        {
            Email = newEmail; // Allows null to unset email
            UpdateTimestamp();
        }

        public void UpdateSecondaryPhone(Phone? newSecondaryPhone)
        {
            SecondaryPhone = newSecondaryPhone; // Allows null to unset secondary phone
            UpdateTimestamp();
        }

        protected new void UpdateTimestamp() => UpdatedAt = DateTime.UtcNow;
    }
}
