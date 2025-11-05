using OrderNowChallenge.Common.Models;
using OrderNowChallenge.DAL.Filters;
using OrderNowChallenge.Domain.Enums;

namespace OrderNowChallenge.DAL.Repositories.Order
{
    public interface IOrderRepository 
    {
        Task<PagedResult<Domain.Models.Order>> GetPagedOrders(OrderFilters filters);
        Task<Domain.Models.Order> GetOrderById(int orderId);
        Task UpdateOrderStatus(int orderId, EOrderStatus status);
        Task UpdateTotalAmount(int orderId, decimal totalAmount);
    }
}
