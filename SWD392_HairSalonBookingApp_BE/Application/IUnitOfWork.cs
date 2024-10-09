using Application.Repositories;

namespace Application
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync();
        IUserRepository UserRepository { get; }
        IComboServiceRepository ComboServiceRepository { get; }
        IComboDetailRepository ComboDetailRepository { get; }
    }
}
