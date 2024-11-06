using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;

namespace Infrastructures.Repositories
{
    public class ServiceComboRepository : IServiceComboServiceRepository
    {
        private readonly AppDbContext _dbContext;

        public ServiceComboRepository(AppDbContext dbContext, 
                                      ICurrentTime timeService, 
                                      IClaimsService claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(ServiceComboService serviceComboService)
        {
            await _dbContext.AddAsync(serviceComboService);
        }
    }
}
