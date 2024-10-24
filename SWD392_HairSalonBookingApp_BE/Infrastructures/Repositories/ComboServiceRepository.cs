using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ComboServiceRepository : GenericRepository<ComboService>, IComboServiceRepository
    {
        private readonly AppDbContext _dbContext;

        public ComboServiceRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService)
            : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ComboService>> GetAllComboServiceAsync()
        {
            return await _dbContext.ComboServices.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<ComboService> GetComboServiceById(Guid id)
        {
            return await _dbContext.ComboServices.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
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

        public async Task DeleteComboService(Guid id)
        {
            var comboService = await _dbSet.FirstOrDefaultAsync(cs => cs.Id == id);
            if (comboService != null)
            {
                comboService.IsDeleted = true;
                comboService.DeleteBy = _claimsService.GetCurrentUserId;
                _dbSet.Update(comboService);
                await _dbContext.SaveChangesAsync();
            }
        }
     
        public async Task<List<ComboDetail>> GetComboDetailsByComboServiceId(Guid comboServiceId)
        {
            return await _dbContext.ComboServiceComboDetails
                                   .Where(cscd => cscd.ComboServiceId == comboServiceId)
                                   .Select(cscd => cscd.ComboDetail)
                                   .ToListAsync();
        }
    }
}
