using RecruitingChallenge.Common.Models;
using RecruitingChallenge.Domain.Enums;
using RecruitingChallenge.Service.Models;

namespace RecruitingChallenge.Service.Orders
{
    public interface IOrderService
    {
        Task<PagedResult<Domain.Models.Order>> GetAllOrders(GetOrdersPagedModel model);
        Task<Domain.Models.Order> GetOrderById(int id);
        Task UpdateOrderStatus(int id, OrderStatus status);
    }
}
