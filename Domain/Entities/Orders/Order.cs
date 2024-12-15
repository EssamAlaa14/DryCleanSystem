using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities.Orders
{
    public class Order : BaseEntity
    {
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; }
        public OrderType OrderType { get; private set; }
        public List<OrderItem> Items { get; private set; } = [];
        public DateTime? DeliveryDate { get; private set; }
        public long InvoiceNumber { get; private set; }
        public short? DeliveryFees { get; private set; }
        public string? Notes { get; private set; }
        public decimal TotalOrderPrice => Items.Sum(item => item.TotalPrice) + DeliveryFees ?? 0;

        // EF Core requires a private parameterless constructor for materialization
        private Order() { }

        // Constructor
        public Order(
            Guid customerId,
            OrderType orderType,
            short deliveryFees = 0, 
            DateTime? deliveryDate = null,
            string? notes = null
            )
        {
            if (customerId == Guid.Empty) throw new ArgumentException("CustomerId cannot be empty.");
            if (deliveryDate.HasValue && deliveryDate.Value < DateTime.UtcNow)
            {
                throw new ArgumentException("DeliveryDate cannot be earlier than the current time.");
            }

            CustomerId = customerId;
            OrderType = orderType;
            DeliveryDate = deliveryDate;
            DeliveryFees = deliveryFees;
            Notes = notes?.Trim();
        }

        public void AddItem(OrderItem item)
        {
            ArgumentNullException.ThrowIfNull(item);
            Items.Add(item);
        }

        public void RemoveItem(Guid itemId)
        {
            var item = Items.SingleOrDefault(i => i.Id == itemId) ?? throw new InvalidOperationException("Item not found.");
            Items.Remove(item);
        }

        public void UpdateDeliveryDate(DateTime? deliveryDate)
        {
            if (deliveryDate.HasValue && deliveryDate.Value < CreatedAt)
            {
                throw new ArgumentException("DeliveryDate cannot be earlier than OrderDate.");
            }

            DeliveryDate = deliveryDate;
        }

        public void UpdateNotes(string? notes)
        {
            Notes = notes?.Trim();
        }
    }
}
