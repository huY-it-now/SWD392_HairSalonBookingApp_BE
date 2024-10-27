using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISalonMemberService
    {
        public Task<SalonMember> GetSalonMemberById(Guid id);
        Task<List<SalonMember>> GetAllStylists();
        Task<List<SalonMember>> GetAllSalonStaff();
    }
}
