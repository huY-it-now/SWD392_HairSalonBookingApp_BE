using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts.DTO.Stylist
{
    public class WorkAndDayOffScheduleDTO
    {
        public DateTime Date { get; set; }
        public bool IsDayOff { get; set; }
        public List<string> WorkShifts { get; set; }
    }
}
