using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;

namespace Infrastructures.Repositories
{
    public class ComboServiceRepository : GenericRepository<ComboService>, IComboServiceRepository
    {
        public ComboServiceRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService)
            : base(dbContext, timeService, claimsService)
        {
        }
    }
}
