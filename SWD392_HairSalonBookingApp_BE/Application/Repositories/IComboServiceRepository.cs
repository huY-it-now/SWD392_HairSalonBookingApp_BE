using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IComboServiceRepository : IGenericRepository<ComboService>
    {
        Task<List<ComboService>> GetAllComboServiceAsync();
        Task<ComboService> GetComboServiceById(Guid id);
        Task<ComboService> AddComboService(ComboService comboService);
        Task<ComboService> UpdateComboService(ComboService comboService);
        Task<List<ComboServiceComboDetail>> GetComboDetailByComboServiceId(Guid comboServiceId);
    }
}
