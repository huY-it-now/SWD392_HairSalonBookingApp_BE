using Domain.Contracts.DTO.Combo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Booking
{
    public class BookingDTO
    {
        public Guid Id { get; set; }
        public DateTime BookingDate { get; set; }
        public bool Checked { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public List<ComboServiceForBookingDTO> ComboServiceName { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
    }
}
