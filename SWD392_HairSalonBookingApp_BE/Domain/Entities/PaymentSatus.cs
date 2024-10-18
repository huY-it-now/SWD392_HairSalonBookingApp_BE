using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PaymentSatus : BaseEntity
    {
        public string StatusName { get; set; }
        public string Discription {  get; set; }
        public ICollection<Payments> Payments { get; set; }
    }
}
