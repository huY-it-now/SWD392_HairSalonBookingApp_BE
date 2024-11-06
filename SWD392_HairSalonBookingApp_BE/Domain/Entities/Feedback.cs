using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Feedback : BaseEntity
    {
        public string RatingContent { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
