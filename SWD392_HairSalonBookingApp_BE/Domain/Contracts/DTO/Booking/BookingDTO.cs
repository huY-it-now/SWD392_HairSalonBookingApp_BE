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
    }
}
