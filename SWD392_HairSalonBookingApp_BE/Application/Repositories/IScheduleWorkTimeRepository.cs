using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IScheduleWorkTimeRepository : IGenericRepository<ScheduleWorkTime>
    {
        Task<List<ScheduleWorkTime>> GetByTime(int year, int month, int day);
    }
}
