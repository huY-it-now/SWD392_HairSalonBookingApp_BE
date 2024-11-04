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

        public async Task<List<StylistDTO>> GetAvailableStylistsByTime(string shift, DateTime date, Guid salonId)
        {
            var schedules = await _dbContext.SalonMemberSchedules
        .Include(s => s.SalonMember)
            .ThenInclude(sm => sm.User)
        .Where(s => s.ScheduleDate.Date == date.Date &&
                    s.WorkShifts.Contains(shift) &&
                    !s.IsDayOff &&
                    s.SalonMember.SalonId == salonId)
        .Select(s => new StylistDTO
        {
            Id = s.SalonMember.Id,
            FullName = s.SalonMember.User.FullName ?? string.Empty,
            Email = s.SalonMember.User.Email ?? string.Empty,
            Job = s.SalonMember.Job ?? string.Empty,
            Rating = s.SalonMember.Rating ?? "No Rating",
            Status = s.SalonMember.User.Status
        })
        .ToListAsync();

            return schedules;
        }

        public async Task DeleteWorkShiftAsync(SalonMemberSchedule schedule)
        {
            _dbContext.SalonMemberSchedules.Remove(schedule);
            await _dbContext.SaveChangesAsync();
        }
    }
}
