using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderNowChallenge.Domain.Exceptions;

namespace OrderNowChallenge.DAL.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger<IUserRepository> _logger;
        private readonly OrderNowDbContext _dbContext;
        private readonly string CLASS_NAME = typeof(UserRepository).Name;

        public UserRepository(
            OrderNowDbContext dbContext,
            IMapper mapper,
            ILogger<IUserRepository> logger) 
        {
            _mapper = mapper;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Domain.Models.User> GetById(Guid id)
        {
            try
            {
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == id);

                return _mapper.Map<Domain.Models.User>(user);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(
                    ex,
                    $"An error ocurrs when getting a User by Id. At {CLASS_NAME}, GetById");

                throw new DataBaseContextException(
                    ex.Message,
                    ex);
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(
                    ex,
                    $"Timeout occurs when getting a User by Id. At {CLASS_NAME}, GetById");

                throw new DataBaseContextException(
                    ex.Message,
                    ex);
            }
        }

        public async Task<Domain.Models.User> GetByUsername(string username)
        {
            try
            {
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Username == username);

                return _mapper.Map<Domain.Models.User>(user);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(
                    ex,
                    $"An error ocurrs when getting a user. At {CLASS_NAME}, GetByUsername");

                throw new DataBaseContextException(
                    ex.Message,
                    ex);
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(
                    ex, 
                    $"Timeout occurs when getting a User. At {CLASS_NAME}, GetByUsername");

                throw new DataBaseContextException(
                    ex.Message, 
                    ex);
            }
        }
    }
}
