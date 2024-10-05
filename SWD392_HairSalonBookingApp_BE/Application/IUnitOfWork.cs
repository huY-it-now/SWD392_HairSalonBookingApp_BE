namespace Application
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync();
    }
}
