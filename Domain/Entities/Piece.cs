using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities
{
    public class Piece : BaseEntity
    {
        public string Name { get; private set; }
        public ClothingType ClothingType { get; private set; }

        // EF Core requires a private parameterless constructor for materialization
        private Piece() { }

        public Piece(
            string name,
            ClothingType clothingType
            )
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ClothingType = clothingType;
        }

        public void Update(string name, ClothingType clothingType)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ClothingType = clothingType;
        }
    }
}
