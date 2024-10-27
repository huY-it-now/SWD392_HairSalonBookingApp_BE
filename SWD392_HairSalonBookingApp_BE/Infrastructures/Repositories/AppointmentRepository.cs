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
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly AppDbContext _dbContext;

        public AppointmentRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService)
            : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Appointment>> GetAppointmentsByStylistIdAndDateRange(Guid stylistId, DateTime fromDate, DateTime toDate)
        {
            return await _dbContext.Appointments.Where(a => a.StylistId == stylistId && a.AppointmentDate >= fromDate && a.AppointmentDate <= toDate).ToListAsync();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(Guid appointmentId)
        {
            return await _dbContext.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId);
        }
    }
}
