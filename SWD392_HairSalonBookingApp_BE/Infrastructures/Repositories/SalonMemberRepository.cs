using Application.Interfaces;
using Domain.Entities;
using Infrastructures;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;

public class SalonMemberRepository : GenericRepository<SalonMember>, ISalonMemberRepository {
    private readonly AppDbContext _dbContext;

    public SalonMemberRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService) {
        _dbContext = dbContext;
    }

    public async Task<List<SalonMember>> GetAllSalonMember() {
        return await _dbContext.SalonMembers.Include(x => x.User).ToListAsync();
    }

    public async Task<List<SalonMember>> GetSalonMemberWithRole(int roleId) {
        return await _dbContext.SalonMembers.Include(x => x.User).Where(x => x.User.RoleId == roleId).ToListAsync();
    }

    public async Task<List<SalonMember>> GetSalonMemberBySalonId(Guid salonId) {
        return await _dbContext.SalonMembers.Where(x => x.SalonId == salonId)
            .Include(x => x.User).Include(x => x.Salon).ToListAsync();
    }

    public Task<List<SalonMember>> GetSalonMembersFree(DateTime dateTime, Salon salon)
    {
        throw new NotImplementedException();
    }
}