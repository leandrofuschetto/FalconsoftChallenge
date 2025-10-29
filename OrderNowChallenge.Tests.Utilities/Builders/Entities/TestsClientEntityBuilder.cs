using OrderNowChallenge.DAL.Entities;

namespace OrderNowChallenge.Tests.Utilities.Builders.Entities
{
    public class TestsClientEntityBuilder
    {
        private readonly ClientEntity clientEntity = new();

        public TestsClientEntityBuilder WithId(Guid guid)
        { 
            clientEntity.Id = guid;
            return this;
        }

        public TestsClientEntityBuilder WithEntryDate(DateTime entrydate)
        {
            clientEntity.EntryDate = entrydate;
            return this;
        }

        public TestsClientEntityBuilder WithEmail(string email)
        {
            clientEntity.Email = email;
            return this;
        }

        public TestsClientEntityBuilder WithFirstName(string firstname)
        {
            clientEntity.Name = firstname;
            return this;
        }

        public TestsClientEntityBuilder WithLastname(string lastname)
        {
            clientEntity.LastName = lastname;
            return this;
        }

        public TestsClientEntityBuilder WithOrders(List<OrderEntity> orders)
        {
            clientEntity.Orders.ToList().AddRange(orders);
            return this;
        }

        public ClientEntity Build() => clientEntity;
    }
}
