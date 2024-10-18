using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IComboServiceService
    {
        public Task DeleteComboServiceAsync(Guid id);
        public Task UpdateComboServiceAsync(ComboService comboService);
        public Task<ComboService> GetComboServiceByIdAsync(Guid id);
        public Task<IEnumerable<ComboService>> GetAllComboServicesAsync();
        public Task<ComboService> CreateComboServiceAsync(ComboService comboService);
    }
}
