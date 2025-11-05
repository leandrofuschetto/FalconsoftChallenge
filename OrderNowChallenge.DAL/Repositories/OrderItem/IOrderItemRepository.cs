namespace OrderNowChallenge.DAL.Repositories.OrderItem
{
    public interface IOrderItemRepository
    {
        Task UpdateItemQuantity(Guid id, int quantity);
    }
}
