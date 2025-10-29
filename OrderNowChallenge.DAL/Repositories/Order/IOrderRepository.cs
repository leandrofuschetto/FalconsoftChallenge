using OrderNowChallenge.Common.Models;
using OrderNowChallenge.DAL.Filters;

namespace OrderNowChallenge.DAL.Repositories.Order
{
    public interface IOrderRepository
    {
        Task<PagedResult<Domain.Models.Order>> GetPagedOrders(OrderFilters filters);
        Task<Domain.Models.Order> GetOrderById(int orderId);
        Task UpdateOrderStatus(int orderId, Domain.Enums.EOrderStatus status);
        Task UpdateOrderItemQuantity(int orderId, Guid itemId, int quantity);
    }
}
