using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Stylish
{
    public class ScheduleDTO
    {
        public Guid StylistId { get; set; }
        public DateTime Date {  get; set; }
        public string WorkShift { get; set; }
        public bool IsDayOff { get; set; }
    }
}
