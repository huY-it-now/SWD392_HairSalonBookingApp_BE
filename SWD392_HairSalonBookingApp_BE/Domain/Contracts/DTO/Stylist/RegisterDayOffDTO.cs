using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Stylist
{
    public class RegisterDayOffDTO
    {
        public Guid StylistId { get; set; }
        public DateTime DayOffDate { get; set; }
        public bool IsFullDay { get; set; }
    }
}
