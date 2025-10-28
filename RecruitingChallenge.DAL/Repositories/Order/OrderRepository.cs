using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecruitingChallenge.DAL.Entities;
using RecruitingChallenge.DAL.Repositories.Filters;
using RecruitingChallenge.DAL.Repositories.User;
using RecruitingChallenge.Domain.Enums;
using RecruitingChallenge.Domain.Exceptions;
using System.Linq.Expressions;

namespace RecruitingChallenge.DAL.Repositories.Order
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger<IUserRepository> _logger;
        private readonly OrderNowDbContext _dbContext;
        private readonly string CLASS_NAME = typeof(OrderRepository).Name;
        private readonly int PageSize = 10;

        public OrderRepository(
            OrderNowDbContext context,
            IMapper mapper,
            ILogger<IUserRepository> logger)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Common.Models.PagedResult<Domain.Models.Order>> GetPagedOrders(OrderFilters filters)
        {
            try
            {
                var query = _dbContext.Orders
                    .Include(o => o.Client)
                    .Include(o => o.OrderItems)
                    .ThenInclude(o => o.Product)
                    .AsQueryable();

                Expression<Func<OrderEntity, object>> keySelector = GetKeySelector(filters);
                
                if (filters.Orientation.Equals(SortOrientation.Asc))
                    query = query.OrderBy(keySelector).ThenBy(o => o.Id);
                else
                    query = query.OrderByDescending(keySelector).ThenByDescending(o => o.Id);

                var items = await query
                    .Take(PageSize + 1)
                    .ToListAsync();

                bool hasNextPage = items.Count > PageSize;

                if (hasNextPage)
                    items.RemoveAt(items.Count - 1);

                var nextCursor = GetCursorValues(items, hasNextPage);

                var domainList = _mapper.Map<IEnumerable<Domain.Models.Order>>(items);

                return new Common.Models.PagedResult<Domain.Models.Order>(
                    domainList.ToList().AsReadOnly(),
                    null,
                    nextCursor,
                    hasNextPage);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(
                    ex,
                    $"An error ocurrs when getting all orders. At {CLASS_NAME}, GetOrders");

                throw new DataBaseContextException(
                    ex.Message,
                    ex);
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(
                    ex,
                    $"Timeout occurs when getting all orders. At {CLASS_NAME}, GetOrders");

                throw new DataBaseContextException(
                    ex.Message,
                    ex);
            }
        }

        private Expression<Func<OrderEntity, object>> GetKeySelector(OrderFilters filters)
        {
            switch (filters.SortBy)
            {
                case OrderSort.EntryDate:
                    return order => order.EntryDate;
                case OrderSort.Status:
                    return order => order.Status;
                case OrderSort.Amount:
                    return order => order.TotalAmount;
                case OrderSort.ClientId:
                    return order => order.ClientId;
                default:
                    return order => order.Id;
            }
        }

        public async Task<Domain.Models.Order> GetOrderById(int orderId)
        {
            try
            {
                var orders = await _dbContext.Orders
                    .Include(o => o.Client)
                    .Include(o => o.OrderItems)
                    .ThenInclude(o => o.Product)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                return _mapper.Map<Domain.Models.Order>(orders);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(
                    ex,
                    $"An error ocurrs when getting all orders. At {CLASS_NAME}, GetOrders");

                throw new DataBaseContextException(
                    ex.Message,
                    ex);
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(
                    ex,
                    $"Timeout occurs when getting all orders. At {CLASS_NAME}, GetOrders");

                throw new DataBaseContextException(
                    ex.Message,
                    ex);
            }
        }

        public async Task UpdateOrderStatus(int orderId, OrderStatus status)
        {
            await _dbContext.Database.BeginTransactionAsync();

            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);

            order.Status = status;

            _dbContext.Orders.Update(order);

            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.CommitTransactionAsync();
        }

        private static string GetCursorValues(
            List<OrderEntity> items,
            bool hasNextPage)
        {
            if (!hasNextPage)
                return null;

            var lastItem = items.Last();
            string nextCursor = lastItem.Id.ToString();

            return nextCursor;
        }
    }
}
