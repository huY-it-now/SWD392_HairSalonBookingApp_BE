using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Stylist
{
    public class WorkShiftDTO
    {
        public string Shift { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public static List<WorkShiftDTO> GetAvailableShifts()
        {
            return new List<WorkShiftDTO>
            {
                new WorkShiftDTO { Shift = "Morning", StartTime = new TimeSpan(7, 0, 0), EndTime = new TimeSpan(12, 0, 0) },
                new WorkShiftDTO { Shift = "Afternoon", StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(17, 0, 0) },
                new WorkShiftDTO { Shift = "Evening", StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(22, 0, 0) }
            };
        }
    }
}
