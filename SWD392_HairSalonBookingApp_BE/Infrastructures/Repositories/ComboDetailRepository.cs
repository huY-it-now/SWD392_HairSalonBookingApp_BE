﻿using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ComboDetailRepository : GenericRepository<ComboDetail>, IComboDetailRepository
    {
        private readonly AppDbContext _dbContext;

        public ComboDetailRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ComboDetail>> GetAllComboDetailsAsync()
        {
            return await _dbContext.ComboDetails.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<ComboDetail> GetComboDetailById(Guid id)
        {
            return await _dbContext.ComboDetails.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
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
    }
}
