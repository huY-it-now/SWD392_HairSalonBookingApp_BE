using Domain.Contracts.DTO.Booking;
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
        Task<List<BookingDTO>> ShowAllUncheckedBooking();
        Task<bool> CheckBooking(Guid bookingId, bool Check);
        Task<bool> CreateBooking(Booking booking);
        Task<bool> UpdateBooking(Booking booking);
        Task<Booking> GetBookingById(Guid Id);
    }
}
