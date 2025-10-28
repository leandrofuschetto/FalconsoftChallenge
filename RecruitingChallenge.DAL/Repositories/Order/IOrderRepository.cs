using RecruitingChallenge.Common.Models;
using RecruitingChallenge.DAL.Repositories.Filters;

namespace RecruitingChallenge.DAL.Repositories.Order
{
    public interface IOrderRepository
    {
        Task<PagedResult<Domain.Models.Order>> GetPagedOrders(OrderFilters filters);
        Task<Domain.Models.Order> GetOrderById(int orderId);
        Task UpdateOrderStatus(int orderId, Domain.Enums.OrderStatus status);
    }
}
