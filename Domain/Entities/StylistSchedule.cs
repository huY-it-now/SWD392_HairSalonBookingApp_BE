using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class StylistSchedule
    {
        public Guid StylistId { get; set; }
        public SalonMember SalonMember { get; set; }
        public Guid ScheduleId { get; set; }
        public SalonMemberSchedule SalonMemberSchedule { get; set; }
    }
}
