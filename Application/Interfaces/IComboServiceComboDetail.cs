using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IComboServiceComboDetail
    {
        Task<List<ComboDetail>> GetComboDetailsByComboServiceId(Guid comboServiceId);
        Task<List<ComboService>> GetComboServicesByComboDetailId(Guid comboDetailId);
    }
}
