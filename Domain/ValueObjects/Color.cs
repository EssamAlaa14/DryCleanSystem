using Domain.ValueObjects.Base;

namespace Domain.ValueObjects
{
    public class Color : ValueObject
    {
        public string ColorName { get; }

        public Color(
            string colorName
            )
        {
            if (string.IsNullOrWhiteSpace(colorName)) throw new ArgumentNullException(nameof(colorName));
            ColorName = colorName.Trim();
        }

        // Predefined common colors
        public static Color Red => new("Red");
        public static Color Blue => new("Blue");
        public static Color Green => new("Green");
        public static Color Black => new("Black");
        public static Color White => new("White");

        // Allow custom colors
        public static Color Custom(string colorName) => new(colorName);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ColorName;
        }

        public override string ToString() => ColorName;
    }
}
