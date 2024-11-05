using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<List<Appointment>> GetAppointmentsByStylistIdAndDateRange(Guid stylistId, DateTime fromDate, DateTime toDate);

        Task<Appointment> GetAppointmentByIdAsync(Guid appointmentId);

    }
}
