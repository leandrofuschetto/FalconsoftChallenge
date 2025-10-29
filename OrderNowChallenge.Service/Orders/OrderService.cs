using OrderNowChallenge.Common.Models;
using OrderNowChallenge.DAL.Filters;
using OrderNowChallenge.DAL.Repositories.Order;
using OrderNowChallenge.Domain.Enums;
using OrderNowChallenge.Domain.Models;
using OrderNowChallenge.Service.Models;
using OrderNowChallenge.Domain.Exceptions;

namespace OrderNowChallenge.Service.Orders
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
                SortBy = model.SortByProperty,
                Orientation = model.Orientation,
                LastCursorId = model.LastCursorId,
                LastCursorValue = model.LastCursorValue,
                AmountFilter = model.AmountFilter,
                ClientEmailFilter = model.ClientEmailFilter,
                EntryDateFilter = model.EntryDateFilter,
                OrderStatusFilter = model.OrderStatusFilter,
            };

            var orders = _orderRepository.GetPagedOrders(orderFilters);

            return orders;
        }

        public async Task<Order> GetOrderById(int id)
        {
            ValidateOrderIdEntry(id);
            
            var order = await _orderRepository.GetOrderById(id);

            return order;
        }

        public async Task UpdateOrderStatus(int id, EOrderStatus status)
        {
            if (id == 0)
                throw new ArgumentException("Id should have a valid value", nameof(id));

            await _orderRepository.UpdateOrderStatus(id, status);
        }

		public async Task UpdateQuantityInOrderItem(int orderId, Guid itemId, int quantity)
		{
            ValidateOrderIdEntry(orderId);

            if (itemId == Guid.Empty)
				throw new ArgumentException("ItemId should have a valid value", nameof(itemId));

			if (quantity <= 0)
				throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

			var order = await _orderRepository.GetOrderById(orderId);

			if (order == null)
				throw new OrderNotFoundException("Order not found");

			if (order.Status != EOrderStatus.Pending)
				throw new OrderCannotBeModifiedException("Only orders in Pending status can be edited");

			var item = order.OrderItems?.FirstOrDefault(i => i.Id == itemId);

			if (item == null)
				throw new OrderItemNotFoundException("Order item not found in the specified order");

			await _orderRepository.UpdateOrderItemQuantity(orderId, itemId, quantity);
		}

        private void ValidateOrderIdEntry(int id)
        {
            if (id == 0)
                throw new ArgumentException("Id should have a valid value", nameof(id));
        }
    }
}
