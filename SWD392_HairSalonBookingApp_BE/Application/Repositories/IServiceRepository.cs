using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task<List<Service>> GetAllServicesAsync();
        Task<Service> GetServiceById(Guid id);
        Task<Service> CreateService(Service service);
    }
}
