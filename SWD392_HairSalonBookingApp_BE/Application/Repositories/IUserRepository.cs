using Domain.Contracts.Abstracts.Shared;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<List<User>> GetAllUserAsync();
        Task<bool> CheckEmailExist(string email);
        Task<User> GetUserByEmail(string email);
        Task<User> Verify(string token);
        Task<User> GetUserById(Guid id);
        Task<List<Booking>> GetBookingsByUserId(Guid userId);
    }
}
