using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Appointment
{
    public class AppointmentDTO
    {
        public string CustomerName { get; set; }
        public string ServiceName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }

        public string AppointmentTime => AppointmentDate.ToString("HH:mm");
        public string AppointmentDay => AppointmentDate.ToString("dddd, dd MMMM yyyy");
    }
}
