using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Discounts : BaseEntity
    {
        public string DiscountName { get; set; }
        public Decimal DiscountValue { get; set; }
        public ICollection<BookingDetails> BookingDetails { get; set; }
    }
}
