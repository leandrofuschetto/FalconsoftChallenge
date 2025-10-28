using RecruitingChallenge.Domain.Models;

namespace RecruitingChallenge.Service.Users
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> GetUserById(Guid id);
    }
}
