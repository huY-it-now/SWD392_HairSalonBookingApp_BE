using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IScheduleRepository : IGenericRepository<SalonMemberSchedule>
    {
        Task<List<SalonMemberSchedule>> GetSchedulesByUserIdAndDateRange(Guid stylistId, DateTime fromDate, DateTime toDate);
        Task<SalonMemberSchedule> GetScheduleByDateAsync(Guid StylistId, DateTime date);
        Task<List<StylistDTO>> GetAvailableStylistsByTime(string shift, DateTime date);
        Task DeleteWorkShiftAsync(SalonMemberSchedule schedule);
    }
}
