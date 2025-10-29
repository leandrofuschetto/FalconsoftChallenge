using OrderNowChallenge.DAL.Entities;

namespace OrderNowChallenge.Tests.Utilities.Builders.Entities
{
    public class TestOrderItemEntityBuilder
    {
        private readonly OrderItemEntity orderItemEntity = new();

        public TestOrderItemEntityBuilder WithId(Guid id)
        { 
            orderItemEntity.Id = id;
            return this;
        }

        public TestOrderItemEntityBuilder WithQuantity(int quantity)
        { 
            orderItemEntity.Quantity = quantity;
            return this;
        }

        public TestOrderItemEntityBuilder WithProduct(ProductEntity product)
        {
            orderItemEntity.Product = product;
            orderItemEntity.Product.Id = product.Id;
            return this;
        }

        public TestOrderItemEntityBuilder WithOrder(OrderEntity order)
        {
            orderItemEntity.Order = order;
            orderItemEntity.OrderId = order.Id;
            return this;
        }

        public OrderItemEntity Build() => orderItemEntity;
    }
}
