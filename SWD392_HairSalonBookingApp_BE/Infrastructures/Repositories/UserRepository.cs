using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CheckEmailExist(string email)
        {
            return await _dbContext.Users.AnyAsync(e => e.Email == email);
        }

        public async Task<List<SalonMember>> GetAllManager()
        {
            return await _dbContext.SalonMembers.Include(x => x.User).Where(x => x.User.RoleId == 3).Include(x => x.Salon).ToListAsync();
        }

        public async Task<List<SalonMember>> GetAllStaff()
        {
            return await _dbContext.SalonMembers.Include(x => x.User).Where(x => x.User.RoleId == 4).Include(x => x.Salon).ToListAsync();
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsByUserId(Guid userId)
        {
            return await _dbContext.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Payments)       // Include Payments entity
                .Include(b => b.ComboService)   // Include ComboService entity
                .ToListAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.Users.Include(x => x.Role).FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<User> Verify(string token)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(t => t.VerificationToken == token);
        }
    }
}
