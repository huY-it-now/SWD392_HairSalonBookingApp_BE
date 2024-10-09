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
        public ICollection<PaymentMethods> PaymentMethods { get; set; }
        public ICollection<PaymentSatus> PaymentSatus { get; set; }
        public ICollection<PaymentLogs> PaymentLogs { get; set; }
    }
}
