using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Booking
{
    public class ViewPendingBookingDTO
    {
        public Guid BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public Decimal Total { get; set; }
    }
}
