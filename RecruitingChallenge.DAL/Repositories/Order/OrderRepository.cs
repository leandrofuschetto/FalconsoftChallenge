using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecruitingChallenge.DAL.Entities;
using RecruitingChallenge.DAL.Filters;
using RecruitingChallenge.DAL.Repositories.User;
using RecruitingChallenge.Domain.Enums;
using RecruitingChallenge.Domain.Exceptions;
using System.Globalization;
using System.Linq;
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

                if (!string.IsNullOrEmpty(filters.FilterValue))
                    query = ApplyFilter(query, filters.SortBy, filters.FilterOperator, filters.FilterValue);
                
                query = ApplySorting(query, filters);
                
                if (filters.LastCursorId.HasValue && !string.IsNullOrWhiteSpace(filters.LastCursorValue))
                    query = ApplyKeyset(
                        query, 
                        filters.SortBy, 
                        filters.Orientation, 
                        filters.LastCursorValue, 
                        filters.LastCursorId ?? 0);

                var items = await query
                    .Take(PageSize + 1)
                    .ToListAsync();

                var domainList = _mapper.Map<IEnumerable<Domain.Models.Order>>(items);

                bool hasNextPage = items.Count > PageSize;
                
                if (!hasNextPage)
                { 
                    return new Common.Models.PagedResult<Domain.Models.Order>(
                        domainList.ToList().AsReadOnly(),
                        null,
                        null,
                        false);
                }

                items.RemoveAt(items.Count - 1);

                return new Common.Models.PagedResult<Domain.Models.Order>(
                    domainList.ToList().AsReadOnly(),
                    GetNextCursorValue(filters, items),
                    items.Last().Id.ToString(),
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
            Expression<Func<OrderEntity, object>> keySelector = GetKeySelector(filters);

            if (filters.Orientation.Equals(ESortOrientation.Asc))
                query = query.OrderBy(keySelector).ThenBy(o => o.Id);
            else
                query = query.OrderByDescending(keySelector).ThenByDescending(o => o.Id);

            return query;
        }

        private IQueryable<OrderEntity> ApplyKeyset(IQueryable<OrderEntity> query, EOrderSort field, ESortOrientation orientation, string lastValue, int lastId)
        {
            var param = Expression.Parameter(typeof(OrderEntity), "o");
            var prop = Expression.Property(param, field.ToString());
            
            var constantValue = GetConstantValueParsed(lastValue, prop);
            var constVal = Expression.Constant(Convert.ChangeType(constantValue, prop.Type));

            var idProp = Expression.Property(param, nameof(OrderEntity.Id));
            var constId = Expression.Constant(lastId);

            Expression condition;
            if (orientation.Equals(ESortOrientation.Asc))
            {
                var greater = Expression.GreaterThan(prop, constVal);
                var equalAndId = Expression.AndAlso(Expression.Equal(prop, constVal), Expression.GreaterThan(idProp, constId));
                condition = Expression.OrElse(greater, equalAndId);
            }
            else
            {
                var less = Expression.LessThan(prop, constVal);
                var equalAndId = Expression.AndAlso(Expression.Equal(prop, constVal), Expression.LessThan(idProp, constId));
                condition = Expression.OrElse(less, equalAndId);
            }

            var lambda = Expression.Lambda<Func<OrderEntity, bool>>(condition, param);
            
            return query.Where(lambda);
        }

        private IQueryable<OrderEntity> ApplyFilter(
            IQueryable<OrderEntity> query, 
            EOrderSort field, 
            EFilterOperator op, 
            string filterValue)
        {
            var param = Expression.Parameter(typeof(OrderEntity), "o");
            var property = Expression.Property(param, field.ToString());
            var constantValue = GetConstantValueParsed(filterValue, property);
            
            var constant = Expression.Constant(Convert.ChangeType(constantValue, property.Type));

            Expression body = op switch
            {
                EFilterOperator.GraterThan => Expression.GreaterThan(property, constant),
                EFilterOperator.LessThan => Expression.LessThan(property, constant),
                EFilterOperator.GraterThanOrEqual => Expression.GreaterThanOrEqual(property, constant),
                EFilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(property, constant),
                _ => Expression.Equal(property, constant)
            };

            var lambda = Expression.Lambda<Func<OrderEntity, bool>>(body, param);
            return query.Where(lambda);
        }

        private static object GetConstantValueParsed(string filterValue, MemberExpression property)
        {
            object constantValue;
            var propertyType = Type.GetTypeCode(property.Type);

            switch (propertyType)
            {
                case TypeCode.Decimal:
                    var normalized = filterValue?.Replace('.', ',') ?? "0";
                    constantValue = Convert.ToDecimal(normalized);
                    break;
                case TypeCode.Int32:
                    constantValue = int.Parse(filterValue ?? "0");
                    break;
                case TypeCode.DateTime:
                    constantValue = DateTime.Parse(filterValue ?? "1970-01-01", CultureInfo.InvariantCulture);
                    break;
                default:
                    constantValue = filterValue ?? string.Empty;
                    break;
            }

            return constantValue;
        }

        private Expression<Func<OrderEntity, object>> GetKeySelector(OrderFilters filters)
        {
            switch (filters.SortBy)
            {
                case EOrderSort.EntryDate:
                    return order => order.EntryDate;
                case EOrderSort.Status:
                    return order => order.Status;
                case EOrderSort.TotalAmount:
                    return order => order.TotalAmount;
                case EOrderSort.ClientId:
                    return order => order.ClientId;
                default:
                    return order => order.Id;
            }
        }

        private static string GetNextCursorValue(
            OrderFilters filters,
            List<OrderEntity> items)
        {
            var lastItem = items.Last();
            
            var property = lastItem.GetType().GetProperty(filters.SortBy.ToString());

            if (property == null)
                return lastItem.Id.ToString();

            var value = property.GetValue(lastItem);
            return value?.ToString();
        }
    }
}
