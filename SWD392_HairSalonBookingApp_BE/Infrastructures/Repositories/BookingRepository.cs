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
            return await _dbContext.Bookings.Include(x => x.ComboService).Include(x => x.Payments).ThenInclude(x => x.PaymentStatus).ToListAsync();
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

        public async Task<List<Booking>> GetUncheckBookingInformation()
        {
            return await _dbContext.Bookings
               .Where(b => b.BookingStatus == "UnCheck")
               .Include(b => b.Payments)
               .Include(b => b.ComboService)
               .ToListAsync();
        }
    }
}
