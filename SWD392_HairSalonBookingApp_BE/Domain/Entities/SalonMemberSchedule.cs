using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SalonMemberSchedule : BaseEntity
    {
        public DateTime ScheduleDate { get; set; }
        public string WorkShift { get; set; }
        public bool IsDayOff { get; set; }
        public bool IsFullDay {  get; set; }
        public List<string> WorkShifts { get; set; }

        #region RelationShip
        public Guid StylistId { get; set; }
        public SalonMember SalonMember { get; set; }
        #endregion
    }
}
