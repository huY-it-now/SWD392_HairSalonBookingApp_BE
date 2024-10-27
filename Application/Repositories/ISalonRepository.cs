using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface ISalonRepository : IGenericRepository<Salon>
    {
        Task<List<Salon>> GetAllSalonAsync();
        Task<Salon> GetSalonByName(string salonName);
        Task<Salon> GetSalonById(Guid id);
    }
}
