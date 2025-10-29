using RecruitingChallenge.Common.Helpers;
using RecruitingChallenge.DAL.Repositories.User;
using RecruitingChallenge.Domain.Exceptions;
using RecruitingChallenge.Domain.Models;

namespace RecruitingChallenge.Service.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _userRepository.GetByUsername(username);

            if (user == null)
                throw new AuthenticationErrorException("Authentication error");

            if (Hasher.HashPassword(password, user.Salt) != user.Password)
                throw new AuthenticationErrorException("Authentication error");
            
            return user;
        }

        public async Task<User> GetUserById(Guid id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
                throw new UserNotFoundException("User not found");

            return user;
        }
    }
}
