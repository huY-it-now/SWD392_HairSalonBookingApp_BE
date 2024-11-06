using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application;
using Application.Interfaces;
using Application.Repositories;
using Domain.Contracts.Abstracts.Shared;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly AppDbContext _dbContext;

        public ServiceRepository(AppDbContext dbContext, 
                                 ICurrentTime timeService, 
                                 IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Service>> GetAllServicesAsync()
        {
            return await _dbContext
                            .Services
                            .Where(d => d.IsDeleted == false)
                            .Include(s => s.ServiceComboServices)
                            .ThenInclude(scs => scs.ComboService)
                            .ThenInclude(cs => cs.ComboServiceComboDetails) // Include ComboServiceComboDetails
                            .ThenInclude(csDetails => csDetails.ComboDetail)
                            .ToListAsync();
        }

        public async Task<Service> GetServiceById(Guid id)
        {
            return await _dbContext
                            .Services
                            .Where(d => d.IsDeleted == false)
                            .Include(s => s.ServiceComboServices)
                            .ThenInclude(scs => scs.ComboService)
                            .ThenInclude(cs => cs.ComboServiceComboDetails) // Include ComboServiceComboDetails
                            .ThenInclude(csDetails => csDetails.ComboDetail)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Service> CreateService(Service service)
        {
            var existingService = await _dbContext
                                            .Services
                                            .FirstOrDefaultAsync(s => s.ServiceName == service.ServiceName && 
                                                                 s.CategoryId == service.CategoryId);

            if (existingService != null)
            {
                throw new ArgumentException("A service with the same name already exists in this category!");
            }

            await _dbContext
                        .Services
                        .AddAsync(service);
            await _dbContext.SaveChangesAsync();

            return service;
        }

        public async Task<Service> UpdateService(Service service)
        {
            var existingService= await _dbContext
                                            .Services
                                            .FirstOrDefaultAsync(c => c.Id == service.Id);

            if (existingService == null)
            {
                return null;
            }

            existingService.ServiceName = service.ServiceName;

            _dbContext
                .Services
                .Update(existingService);

            await _dbContext.SaveChangesAsync();

            return existingService;
        }
    }
}
