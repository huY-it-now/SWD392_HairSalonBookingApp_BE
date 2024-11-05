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
    public class ScheduleWorkTimeRepository : GenericRepository<ScheduleWorkTime>, IScheduleWorkTimeRepository
    {
        private readonly AppDbContext _dbContext;

        public ScheduleWorkTimeRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public Task<List<ScheduleWorkTime>> GetByTime(int year, int month, int day)
        {
            return _dbContext.ScheduleWorkTime.Where(sms => sms.ScheduleDate.Day == day && sms.ScheduleDate.Year == year && sms.ScheduleDate.Month == month).OrderBy(sms => sms.ScheduleDate).ToListAsync();
        }
    }
}
