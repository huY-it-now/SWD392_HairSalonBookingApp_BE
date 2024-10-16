using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly AppDbContext _dbContext;

        public ServiceRepository(AppDbContext dbContext, ICurrentTime timeService, IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Service>> GetAllServicesAsync()
        {
            return await _dbContext.Services.ToListAsync();
        }

        public async Task<Service> GetServiceById(Guid id)
        {
            return await _dbContext.Services.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
