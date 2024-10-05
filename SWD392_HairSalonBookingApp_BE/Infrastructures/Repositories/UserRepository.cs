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

        public async Task<List<User>> GetAllUserAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }
    }
}
