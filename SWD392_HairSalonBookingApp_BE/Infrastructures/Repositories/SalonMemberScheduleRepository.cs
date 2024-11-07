using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class SalonMemberScheduleRepository : GenericRepository<SalonMemberSchedule>, ISalonMemberScheduleRepository
    {
        private readonly AppDbContext _dbContext;

        public SalonMemberScheduleRepository(AppDbContext dbContext, 
                                             ICurrentTime timeService, 
                                             IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<SalonMemberSchedule> GetByTime(int year, int month, int day)
        {
            return await _dbContext.SalonMemberSchedules
                                    .Where(sms => sms.ScheduleDate.Day == day && 
                                           sms.ScheduleDate.Year == year && 
                                           sms.ScheduleDate.Month == month)
                                    .OrderBy(sms => sms.ScheduleDate)
                                    .FirstOrDefaultAsync();
        }
    }
}
