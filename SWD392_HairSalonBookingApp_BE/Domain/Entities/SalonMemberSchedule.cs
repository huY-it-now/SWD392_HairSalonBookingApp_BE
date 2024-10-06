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
        public TimeOnly TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }

        #region RelationShip
        public Guid SalonMenberId { get; set; }
        public SalonMember SalonMember { get; set; }
        #endregion
    }
}
