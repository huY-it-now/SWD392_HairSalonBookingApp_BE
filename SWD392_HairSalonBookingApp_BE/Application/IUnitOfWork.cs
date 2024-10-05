using Application.Repositories;

namespace Application
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync();
        public IUserRepository UserRepository { get; }
    }
}
