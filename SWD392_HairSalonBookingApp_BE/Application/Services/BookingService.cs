using Application.Interfaces;
using Application.Repositories;
using AutoMapper;
using Domain.Contracts.DTO.Booking;
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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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

        public async Task<bool> CreateBooking(Booking booking)
        {
            if (booking != null)
            {
                await _bookingRepository.AddAsync(booking);
            }
            else
            {
                return false;
            }

            return await  _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<Booking> GetBookingById(Guid Id)
        {
            return await _bookingRepository.GetByIdAsync(Id);
        }

        public async Task<List<BookingDTO>> ShowAllUncheckedBooking()
        {
            return _mapper.Map<List<BookingDTO>>(await _bookingRepository.GetFullBookingInformation());
        }

        public async Task<bool> UpdateBooking(Booking booking)
        {
            _bookingRepository.Update(booking);

            return await _unitOfWork.SaveChangeAsync() > 0;
        }
    }
}
