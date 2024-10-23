using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Customer : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        #region Relationships
        public List<Appointment> Appointments { get; set; } 
        #endregion

        public Customer()
        {
            Appointments = new List<Appointment>();
        }
    }
}
