using Microsoft.EntityFrameworkCore;
using OrderNowChallenge.Common.Models;
using OrderNowChallenge.DAL.Filters;
using OrderNowChallenge.DAL.Repositories.Order;
using OrderNowChallenge.DAL.Repositories.OrderItem;
using OrderNowChallenge.Domain.Enums;
using OrderNowChallenge.Domain.Exceptions;
using OrderNowChallenge.Domain.Models;
using OrderNowChallenge.Service.Models;

namespace OrderNowChallenge.Service.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _unitOfWork = unitOfWork;
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

            var order = await _orderRepository.GetOrderById(id);

            if (order == null)
                throw new OrderNotFoundException();

            await _orderRepository.UpdateOrderStatus(id, status);

            await _unitOfWork.CommitChanges();
        }

		public async Task UpdateQuantityInOrderItem(int orderId, Guid itemId, int quantity)
		{
            ValidateOrderIdEntry(orderId);
            ValidateItemIdUserEntry(itemId);
            ValidateQuantityUserEntry(quantity);
            
            var order = await _orderRepository.GetOrderById(orderId);

            ValidateUpdateQuantityInOrder(order);
			
			var item = order.OrderItems?.FirstOrDefault(i => i.Id == itemId);

			if (item == null)
				throw new OrderItemNotFoundException("Order item not found in the specified order");

            try
            {
                await _unitOfWork.BeginTransaction();

                await _orderItemRepository.UpdateItemQuantity(item.Id, quantity);

                var totalAmount = order.OrderItems
                    .Sum(oi => quantity * oi.UnitPrice);

                await _orderRepository.UpdateTotalAmount(order.Id, totalAmount);

                await _unitOfWork.CommitChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _unitOfWork.RollbackTransaction();
                
                throw new DataBaseContextException("The order was modified by another user", ex);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        private void ValidateUpdateQuantityInOrder(Order order)
        {
            if (order == null)
                throw new OrderNotFoundException("Order not found");

            if (order.Status != EOrderStatus.Pending)
                throw new OrderCannotBeModifiedException("Only orders in Pending status can be edited");
        }

        private void ValidateQuantityUserEntry(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        }

        private void ValidateItemIdUserEntry(Guid itemId)
        {
            if (itemId == Guid.Empty)
                throw new ArgumentException("ItemId should have a valid value", nameof(itemId));
        }

        private void ValidateOrderIdEntry(int id)
        {
            if (id == 0)
                throw new ArgumentException("Id should have a valid value", nameof(id));
        }
    }
}
