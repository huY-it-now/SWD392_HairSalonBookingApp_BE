using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Booking;
using Domain.Contracts.DTO.Salon;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces 
{
    public interface IBookingService
    {
        Task<Result<object>> CreateBookingWithRequest(Guid CustomerId, Guid salonId, Guid SalonMemberId, DateTime cuttingDate, Guid ComboServiceId, string CustomerName, string CustomerPhoneNumber);
        Task<List<ViewUncheckBookingDTO>> ShowAllUncheckedBooking();
        Task<List<ViewCheckedBookingDTO>> ShowAllCheckedBooking();
        Task<string> CheckBooking(Guid bookingId, bool Check);
        Task<bool> CreateBooking(Booking booking);
        Task<bool> UpdateBooking(Booking booking);
        Task<Booking> GetBookingById(Guid Id);
        Task<BookingDTO> AddRandomStylist(Guid Id);
        Task<Result<object>> AddFeedBack(Guid bookingId, string FeedBack);
    }
}
