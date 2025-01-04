using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities.Orders
{
    public class Order : BaseEntity
    {
        public Guid CustomerId { get; private set; }
        public Guid AddressId { get; private set; }
        public Customer Customer { get; private set; }
        public OrderType OrderType { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
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
            Guid addressId,
            OrderType orderType,
            OrderStatus orderStatus = OrderStatus.New,
            short deliveryFees = 0,
            string? notes = null
            )
        {
            if (customerId == Guid.Empty) throw new ArgumentException("CustomerId cannot be empty.");
            if (addressId == Guid.Empty) throw new ArgumentException("AddressId cannot be empty.");

            CustomerId = customerId;
            AddressId = addressId;
            OrderType = orderType;
            OrderStatus = orderStatus;
            DeliveryFees = deliveryFees;
            Notes = notes?.Trim();
        }

        public void AddItem(OrderItem item)
        {
            ArgumentNullException.ThrowIfNull(item);
            Items.Add(item);
            UpdateTimestamp();
        }

        public void RemoveItem(Guid itemId)
        {
            var item = Items.SingleOrDefault(i => i.Id == itemId) ?? throw new InvalidOperationException("Item not found.");
            Items.Remove(item);
            UpdateTimestamp();
        }

        public void UpdateNotes(string? notes)
        {
            Notes = notes?.Trim();
            UpdateTimestamp();
        }

        public void UpdateOrderStatus(OrderStatus orderStatus)
        {
            OrderStatus = orderStatus;
            if (orderStatus == OrderStatus.Delivered)
            {
                DeliveryDate = DateTime.Now;
            }
            UpdateTimestamp();
        }
    }
}
