using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces 
{
    public interface IBookingService
    {
        Task<List<Booking>> ShowAllUncheckedBooking();
        Task<bool> CheckBooking(Guid bookingId, bool Check);
    }
}
