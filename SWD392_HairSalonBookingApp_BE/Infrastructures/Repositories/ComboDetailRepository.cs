using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;

namespace Infrastructures.Repositories
{
    public class ComboDetailRepository : GenericRepository<ComboDetail>, IComboDetailRepository
    {
        public ComboDetailRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService)
            : base(dbContext, timeService, claimsService)
        {
        }
    }
}
