using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingChallenge.DAL.Entities
{
    public class OrderItemEntity
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public Guid ProductId { get; set; }
        
        public OrderEntity Order { get; set; }
        public ProductEntity Product { get; set; }
    }
}
