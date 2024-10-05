using Domain.Contracts.Abstracts.Shared;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<List<User>> GetAllUserAsync();
        Task<bool> CheckEmailExist(string email);
    }
}
