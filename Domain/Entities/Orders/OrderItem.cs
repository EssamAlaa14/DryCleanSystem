using Domain.Entities.Base;
using Domain.ValueObjects;

namespace Domain.Entities.Orders
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; }
        public Guid PieceId { get; private set; }
        public Piece Piece { get; private set; }
        public string? Description { get; private set; }
        public int Quantity { get; private set; }
        public decimal SingleQuantityPrice { get; private set; }
        public decimal TotalPrice { get; private set; }
        public Color Color { get; private set; }
        public string? Comment { get; private set; }
        public ICollection<CleaningType> CleaningTypes { get; private set; } = [];

        // EF Core requires a private parameterless constructor for materialization
        private OrderItem() { }

        public OrderItem(
            Guid orderId,
            Guid pieceId,
            int quantity,
            decimal singleQuantityPrice,
            Color color,
            string? description,
            string? comment
            )
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than 0.");
            if (singleQuantityPrice <= 0) throw new ArgumentException("Price must be greater than 0.");

            OrderId = orderId;
            PieceId = pieceId;
            Quantity = quantity;
            SingleQuantityPrice = singleQuantityPrice;
            TotalPrice = quantity * singleQuantityPrice;
            Color = color;
            Description = description?.Trim();
            Comment = comment?.Trim();
        }

        public void AddCleaningType(CleaningType cleaningType)
        {
            ArgumentNullException.ThrowIfNull(cleaningType);
            CleaningTypes.Add(cleaningType);
            UpdateTimestamp();
        }

        public void RemoveCleaningType(CleaningType cleaningType)
        {
            ArgumentNullException.ThrowIfNull(cleaningType);
            CleaningTypes.Remove(cleaningType);
            UpdateTimestamp();
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than 0.");
            Quantity = quantity;
            TotalPrice = Quantity * SingleQuantityPrice;
            UpdateTimestamp();
        }

        public void UpdateSingleQuantityPrice(decimal singleQuantityPrice)
        {
            if (singleQuantityPrice <= 0) throw new ArgumentException("Price must be greater than 0.");
            SingleQuantityPrice = singleQuantityPrice;
            TotalPrice = Quantity * SingleQuantityPrice;
            UpdateTimestamp();
        }

        public void UpdateColor(Color color)
        {
            Color = color;
            UpdateTimestamp();
        }

        public void UpdateComment(string? comment)
        {
            Comment = comment?.Trim();
            UpdateTimestamp();
        }

        public void UpdateDescription(string? description)
        {
            Description = description?.Trim();
            UpdateTimestamp();
        }
    }
}
