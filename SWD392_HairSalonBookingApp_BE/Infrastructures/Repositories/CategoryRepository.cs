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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepository(AppDbContext dbContext, 
                                  ICurrentTime timeService, 
                                  IClaimsService claimsService) : base(dbContext, timeService, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Category>> GetAllCategoryAsync()
        {
            return await _dbContext
                            .Categories
                            .Where(d => d.IsDeleted == false)
                            .Include(c => c.Services)
                            .ToListAsync();
        }

        public async Task<Category> GetCategoryById(Guid id)
        {
            return await _dbContext
                            .Categories
                            .Where(d => d.IsDeleted == false)
                            .Include(c => c.Services)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category> CreateCategory(Category category)
        {
            var existingCategory = await _dbContext
                                            .Categories
                                            .FirstOrDefaultAsync(c => c.CategoryName == category.CategoryName);
            if (existingCategory != null)
            {
                return null;
            }

            await _dbContext
                        .Categories
                        .AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            var existingCategory = await _dbContext
                                            .Categories
                                            .FirstOrDefaultAsync(c => c.Id == category.Id);

            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.CategoryName = category.CategoryName;
            _dbContext
                .Categories
                .Update(existingCategory);
            await _dbContext.SaveChangesAsync();

            return existingCategory;
        }
    }
}
