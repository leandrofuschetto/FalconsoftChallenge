namespace OrderNowChallenge.DAL.Entities
{
    public class ClientEntity
    {
        public Guid Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ICollection<OrderEntity> Orders { get; set; }

        public ClientEntity()
        {
            Orders = new List<OrderEntity>();
        }
    }
}
