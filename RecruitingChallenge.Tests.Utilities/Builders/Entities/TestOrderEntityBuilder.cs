using RecruitingChallenge.DAL.Entities;
using RecruitingChallenge.Domain.Enums;

namespace RecruitingChallenge.Tests.Utilities.Builders.Entities
{
    public class TestOrderEntityBuilder
    {
        private readonly OrderEntity orderEntity = new();

        public TestOrderEntityBuilder WithId(int id)
        {
            orderEntity.Id = id;
            return this;
        }

        public TestOrderEntityBuilder WithEntryDate(DateTime entryDate)
        {
            orderEntity.EntryDate = entryDate;
            return this;
        }

        public TestOrderEntityBuilder WithStatus(EOrderStatus status)
        {
            orderEntity.Status = status;
            return this;
        }

        public TestOrderEntityBuilder WithClient(ClientEntity client)
        {
            orderEntity.Client = client;
            orderEntity.Client.Id = client.Id;
            return this;
        }

        public TestOrderEntityBuilder WithOrderItems(OrderItemEntity orderItem)
        {
            orderEntity.OrderItems.Add(orderItem); 
            return this;
        }

        public TestOrderEntityBuilder WithTotalAmount(decimal totalAmount)
        {
            orderEntity.TotalAmount = totalAmount;
            return this;
        }

        public OrderEntity Build() => orderEntity;
    }
}