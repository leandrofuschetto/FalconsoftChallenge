using Microsoft.EntityFrameworkCore;

namespace OrderNowChallenge.DAL.Repositories.OrderItem
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private OrderNowDbContext _dbContext;

        public OrderItemRepository(OrderNowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateItemQuantity(Guid id, int quantity)
        {
            var orderItem = await _dbContext.OrderItems
                .AsTracking()
                .FirstOrDefaultAsync(oi => oi.Id == id);

            orderItem.Quantity = quantity;
        }
    }
}
