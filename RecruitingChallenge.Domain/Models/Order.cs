using RecruitingChallenge.Domain.Enums;

namespace RecruitingChallenge.Domain.Models
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
        public OrderStatus Status { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; } 

        public decimal CalculateTotalAmount() => OrderItems.Sum(item => item.Subtotal);
        public bool IsOrderComplete() => Status == OrderStatus.Completed || Status == OrderStatus.Delivered;
        public bool IsOrderPending() => Status == OrderStatus.Pending || Status == OrderStatus.Processing;
        public bool isOrderCancelled() => Status == OrderStatus.Cancelled;
    }
}
