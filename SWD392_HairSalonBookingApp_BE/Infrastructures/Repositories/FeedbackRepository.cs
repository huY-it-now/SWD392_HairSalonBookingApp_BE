using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        private readonly AppDbContext _dbContext;

        public FeedbackRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<Feedback> GetFeedbackById(Guid id)
        {
            return await _dbContext.Feedbacks.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
