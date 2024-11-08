﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<Booking> GetBookingWithPayment(Guid id); 
        Task<List<Booking>> GetPendingBookingInformation();
        Task<List<Booking>> GetCheckedBookingInformation();
        Task<Booking> GetBookingByIdWithComboAndPayment(Guid id);
        Task<List<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingDetail(Guid bookingId);


        Task<List<Booking>> GetBookingsByStylistIdAndDateRange(Guid stylistId, DateTime fromDate, DateTime toDate);
        Task<Booking> GetBookingByIdAsync(Guid bookingId);
        Task<List<Booking>> GetAllBookingWithAllStatus();
        Task<List<Booking>> GetBookingForStylist(Guid stylistId);
        Task<List<Booking>> GetBookingUncompletedNow(Guid userId);
        Task<bool> AnyAsync(Expression<Func<Booking, bool>> predicate);
        Task<Booking> GetBookingBySalonAndDateAsync(Guid salonId, DateTime bookingDate);
        Task<Booking> GetBookingForStylistAsync(Guid bookingId);
    }
}
