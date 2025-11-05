using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using OrderNowChallenge.DAL;
using OrderNowChallenge.Domain.Models;

namespace OrderNowChallenge.Service
{
    public interface IUnitOfWork
    {
        OrderNowDbContext DbContext { get; }
        User CurrentUser { get; }
        Task CommitChanges();
        Task BeginTransaction();
        Task RollbackTransaction();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderNowDbContext _dbContext;
        private IDbContextTransaction _currentTransaction;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnitOfWork(
            OrderNowDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public OrderNowDbContext DbContext => _dbContext;

        public User CurrentUser
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.Items["User"] as User;
                return user;
            }
        }

        public async Task BeginTransaction()
        {
            if (_currentTransaction != null)
                return;

            _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitChanges()
        {
            try
            {
                await _dbContext.SaveChangesAsync();

                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
            catch
            {
                await RollbackTransaction();
                throw;
            }
        }

        public async Task RollbackTransaction()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }
}
