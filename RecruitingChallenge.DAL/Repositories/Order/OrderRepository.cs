using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecruitingChallenge.Common.Enums;
using RecruitingChallenge.Common.Extensions;
using RecruitingChallenge.DAL.Entities;
using RecruitingChallenge.DAL.Filters;
using RecruitingChallenge.DAL.Repositories.User;
using RecruitingChallenge.Domain.Enums;
using RecruitingChallenge.Domain.Exceptions;

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

                query = ApplyFilters(query, filters);

                if (filters.SortBy.HasValue)
                    query = ApplySorting(query, filters);

                if (filters.LastCursorId.HasValue && !string.IsNullOrWhiteSpace(filters.LastCursorValue))
                    query = ApplyKeyset(
                        query, 
                        filters.SortBy ?? ESortOrderByProperty.Id, 
                        filters.Orientation ?? ESortOrientation.Asc, 
                        filters.LastCursorValue, 
                        filters.LastCursorId ?? 0);

                var items = await query
                    .Take(PageSize + 1)
                    .ToListAsync();

                bool hasNextPage = items.Count > PageSize;
                
                if (hasNextPage)
                    items.RemoveAt(items.Count - 1);

                string nextCursor = null;
                string nextCursorValue = null;
                if (hasNextPage && items.Any())
                {
                    var lastItem = items.Last();
                    nextCursor = lastItem.Id.ToString();
                    nextCursorValue = GetCursorValue(lastItem, filters.SortBy.HasValue ? filters.SortBy.Value : ESortOrderByProperty.Id);
                }

                return new Common.Models.PagedResult<Domain.Models.Order>(
                    items: _mapper.Map<IEnumerable<Domain.Models.Order>>(items).ToList().AsReadOnly(),
                    nextCursorValue: nextCursorValue,
                    nextCursor: nextCursor,
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

        private IQueryable<OrderEntity> ApplyFilters(IQueryable<OrderEntity> query, OrderFilters filters)
        {
            if (filters.OrderStatusFilter.HasValue)
                query = query.Where(o => o.Status == filters.OrderStatusFilter.Value);

            if (filters.AmountFilter.HasValue)
                query = query.Where(o => o.TotalAmount == filters.AmountFilter.Value);

            if (!string.IsNullOrWhiteSpace(filters.ClientEmailFilter))
                query = query.Where(o => o.Client.Email == filters.ClientEmailFilter);

            if (filters.EntryDateFilter.HasValue)
                query = query.Where(o => o.EntryDate == Convert.ToDateTime(filters.EntryDateFilter.Value));

            return query;
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

        public async Task UpdateOrderStatus(int orderId, EOrderStatus status)
        {
            await _dbContext.Database.BeginTransactionAsync();

            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);

            order.Status = status;

            _dbContext.Orders.Update(order);

            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.CommitTransactionAsync();
        }

        private IQueryable<OrderEntity> ApplySorting(IQueryable<OrderEntity> query, OrderFilters filters)
        {
            return filters.SortBy switch
            {
                ESortOrderByProperty.Id => filters.Orientation == ESortOrientation.Asc
                    ? query.OrderBy(x => x.Id)
                    : query.OrderByDescending(x => x.Id),

                ESortOrderByProperty.EntryDate => filters.Orientation == ESortOrientation.Asc
                    ? query.OrderBy(x => x.EntryDate).ThenBy(x => x.Id)
                    : query.OrderByDescending(x => x.EntryDate).ThenByDescending(x => x.Id),

                ESortOrderByProperty.TotalAmount => filters.Orientation == ESortOrientation.Asc
                    ? query.OrderBy(x => x.TotalAmount).ThenBy(x => x.Id)
                    : query.OrderByDescending(x => x.TotalAmount).ThenByDescending(x => x.Id),

                ESortOrderByProperty.Status => filters.Orientation == ESortOrientation.Asc
                    ? query.OrderBy(x => x.Status).ThenBy(x => x.Id)
                    : query.OrderByDescending(x => x.Status).ThenByDescending(x => x.Id),

                ESortOrderByProperty.ClientEmail => filters.Orientation == ESortOrientation.Asc
                    ? query.OrderBy(x => x.Client.Email).ThenBy(x => x.Id)
                    : query.OrderByDescending(x => x.Client.Email).ThenByDescending(x => x.Id),

                _ => throw new ArgumentException($"Unsupported sort property: {filters.SortBy}")
            };
        }

        private IQueryable<OrderEntity> ApplyKeyset(
            IQueryable<OrderEntity> query, 
            ESortOrderByProperty sortByProperty, 
            ESortOrientation orientation, 
            string lastValue, 
            int lastId)
        {
            return sortByProperty switch
            {
                ESortOrderByProperty.Id => ApplyKeysetPaginationById(query, lastId, int.Parse(lastValue), orientation),
                ESortOrderByProperty.EntryDate => ApplyKeysetPaginationByEntryDate(query, lastId, Convert.ToDateTime(lastValue), orientation),
                ESortOrderByProperty.TotalAmount => ApplyKeysetPaginationByTotalAmount(query, lastId, decimal.Parse(lastValue), orientation),
                ESortOrderByProperty.Status => ApplyKeysetPaginationByStatus(query, lastId, Enum.Parse<EOrderStatus>(lastValue), orientation),
                ESortOrderByProperty.ClientEmail => ApplyKeysetPaginationByClientEmail(query, lastId, lastValue, orientation),
                _ => throw new ArgumentException($"Unsupported sort property: {sortByProperty}")
            };
        }

        private IQueryable<OrderEntity> ApplyKeysetPaginationById(
            IQueryable<OrderEntity> query, 
            int lastId, 
            int lastValue, 
            ESortOrientation orientation)
        {
            return orientation == ESortOrientation.Asc
                ? query.Where(x => x.Id > lastValue)
                : query.Where(x => x.Id < lastValue);
        }

        private IQueryable<OrderEntity> ApplyKeysetPaginationByEntryDate(
            IQueryable<OrderEntity> query,
            int lastId,
            DateTime lastValue,
            ESortOrientation orientation)
        {
            return orientation == ESortOrientation.Asc
                ? query.Where(x => x.EntryDate > lastValue || (x.EntryDate == lastValue && x.Id > lastId))
                : query.Where(x => x.EntryDate < lastValue || (x.EntryDate == lastValue && x.Id < lastId));
        }

        private IQueryable<OrderEntity> ApplyKeysetPaginationByTotalAmount(
            IQueryable<OrderEntity> query, 
            int lastId, 
            decimal lastValue, 
            ESortOrientation orientation)
        {
            return orientation == ESortOrientation.Asc
                ? query.Where(x => x.TotalAmount > lastValue || (x.TotalAmount == lastValue && x.Id > lastId))
                : query.Where(x => x.TotalAmount < lastValue || (x.TotalAmount  == lastValue && x.Id < lastId));
        }

        private IQueryable<OrderEntity> ApplyKeysetPaginationByStatus(
            IQueryable<OrderEntity> query,
            int lastId,
            EOrderStatus lastValue,
            ESortOrientation orientation)
        {
            return orientation == ESortOrientation.Asc  
                ? query.Where(x => x.Status > lastValue || (x.Status == lastValue && x.Id > lastId))
                : query.Where(x => x.Status < lastValue || (x.Status == lastValue && x.Id < lastId));
        }

        private IQueryable<OrderEntity> ApplyKeysetPaginationByClientEmail(
            IQueryable<OrderEntity> query, 
            int lastId, 
            string lastValue, 
            ESortOrientation orientation)
        {
            return orientation == ESortOrientation.Asc
                ? query.Where(x => x.Client.Email.CompareTo(lastValue) > 0 || (x.Client.Email == lastValue && x.Id > lastId))
                : query.Where(x => x.Client.Email.CompareTo(lastValue) < 0 || (x.Client.Email == lastValue && x.Id < lastId));
        }

        private string GetCursorValue(OrderEntity order, ESortOrderByProperty sortByProperty)
        {
            return sortByProperty switch
            {
                ESortOrderByProperty.Id => order.Id.ToString(),
                ESortOrderByProperty.EntryDate => order.EntryDate.ToString("yyyy-MM-dd").TryFormatAsDate(),
                ESortOrderByProperty.TotalAmount => order.TotalAmount.ToString("F2"),
                ESortOrderByProperty.Status => order.Status.ToString(),
                ESortOrderByProperty.ClientEmail => order.Client.Email,
                _ => order.Id.ToString()
            };
        }
    }
}
