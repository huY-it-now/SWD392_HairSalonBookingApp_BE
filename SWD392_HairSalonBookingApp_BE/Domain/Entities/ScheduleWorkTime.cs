using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ScheduleWorkTime : BaseEntity
    {
        public DateTime ScheduleDate { get; set; }
        public string WorkShifts { get; set; }
        public Guid SalonMemberScheduleId { get; set; }
        public SalonMemberSchedule SalonMemberSchedule { get; set; }
    }
}
