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

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
        #endregion
    }
}
