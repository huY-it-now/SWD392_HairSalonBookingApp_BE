using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<bool> CheckBooking(Guid bookingId, bool Check)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return false;
            }

            if (Check)
            {
                booking.Checked = true;

                _bookingRepository.Update(booking);
            }
            else
            {
                _bookingRepository.SoftRemove(booking);
            }

            return true;
        }

        public async Task<List<Booking>> ShowAllUncheckedBooking()
        {
            return await _bookingRepository.GetFullBookingInformation();
        }
    }
}
