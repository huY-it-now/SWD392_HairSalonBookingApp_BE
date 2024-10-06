using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{   enum DayOfWeek
    {
        Day_Off = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7,
    }
    public class OpeningHours : BaseEntity
    {
        public Guid SalonId { get; set; }
        public Salon Salon { get; set;}
        public enum DayOfWeek;
        public TimeOnly TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
    }
}
