using OrderNowChallenge.Common.Models;
using OrderNowChallenge.Domain.Enums;
using OrderNowChallenge.Service.Models;

namespace OrderNowChallenge.Service.Orders
{
    public interface IOrderService
    {
        Task<PagedResult<Domain.Models.Order>> GetAllOrders(GetOrdersPagedModel model);
        Task<Domain.Models.Order> GetOrderById(int id);
        Task UpdateOrderStatus(int id, EOrderStatus status);
        Task UpdateQuantityInOrderItem(int orderId, Guid itemId, int quantity);
    }
}
