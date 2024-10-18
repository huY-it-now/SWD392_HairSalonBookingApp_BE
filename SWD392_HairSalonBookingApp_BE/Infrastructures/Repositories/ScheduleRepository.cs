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
            return await _dbContext.SalonMemberSchedules.Where(s => s.StylistId == stylistId && s.Date >= fromDate && s.Date <= toDate).ToListAsync();
        }
    }
}
