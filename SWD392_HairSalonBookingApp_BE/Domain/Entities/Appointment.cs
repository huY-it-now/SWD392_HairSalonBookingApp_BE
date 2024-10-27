using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }

        #region Relationship
        public Guid StylistId { get; set; }
        public SalonMember Stylist { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ComboServiceId { get; set; }
        public ComboService ComboService { get; set; }
        #endregion
    }
}
