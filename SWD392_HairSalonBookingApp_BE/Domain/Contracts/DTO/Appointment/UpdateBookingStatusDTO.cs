using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.DTO.Appointment;

namespace Domain.Contracts.DTO.Appointment
{
    public class UpdateBookingStatusDTO
    {
        public Guid BookingId { get; set; }
        public string Status { get; set; }
    }
}
