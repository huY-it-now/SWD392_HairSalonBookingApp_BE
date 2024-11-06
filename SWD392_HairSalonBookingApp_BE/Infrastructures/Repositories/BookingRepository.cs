using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Infrastructures.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly AppDbContext _dbContext;

        public BookingRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _dbContext.Bookings.Include(x => x.SalonMember).ThenInclude(x => x.User).Include(x => x.ComboService).Include(x => x.Payments).ThenInclude(x => x.PaymentStatus).ToListAsync();
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
            return await _dbContext.Bookings.Include(x => x.ComboService).Include(x => x.Payments).Where(x => x.Id == bookingId).FirstOrDefaultAsync();
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
               .Include(b => b.Payments)
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
            return await _dbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.ComboService)
                .Include(b => b.salon)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<List<Booking>> GetAllBookingWithAllStatus()
        {
            return await _dbContext.Bookings.Include(x => x.SalonMember).ThenInclude(x => x.User).Include(x => x.ComboService).Include(x => x.Payments).ThenInclude(x => x.PaymentStatus).ToListAsync();
        }

        public async Task<List<Booking>> GetBookingForStylist(Guid stylistId)
        {
            return await _dbContext.Bookings
                    .Where(x => x.SalonMemberId == stylistId)
                    .Include(x => x.ComboService)
                    .ToListAsync();
        }
    }
}
