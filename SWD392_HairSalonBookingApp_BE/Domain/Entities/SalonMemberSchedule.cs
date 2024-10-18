using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SalonMemberSchedule : BaseEntity
    {
        public DateTime Date { get; set; }
        public string WorkShift { get; set; }
        public bool IsDayOff { get; set; }

        #region RelationShip
        public Guid StylistId { get; set; }
        public SalonMember SalonMember { get; set; }
        #endregion
    }
}
