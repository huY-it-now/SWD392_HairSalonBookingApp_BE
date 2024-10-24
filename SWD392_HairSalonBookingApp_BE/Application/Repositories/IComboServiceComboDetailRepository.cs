using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IComboServiceComboDetailRepository
    {
        Task<List<ComboDetail>> GetComboDetailsByComboServiceId(Guid comboServiceId);
        Task<List<ComboService>> GetComboServicesByComboDetailId(Guid comboDetailId);
        Task AddAsync(ComboServiceComboDetail comboServiceComboDetail);
    }
}
