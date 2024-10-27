using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PaymentLogs : BaseEntity
    {
        public DateTime Timestamped { get; set; }   
        public string LogMessage { get; set; }
        public Guid PaymentId { get; set; }
        public Payments Payments { get; set; }
    }
}
