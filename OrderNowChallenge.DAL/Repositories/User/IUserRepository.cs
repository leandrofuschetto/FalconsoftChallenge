namespace OrderNowChallenge.DAL.Repositories.User
{
    public interface IUserRepository
    {
        Task<Domain.Models.User> GetByUsername(string username);
        Task<Domain.Models.User> GetById(Guid id);

    }
}
