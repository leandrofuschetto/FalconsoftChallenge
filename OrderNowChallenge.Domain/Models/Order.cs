using OrderNowChallenge.Domain.Enums;

namespace OrderNowChallenge.Domain.Models
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public int Id { get; set; }
        public DateTime EntryDate { get; set; }
        public Client Client { get; set; }
        public decimal TotalAmount { get; set; }
        public EOrderStatus Status { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; } 

        public decimal CalculateTotalAmount() => OrderItems.Sum(item => item.Subtotal);
        public bool IsOrderComplete() => Status == EOrderStatus.Completed || Status == EOrderStatus.Delivered;
        public bool IsOrderPending() => Status == EOrderStatus.Pending || Status == EOrderStatus.Processing;
        public bool isOrderCancelled() => Status == EOrderStatus.Cancelled;
    }
}
