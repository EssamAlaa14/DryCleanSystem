using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities
{
    public class Piece : BaseEntity
    {
        public string Name { get; private set; }
        public int DisplayOrder { get; private set; }
        public ClothingType ClothingType { get; private set; }

        // EF Core requires a private parameterless constructor for materialization
        private Piece() { }

        public Piece(
            string name,
            int displayOrder,
            ClothingType clothingType
            )
        {
            Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
            DisplayOrder = displayOrder;
            ClothingType = clothingType;
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

        public void UpdateClothingType(ClothingType clothingType)
        {
            ClothingType = clothingType;
            UpdateTimestamp();
        }
    }
}
