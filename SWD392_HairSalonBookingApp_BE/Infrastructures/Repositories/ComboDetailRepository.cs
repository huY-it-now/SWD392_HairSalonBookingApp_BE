using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ComboDetailRepository : GenericRepository<ComboDetail>, IComboDetailRepository
    {
        private readonly AppDbContext _dbContext;

        public ComboDetailRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService)
            : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ComboDetail>> GetAllComboDetailsAsync()
        {
            return await _dbContext.ComboDetails.ToListAsync();
        }

        public async Task<ComboDetail> GetComboDetailById(Guid id)
        {
            return await _dbContext.ComboDetails.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
        }

        public async Task<ComboDetail> AddComboDetail(ComboDetail comboDetail)
        {
            await AddAsync(comboDetail);
            await _dbContext.SaveChangesAsync();
            return comboDetail;
        }

        public async Task<ComboDetail> UpdateComboDetail(ComboDetail comboDetail)
        {
            Update(comboDetail);
            await _dbContext.SaveChangesAsync();
            return comboDetail;
        }

        public async Task DeleteComboDetail(Guid id)
        {
            var comboDetail = await _dbSet.FirstOrDefaultAsync(cd => cd.Id == id);
            if (comboDetail != null)
            {
                SoftRemove(comboDetail);
                await _dbContext.SaveChangesAsync();
            }
        }

        // 
        public async Task<List<ComboService>> GetComboServicesByComboDetailId(Guid comboDetailId)
        {
            return await _dbContext.ComboServiceComboDetails
                                   .Where(cscd => cscd.ComboDetailId == comboDetailId)
                                   .Select(cscd => cscd.ComboService)
                                   .ToListAsync();
        }

        public async Task<ComboDetail> CheckComboDetailExistByName(string name)
        {
            return await _dbContext.ComboDetails.Where(x => x.IsDeleted == false).FirstOrDefaultAsync(n => n.Content == name);
        }

        public async Task<List<ComboDetail>> GetAllComboDetailIsDeleted()
        {
            return await _dbContext.ComboDetails.Where(x => x.IsDeleted == true).ToListAsync();
        }

        //public async Task<List<ComboDetail>> GetComboDetailsByComboServiceId(Guid comboServiceId)
        //{
        //    return await _dbContext.ComboServiceComboDetails
        //                           .Where(cscd => cscd.ComboServiceId == comboServiceId)
        //                           .Select(cscd => cscd.ComboDetail)
        //                           .ToListAsync();
        //}
    }
}
