using Domain.Entities.Base;

namespace Domain.Entities.Orders
{
    public class CleaningType : BaseEntity
    {
        public string Name { get; private set; }
        public string NameArabic { get; private set; }

        // EF Core requires a private parameterless constructor for materialization
        private CleaningType() { }

        public CleaningType(string name, string nameArabic)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(nameArabic))
            {
                throw new ArgumentException("Arabic name is required.", nameof(nameArabic));
            }

            Name = name.Trim();
            NameArabic = nameArabic.Trim();
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

        public void UpdateNameArabic(string nameArabic)
        {
            if (string.IsNullOrWhiteSpace(nameArabic))
            {
                throw new ArgumentException("Arabic name is required.", nameof(nameArabic));
            }

            NameArabic = nameArabic.Trim();
        }
    }
}
