using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payments : BaseEntity
    {
        public Decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }

        public Guid PaymentMethodId { get; set; }
        public PaymentMethods PaymentMethods { get; set; }
        public Guid PaymentStatusId { get; set; }
        public PaymentSatus PaymentSatus { get; set; }
        public ICollection<PaymentLogs> PaymentLogs { get; set; }
    }
}
