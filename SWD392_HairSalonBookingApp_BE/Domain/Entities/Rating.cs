using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Rating : BaseEntity
    {
        public string RatingContent { get; set; }
        public Guid SalonId { get; set; }
        public Salon Salon { get; set;}
    }
}
