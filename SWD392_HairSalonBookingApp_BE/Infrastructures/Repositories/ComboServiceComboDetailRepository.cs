using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class ComboServiceComboDetailRepository : IComboServiceComboDetailRepository
    {
        private readonly AppDbContext _dbContext;

        public ComboServiceComboDetailRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(ComboServiceComboDetail comboServiceComboDetail)
        {
            await _dbContext.AddAsync(comboServiceComboDetail);
        }

        public async Task<List<ComboDetail>> GetComboDetailsByComboServiceId(Guid comboServiceId)
        {
            return await _dbContext.ComboServiceComboDetails
                                         .Where(cscd => cscd.ComboServiceId == comboServiceId)
                                         .Select(cscd => cscd.ComboDetail)
                                         .Where(d => d.IsDeleted == false)
                                         .ToListAsync();
        }

        public async Task<List<ComboService>> GetComboServicesByComboDetailId(Guid comboDetailId)
        {
            return await _dbContext.ComboServiceComboDetails
                                         .Where(cscd => cscd.ComboDetailId == comboDetailId)
                                         .Select(cscd => cscd.ComboService)
                                         .ToListAsync();
        }
    }

}
