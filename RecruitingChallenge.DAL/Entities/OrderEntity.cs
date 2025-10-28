using RecruitingChallenge.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingChallenge.DAL.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public DateTime EntryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public Guid ClientId { get; set; }

        public ClientEntity Client { get; set; }
        public ICollection<OrderItemEntity> OrderItems { get; set; }

        public OrderEntity()
        {
            OrderItems = new List<OrderItemEntity>();
        }
    }
}
