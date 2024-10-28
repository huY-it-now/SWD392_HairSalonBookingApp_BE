using Domain.Entities;

namespace Application.Repositories
{
    public interface IServiceComboServiceRepository
    {
        Task AddAsync(ServiceComboService serviceComboService);
    }
}
