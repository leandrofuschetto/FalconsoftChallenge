using OrderNowChallenge.Domain.Enums;

namespace OrderNowChallenge.DAL.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public DateTime EntryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public EOrderStatus Status { get; set; }
        public Guid ClientId { get; set; }

        public ClientEntity Client { get; set; }
        public ICollection<OrderItemEntity> OrderItems { get; set; }

        public OrderEntity()
        {
            OrderItems = new List<OrderItemEntity>();
        }
    }
}
