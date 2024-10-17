using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class SalonRepository : GenericRepository<Salon>, ISalonRepository
    {
        private readonly AppDbContext _dbContext;

        public SalonRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Salon>> GetAllSalonAsync()
        {
            return await _dbContext.Salons.ToListAsync();
        }
    }
}
