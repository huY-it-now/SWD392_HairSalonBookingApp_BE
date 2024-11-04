using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface ISalonMemberScheduleRepository : IGenericRepository<SalonMemberSchedule>
    {
        Task<SalonMemberSchedule> GetByTime(int year, int month, int day);
    }
}
