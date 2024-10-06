using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BookingDetails : BaseEntity
    {
        #region RelationShip
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }
        public Guid ServiceId { get; set; }
        public Service Services { get; set; }
        public Guid ComboServiceId { get; set; }
        public ComboService ComboServices { get; set; }
        public Guid DiscountId { get; set; }
        public Discounts Discounts { get; set; }
        #endregion
    }
}
