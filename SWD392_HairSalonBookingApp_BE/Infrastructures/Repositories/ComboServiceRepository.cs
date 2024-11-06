using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ComboServiceRepository : GenericRepository<ComboService>, IComboServiceRepository
    {
        private readonly AppDbContext _dbContext;

        public ComboServiceRepository(AppDbContext dbContext, 
                                      ICurrentTime timeService, 
                                      IClaimsService claimsService)
            : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ComboService>> GetAllComboServiceAsync()
        {
            return await _dbContext.ComboServices
                                        .Include(cs => cs.ComboServiceComboDetails)
                                        .ThenInclude(cs => cs.ComboDetail)
                                        .ToListAsync();
        }

        public async Task<ComboService> GetComboServiceById(Guid id)
        {
            return await _dbContext.ComboServices
                                        .Where(d => d.IsDeleted == false)
                                        .Include(cs => cs.ComboServiceComboDetails)
                                        .ThenInclude(cs => cs.ComboDetail)
                                        .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<ComboService> AddComboService(ComboService comboService)
        {
            comboService.CreationDate = _timeService.GetCurrentTime();
            comboService.CreatedBy = _claimsService.GetCurrentUserId;

            await _dbSet.AddAsync(comboService);
            await _dbContext.SaveChangesAsync();

            return comboService;
        }

        public async Task<ComboService> UpdateComboService(ComboService comboService)
        {
            comboService.ModificationDate = _timeService.GetCurrentTime();
            comboService.ModificationBy = _claimsService.GetCurrentUserId;

            _dbSet.Update(comboService);

            await _dbContext.SaveChangesAsync();

            return comboService;
        }

        public async Task<List<ComboServiceComboDetail>> GetComboDetailByComboServiceId(Guid comboServiceId)
        {
            return await _dbContext.ComboServiceComboDetails
                                            .Include(detail => detail.ComboDetail) // Eagerly load ComboDetail
                                            .Where(detail => detail.ComboServiceId == comboServiceId)
                                            .ToListAsync();
        }
    }
}
