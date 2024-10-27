using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ScheduleRepository : GenericRepository<SalonMemberSchedule>, IScheduleRepository
    {
        private readonly AppDbContext _dbContext;
        public ScheduleRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<SalonMemberSchedule>> GetSchedulesByUserIdAndDateRange(Guid stylistId, DateTime fromDate, DateTime toDate)
        {
            return await _dbContext.SalonMemberSchedules.Where(s => s.SalonMemberId == stylistId && s.ScheduleDate >= fromDate && s.ScheduleDate <= toDate).ToListAsync();
        }

        public async Task<SalonMemberSchedule> GetScheduleByDateAsync(Guid stylistId, DateTime date)
        {
            return await _dbContext.SalonMemberSchedules.FirstOrDefaultAsync(s => s.SalonMemberId == stylistId && s.ScheduleDate == date);
        }

        public async Task<List<StylistDTO>> GetAvailableStylistsByShift(string shift, DateTime date)
        {
            var schedules = await _dbContext.SalonMemberSchedules
                .Where(s => s.ScheduleDate.Date == date.Date && // So sánh chỉ phần ngày
                            s.WorkShifts.Contains(shift) && // Kiểm tra ca làm việc
                            !s.IsDayOff) // Kiểm tra không phải ngày nghỉ
                .Select(s => new StylistDTO { Id = s.SalonMemberId })
                .ToListAsync();

            return schedules;
        }
    }
}
