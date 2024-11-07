using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Infrastructures.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly AppDbContext _dbContext;

        public BookingRepository(AppDbContext dbContext, 
                                 ICurrentTime timeService, 
                                 IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _dbContext.Bookings
                                        .Include(x => x.SalonMember)
                                        .ThenInclude(x => x.User)
                                        .Include(x => x.ComboService)   
                                        .Include(x => x.Payments)
                                        .ThenInclude(x => x.PaymentStatus)
                                        .ToListAsync();
        }

        public async Task<Booking> GetBookingByIdWithComboAndPayment(Guid id)
        {
            return await _dbContext.Bookings
                                        .Where(b => b.Id == id)
                                        .Include(b => b.Payments)
                                        .Include(b => b.ComboService)
                                        .SingleOrDefaultAsync();
        }

        public async Task<Booking> GetBookingDetail(Guid bookingId)
        {
            return await _dbContext.Bookings
                                        .Include(x => x.Feedback)
                                        .Include(x => x.SalonMember)
                                        .ThenInclude(x => x.User)
                                        .Include(x => x.ComboService)
                                        .Include(x => x.Payments)
                                        .ThenInclude(x => x.PaymentStatus)
                                        .Where(x => x.Id == bookingId)
                                        .FirstOrDefaultAsync();
        }

        public async Task<Booking> GetBookingWithPayment(Guid id)
        {
            return await _dbContext.Bookings
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(booking => booking.Id == id);
        }

        public async Task<List<Booking>> GetCheckedBookingInformation()
        {
            return await _dbContext.Bookings
                                       .Where(b => b.BookingStatus == "Checked")
                                       .Include(b => b.User)
                                       .Include(b => b.Payments).ThenInclude(x => x.PaymentStatus)
                                       .Include(b => b.SalonMember)
                                       .Include(b => b.ComboService)
                                       .ToListAsync();
        }

        public async Task<List<Booking>> GetPendingBookingInformation()
        {
            return await _dbContext.Bookings
                                       .Where(b => b.BookingStatus == "Pending")
                                       .Include(b => b.Payments)
                                       .Include(b => b.ComboService)
                                       .ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsByStylistIdAndDateRange(Guid stylistId, DateTime fromDate, DateTime toDate)
        {
            return await _dbContext.Bookings
                                        .Where(b => b.BookingDate >= fromDate && b.BookingDate <= toDate)
                                        .Where(b => b.SalonMemberId == stylistId && b.BookingDate >= fromDate && b.BookingDate <= toDate)
                                        .Include(b => b.User)
                                        .Include(b => b.ComboService)
                                        .ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            return await _dbContext.Bookings.Where(x => x.BookingStatus == "Completed")
                                        .Include(b => b.User)
                                        .Include(b => b.ComboService)
                                        .Include(b => b.salon)
                                        .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<List<Booking>> GetAllBookingWithAllStatus()
        {
            return await _dbContext.Bookings
                                        .Include(x => x.Feedback)
                                        .Include(x => x.SalonMember)
                                        .ThenInclude(x => x.User)
                                        .Include(x => x.ComboService)
                                        .Include(x => x.Payments)
                                        .ThenInclude(x => x.PaymentStatus)
                                        .ToListAsync();
        }

        public async Task<List<Booking>> GetBookingForStylist(Guid stylistId)
        {
            return await _dbContext.Bookings.Where(x => x.BookingStatus == "Checked")
                .Include(x => x.Feedback).Include(x => x.SalonMember).Include(x => x.salon)
                                        .Where(x => x.SalonMemberId == stylistId).Include(x => x.User).Include(x => x.Payments).ThenInclude(x => x.PaymentStatus)
                                        .Include(x => x.ComboService)
                                        .ToListAsync();
        }

        public Task<List<Booking>> GetBookingUncompletedNow(Guid userId)
        {
            return _dbContext.Bookings
                                .Where(x => x.BookingStatus != "Completed")
                                .Include(x => x.salon)
                                .Include(x => x.SalonMember)
                                .ThenInclude(x => x.User).Where(x => x.UserId == userId)
                                .Include(x => x.ComboService)
                                .ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<Booking, bool>> predicate)
        {
            return await _dbContext.Bookings
                                        .AnyAsync(predicate);
        }

        public async Task<Booking> GetBookingBySalonAndDateAsync(Guid salonId, DateTime bookingDate)
        {
            return await _dbContext.Set<Booking>()
            .Where(b => b.SalonId == salonId && b.BookingDate == bookingDate)
            .FirstOrDefaultAsync();
        }
    }
}
