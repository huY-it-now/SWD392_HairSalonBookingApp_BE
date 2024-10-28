using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Booking
{
    public class ViewCheckedBookingDTO
    {
        public Guid BookingId { get; set; }
        public string? Feedback { get; set; }
        public DateTime BookingDate { get; set; }
        public bool Checked { get; set; }
        public Decimal Total {  get; set; }
        public string PaymentStatus { get; set; }
    }
}
