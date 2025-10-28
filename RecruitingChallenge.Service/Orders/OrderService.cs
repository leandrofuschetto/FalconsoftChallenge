using RecruitingChallenge.Common.Models;
using RecruitingChallenge.DAL.Repositories.Filters;
using RecruitingChallenge.DAL.Repositories.Order;
using RecruitingChallenge.Domain.Enums;
using RecruitingChallenge.Domain.Models;
using RecruitingChallenge.Service.Models;

namespace RecruitingChallenge.Service.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task<PagedResult<Order>> GetAllOrders(GetOrdersPagedModel model)
        {
            var orderFilters = new OrderFilters()
            {
                Orientation = model.Orientation,
                SortBy = model.SortBy,
                Status = model.Status,
                LastCursorId = model.LastCursorId,
                LastCursorValue = model.LastCursorValue
            };

            var orders = _orderRepository.GetPagedOrders(orderFilters);

            return orders;
        }

        public async Task<Order> GetOrderById(int id)
        {
            if (id == 0)
                throw new ArgumentException("Id should have a valid value", nameof(id));

            var order = await _orderRepository.GetOrderById(id);

            return order;
        }

        public async Task UpdateOrderStatus(int id, OrderStatus status)
        {
            if (id == 0)
                throw new ArgumentException("Id should have a valid value", nameof(id));

            await _orderRepository.UpdateOrderStatus(id, status);
        }
    }
}
